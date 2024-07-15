using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace GameFramework.Toolkit.Editor
{
    public static class InputConfig
    {
        public const string ConfigVersion = "v1.0.0";

        public const string XmlVersion = "Version";

        /// <summary>
        /// 导出Xml配置
        /// </summary>
        /// <param name="savePath"></param>
        public static void ExportXmlConfig(string savePath, InputEditorConfig editorConfig)
        {
            if (editorConfig == null)
            {
                throw new Exception("InputEditorConfig为空");
            }

            if (editorConfig.DeviceGroups == null)
            {
                throw new Exception("DeviceGroups为空");
            }

            if (File.Exists(savePath))
                File.Delete(savePath);

            // 创建一个新的XmlDocument对象
            XmlDocument doc = new XmlDocument();

            // 创建XML声明部分
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            
            //创建根节点
            XmlElement InputConfigElement = doc.CreateElement(string.Empty, "InputSetting", string.Empty);
            doc.AppendChild(InputConfigElement);

            InputConfigElement.SetAttribute("SaveOnExit", editorConfig.SaveOnExit.ToString());

            foreach (var group in editorConfig.DeviceGroups)
            {
                //创建组
                XmlElement groupElement = doc.CreateElement(string.Empty, "Group", string.Empty);
                groupElement.SetAttribute("Name", group.GroupName);
                groupElement.SetAttribute("CacheName", group.CacheName);
                groupElement.SetAttribute("Enable", group.Enable.ToString());

                foreach (var device in group.InputDevices)
                {
                    //创建设备
                    XmlElement deviceElement = doc.CreateElement(string.Empty, "Device", string.Empty);
                    deviceElement.SetAttribute("TypeName", device.GetType().FullName);
                    deviceElement.SetAttribute("Enable", device.Enable.ToString());
                    deviceElement.SetAttribute("Foldout", device.Foldout.ToString());

                    foreach (var action in device.Actions)
                    {
                        //创建行为
                        XmlElement actionElement = doc.CreateElement(string.Empty, "Action", string.Empty);
                        
                        actionElement.SetAttribute("TypeName", action.BaseType.FullName);
                        
                        SerializationFieldElement(action, actionElement);

                        deviceElement.AppendChild(actionElement);
                    }

                    groupElement.AppendChild(deviceElement);
                }


                InputConfigElement.AppendChild(groupElement);
            }

            doc.Save(savePath);
        }

        /// <summary>
        /// 导入Xml配置
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static InputEditorConfig ImportXmlConfig(string filePath)
        {
            if (File.Exists(filePath) == false)
                return null;

            if (Path.GetExtension(filePath) != ".xml")
                throw new Exception($"Only support xml : {filePath}");

            InputEditorConfig editorConfig = new InputEditorConfig();
            editorConfig.DeviceGroups = new List<InputGroupDataEditor>();

            // 加载配置文件
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlElement root = xmlDoc.DocumentElement;

            if (root.HasAttribute("SaveOnExit") == false)
            {
                throw new Exception($"Not found attribute SaveOnExit in Root");
            }
            editorConfig.SaveOnExit = root.GetAttribute("SaveOnExit") == "True" ? true : false;

            var groupList = root.GetElementsByTagName("Group");

            foreach (var groupNode in groupList)
            {
                XmlElement groupElement = groupNode as XmlElement;
                if (groupElement.HasAttribute("Name") == false)
                    throw new Exception($"Not found attribute Name in Group");
                if (groupElement.HasAttribute("CacheName") == false)
                    throw new Exception($"Not found attribute CacheName in Group");
                if (groupElement.HasAttribute("Enable") == false)
                    throw new Exception($"Not found attribute Enable in Group");

                var group = new InputGroupDataEditor();
                group.GroupName = groupElement.GetAttribute("Name");
                group.CacheName = groupElement.GetAttribute("CacheName");
                group.Enable = groupElement.GetAttribute("Enable") == "True" ? true : false;
                group.InputDevices = new List<InputDeviceDataEditor>();

                var deviceList = groupElement.GetElementsByTagName("Device");

                foreach (var deviceNode in deviceList)
                {
                    XmlElement deviceElement = deviceNode as XmlElement;
                    if (deviceElement.HasAttribute("TypeName") == false)
                        throw new Exception($"Not found attribute TypeName in Device");
                    if (deviceElement.HasAttribute("Enable") == false)
                        throw new Exception($"Not found attribute Enable in Device");
                    if (deviceElement.HasAttribute("Foldout") == false)
                        throw new Exception($"Not found attribute Foldout in Device");
                    
                    var typeName = deviceElement.GetAttribute("TypeName");
                    var device = (InputDeviceDataEditor)Activator.CreateInstance(Type.GetType(typeName));
                    device.Enable = deviceElement.GetAttribute("Enable") == "True" ? true : false;
                    device.Foldout = deviceElement.GetAttribute("Foldout") == "True" ? true : false;
                    device.Actions = new List<InputActionDataEditor>();

                    var actionList = deviceElement.GetElementsByTagName("Action");

                    foreach (var actionNode in actionList)
                    {
                        XmlElement actionElement = actionNode as XmlElement;
                        if (actionElement.HasAttribute("TypeName") == false)
                            throw new Exception($"Not found attribute TypeName in Action");

                        var actionTypeName = actionElement.GetAttribute("TypeName");
                        var action = (InputActionDataEditor)Activator.CreateInstance(Type.GetType(actionTypeName));

                        DeSerializationFieldElement(action, actionElement);

                        device.Actions.Add(action);
                    }

                    group.InputDevices.Add(device); 
                }

                editorConfig.DeviceGroups.Add(group);
            } 

            return editorConfig;
        }
          
        private static void SerializationFieldElement(object obj, XmlElement parent)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(obj);
                if (value != null)
                {
                    XmlElement fieldElement = parent.OwnerDocument.CreateElement(field.Name);

                    if (IsCollectionType(field.FieldType))
                    {
                        foreach (var item in (IEnumerable)value)
                        {
                            XmlElement itemElement = parent.OwnerDocument.CreateElement("Item");
                            SerializeValue(item, itemElement);
                            fieldElement.AppendChild(itemElement);
                        }
                    }
                    else
                    {
                        SerializeValue(value, fieldElement);
                    }
                    parent.AppendChild(fieldElement);
                }
            }
        }
        
        private static void DeSerializationFieldElement(object obj, XmlElement actionElement)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {

                //获取字段节点
                XmlNodeList fieldNodes = actionElement.SelectNodes(field.Name);
                if (fieldNodes.Count == 0)
                {
                    continue;
                } 

                if (IsCollectionType(field.FieldType))
                {
                    IList list = (IList)Activator.CreateInstance(field.FieldType);
                    Type itemType = field.FieldType.IsArray ? field.FieldType.GetElementType() : field.FieldType.GetGenericArguments()[0];

                    foreach (XmlElement fieldNode in fieldNodes)
                    {
                        XmlNodeList itemNodes = fieldNode.SelectNodes("Item");
                        foreach (XmlElement itemElement in itemNodes)
                        {
                            object itemValue = DeserializeValue(itemElement, itemType);
                            list.Add(itemValue);
                        }
                    }

                    if (field.FieldType.IsArray)
                    {
                        Array array = Array.CreateInstance(itemType, list.Count);
                        list.CopyTo(array, 0);
                        field.SetValue(obj, array);
                    }
                    else
                    {
                        field.SetValue(obj, list);  
                    }
                }
                else
                {
                    object fieldValue = DeserializeValue((XmlElement)fieldNodes[0], field.FieldType);
                    field.SetValue(obj, fieldValue);
                }
            }
        }

        /// <summary>
        /// 序列化数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="element"></param>
        private static void SerializeValue(object value, XmlElement element)
        {
            if (value.GetType().IsEnum)
            {
                element.InnerText = Convert.ToInt32(value).ToString();
            }
            else if (IsComplexType(value.GetType()))
            {
                SerializationFieldElement(value, element);
            }
            else
            {
                element.InnerText = value.ToString();
            }
        }

        private static object DeserializeValue(XmlElement element, Type type)
        {
            if (type.IsEnum)
            {
                return Enum.ToObject(type, int.Parse(element.InnerText));
            }
            else if (IsComplexType(type))
            {
                object complexValue = Activator.CreateInstance(type);
                DeSerializationFieldElement(complexValue, element);
                return complexValue;
            }
            else
            {
                return Convert.ChangeType(element.InnerText, type);
            }
        }

        /// <summary>
        /// 是否是Array或List类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsCollectionType(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) 
                || type.IsArray;
        }

        /// <summary>
        /// 是否是复杂类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsComplexType(Type type)
        {
            return type.IsClass && type != typeof(string);
        }
    }
}
