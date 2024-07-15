using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public static class EditorUtilities
    {
        static Dictionary<string, GUIContent> s_GUIContentCache;

        static EditorUtilities()
        {
            s_GUIContentCache = new Dictionary<string, GUIContent>();
        }

        /// <summary>
        /// 获取文本内容
        /// </summary>
        /// <param name="textAndTooltip"></param>
        /// <returns></returns>
        public static GUIContent GetContent(string textAndTooltip)
        {
            if (string.IsNullOrEmpty(textAndTooltip))
                return GUIContent.none;

            GUIContent content;

            if (!s_GUIContentCache.TryGetValue(textAndTooltip, out content))
            {
                var s = textAndTooltip.Split('|');
                content = new GUIContent(s[0]);

                if (s.Length > 1 && !string.IsNullOrEmpty(s[1]))
                    content.tooltip = s[1];

                s_GUIContentCache.Add(textAndTooltip, content);
            }

            return content;
        }

        public static void DrawItem(object obj, FieldInfo fieldInfo)
        {
            if (obj == null || fieldInfo == null)
            {
                EditorGUILayout.LabelField("Null");
                return;
            }

            object item = fieldInfo.GetValue(obj);
            Type type = fieldInfo.FieldType;

            if (type == typeof(int))
            {
                int intValue = (int)item;
                int newValue = EditorGUILayout.IntField(fieldInfo.Name, intValue);
                if (newValue != intValue)
                {
                    fieldInfo.SetValue(obj, newValue);
                }
            }
            else if (type == typeof(float))
            {
                float floatValue = (float)item;
                float newValue = EditorGUILayout.FloatField(fieldInfo.Name, floatValue);
                if (newValue != floatValue)
                {
                    fieldInfo.SetValue(obj, newValue);
                }
            }
            else if (type == typeof(string))
            {
                string stringValue = (string)item;
                string newValue = EditorGUILayout.TextField(fieldInfo.Name, stringValue);
                if (newValue != stringValue)
                {
                    fieldInfo.SetValue(obj, newValue);
                }
            }
            else if (type == typeof(bool))
            {
                bool boolValue = (bool)item;
                bool newValue = EditorGUILayout.Toggle(fieldInfo.Name, boolValue);
                if (newValue != boolValue)
                {
                    fieldInfo.SetValue(obj, newValue);
                }
            }
            else if (type.IsEnum)
            {
                Enum enumValue = (Enum)item;
                Enum newValue = EditorGUILayout.EnumPopup(fieldInfo.Name, enumValue);
                if (!newValue.Equals(enumValue))
                {
                    fieldInfo.SetValue(obj, newValue);
                }
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                IList list = (IList)item;
                using (new EditorGUILayout.VerticalScope("framebox"))
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(fieldInfo.Name);
                    if (GUILayout.Button("+", GUILayout.Width(20)))
                    {
                        list.Add(CreateDefaultValue(type.GetGenericArguments()[0]));
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel++;
                    for (int i = 0; i < list.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(list[i].GetType().Name, GUILayout.Width(80));
                        object newItem = DrawItemField(list[i]);
                        if (!Equals(newItem, list[i]))
                        {
                            list[i] = newItem;
                        }
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;
                }

            }
            else
            {
                EditorGUILayout.LabelField(fieldInfo.Name);

                EditorGUI.indentLevel++;
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    DrawItem(item, field);
                }
                EditorGUI.indentLevel--;
            }
        }

        private static object DrawItemField(object value)
        {
            if (value == null)
            {
                EditorGUILayout.LabelField("Null");
                return null;
            }

            Type type = value.GetType();

            if (type == typeof(int))
            {
                return EditorGUILayout.IntField((int)value);
            }
            else if (type == typeof(float))
            {
                return EditorGUILayout.FloatField((float)value);
            }
            else if (type == typeof(string))
            {
                return EditorGUILayout.TextField((string)value);
            }
            else if (type == typeof(bool))
            {
                return EditorGUILayout.Toggle((bool)value);
            }
            else if (type.IsEnum)
            {
                return EditorGUILayout.EnumPopup((Enum)value);
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                DrawItem(value, null);
                return value;
            }
            else
            {
                DrawItem(value, null);
                return value;
            }
        }

        private static object CreateDefaultValue(Type type)
        {
            if (type == typeof(int))
            {
                return 0;
            }
            else if (type == typeof(float))
            {
                return 0f;
            }
            else if (type == typeof(string))
            {
                return string.Empty;
            }
            else if (type == typeof(bool))
            {
                return false;
            }
            else if (type.IsEnum)
            {
                return Enum.GetValues(type).GetValue(0);
            }
            else
            {
                return Activator.CreateInstance(type);
            }
        }
    }
}
