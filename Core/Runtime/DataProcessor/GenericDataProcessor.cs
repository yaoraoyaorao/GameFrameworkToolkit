using System;
using System.IO;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public abstract class GenericDataProcessor<T> : DataProcessor
    {
        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        public override bool IsId
        {
            get
            {
                return false;
            }
        }

        public override bool IsComment
        {
            get
            {
                return false;
            }
        }

        public abstract T Parse(string value);
    }

    public sealed class IdProcessor : DataProcessor
    {
        public override System.Type Type
        {
            get
            {
                return typeof(int);
            }
        }

        public override bool IsId
        {
            get
            {
                return true;
            }
        }

        public override bool IsComment
        {
            get
            {
                return false;
            }
        }

        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "int";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "id"
            };
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(uint.Parse(value));
        }
    }

    public sealed class ByteProcessor : GenericDataProcessor<byte>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "byte";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "byte",
                "system.byte"
            };
        }

        public override byte Parse(string value)
        {
            return byte.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class CharProcessor : GenericDataProcessor<char>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "char";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "char",
                "system.char"
            };
        }

        public override char Parse(string value)
        {
            return char.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class DecimalProcessor : GenericDataProcessor<decimal>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "decimal";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "decimal",
                "system.decimal"
            };
        }

        public override decimal Parse(string value)
        {
            return decimal.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class Color32Processor : GenericDataProcessor<Color32>
    {
        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "Color32";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "color32",
                "unityengine.color32"
            };
        }

        public override Color32 Parse(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Color32(byte.Parse(splitedValue[0]), byte.Parse(splitedValue[1]), byte.Parse(splitedValue[2]), byte.Parse(splitedValue[3]));
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            Color32 color32 = Parse(value);
            binaryWriter.Write(color32.r);
            binaryWriter.Write(color32.g);
            binaryWriter.Write(color32.b);
            binaryWriter.Write(color32.a);
        }
    }

    public sealed class DoubleProcessor : GenericDataProcessor<double>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "double";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "double",
                "system.double"
            };
        }

        public override double Parse(string value)
        {
            return double.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class ColorProcessor : GenericDataProcessor<Color>
    {
        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "Color";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "color",
                    "unityengine.color"
            };
        }

        public override Color Parse(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Color(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            Color color = Parse(value);
            binaryWriter.Write(color.r);
            binaryWriter.Write(color.g);
            binaryWriter.Write(color.b);
            binaryWriter.Write(color.a);
        }
    }

    public sealed class DateTimeProcessor : GenericDataProcessor<DateTime>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "DateTime";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "datetime",
                    "system.datetime"
            };
        }

        public override DateTime Parse(string value)
        {
            return DateTime.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value).Ticks);
        }
    }

    public sealed class BooleanProcessor : GenericDataProcessor<bool>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "bool";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "bool",
                "boolean",
                "system.boolean"
            };
        }

        public override bool Parse(string value)
        {
            return bool.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public class CommentProcessor : DataProcessor
    {
        public override System.Type Type
        {
            get
            {
                return null;
            }
        }

        public override bool IsId
        {
            get
            {
                return false;
            }
        }

        public override bool IsComment
        {
            get
            {
                return true;
            }
        }

        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return null;
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    string.Empty,
                    "#",
                    "comment"
            };
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {

        }
    }

    public sealed class Int16Processor : GenericDataProcessor<short>
    {
        public override bool IsSystem
        {
            get { return true; }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "short";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "short",
                "int16",
                "system.int16"
            };
        }

        public override short Parse(string value)
        {
            return short.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class Int32Processor: GenericDataProcessor<int>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "int";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "int",
                    "int32",
                    "system.int32"
            };
        }

        public override int Parse(string value)
        {
            return int.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class Int64Processor : GenericDataProcessor<long>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "long";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "long",
                    "int64",
                    "system.int64"
            };
        }

        public override long Parse(string value)
        {
            return long.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class QuaternionProcessor : GenericDataProcessor<Quaternion>
    {
        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "Quaternion";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "quaternion",
                    "unityengine.quaternion"
            };
        }

        public override Quaternion Parse(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Quaternion(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            Quaternion quaternion = Parse(value);
            binaryWriter.Write(quaternion.x);
            binaryWriter.Write(quaternion.y);
            binaryWriter.Write(quaternion.z);
            binaryWriter.Write(quaternion.w);
        }
    }

    public sealed class RectProcessor : GenericDataProcessor<Rect>
    {
        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "Rect";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "rect",
                    "unityengine.rect"
            };
        }

        public override Rect Parse(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Rect(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            Rect rect = Parse(value);
            binaryWriter.Write(rect.x);
            binaryWriter.Write(rect.y);
            binaryWriter.Write(rect.width);
            binaryWriter.Write(rect.height);
        }
    }

    public sealed class SByteProcessor : GenericDataProcessor<sbyte>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "sbyte";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "sbyte",
                    "system.sbyte"
            };
        }

        public override sbyte Parse(string value)
        {
            return sbyte.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class SingleProcessor : GenericDataProcessor<float>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "float";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "float",
                    "single",
                    "system.single"
            };
        }

        public override float Parse(string value)
        {
            return float.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class StringProcessor : GenericDataProcessor<string>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "string";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "string",
                    "system.string"
            };
        }

        public override string Parse(string value)
        {
            return value;
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class UInt16Processor : GenericDataProcessor<ushort>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "ushort";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "ushort",
                "uint16",
                "system.uint16"
            };
        }

        public override ushort Parse(string value)
        {
            return ushort.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class UInt32Processor : GenericDataProcessor<uint>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "uint";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "uint",
                "uint32",
                "system.uint32"
            };
        }

        public override uint Parse(string value)
        {
            return uint.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class UInt64Processor : GenericDataProcessor<ulong>
    {
        public override bool IsSystem
        {
            get
            {
                return true;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "ulong";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "ulong",
                "uint64",
                "system.uint64"
            };
        }

        public override ulong Parse(string value)
        {
            return ulong.Parse(value);
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }

    public sealed class Vector2Processor : GenericDataProcessor<Vector2>
    {
        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "Vector2";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "vector2",
                    "unityengine.vector2"
            };
        }

        public override Vector2 Parse(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector2(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]));
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            Vector2 vector2 = Parse(value);
            binaryWriter.Write(vector2.x);
            binaryWriter.Write(vector2.y);
        }
    }

    public sealed class Vector3Processor : GenericDataProcessor<Vector3>
    {
        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "Vector3";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                    "vector3",
                    "unityengine.vector3"
            };
        }

        public override Vector3 Parse(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            Vector3 vector3 = Parse(value);
            binaryWriter.Write(vector3.x);
            binaryWriter.Write(vector3.y);
            binaryWriter.Write(vector3.z);
        }
    }

    public sealed class Vector4Processor : GenericDataProcessor<Vector4>
    {
        public override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public override string LanguageKeyword
        {
            get
            {
                return "Vector4";
            }
        }

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "vector4",
                "unityengine.vector4"
            };
        }

        public override Vector4 Parse(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector4(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public override void WriteToStream(BinaryWriter binaryWriter, string value)
        {
            Vector4 vector4 = Parse(value);
            binaryWriter.Write(vector4.x);
            binaryWriter.Write(vector4.y);
            binaryWriter.Write(vector4.z);
            binaryWriter.Write(vector4.w);
        }
    }
}
