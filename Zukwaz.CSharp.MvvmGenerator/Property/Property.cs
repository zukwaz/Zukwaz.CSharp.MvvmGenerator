using Lyxbux.Extensions;

namespace Zukwaz.CSharp.MvvmGenerator
{
    public abstract class Property
    {
        public string Type { set; get; } = string.Empty;
        public string Name { set; get; } = string.Empty;
        public string Value { set; get; } = string.Empty;
        public bool Nullable { set; get; } = false;

        public string RtType
        {
            get
            {
                string type = $@"{Type}";

                if (this is ListPropertyDM)
                {
                    type = $@"DMList<{type}>";
                }
                else if (this is ListPropertyVM)
                {
                    type = $@"VMList<{type}>";
                }

                if (Nullable)
                {
                    type = $@"{type}?";
                }

                return $@"{type}";
            }
        }
        public string RtOppositeType
        {
            get
            {
                string type = $@"{Type}";

                if (this is ListPropertyDM)
                {
                    type = $@"VMList<{type}>";
                }
                else if (this is ListPropertyVM)
                {
                    type = $@"DMList<{type}>";
                }

                if (Nullable)
                {
                    type = $@"{type}?";
                }

                return $@"{type}";
            }
        }
        public string RtFieldName
        {
            get
            {
                return $@"_{Name}";
            }
        }
        public string RtValue
        {
            get
            {
                string value = $@"{Value}";

                if (value.IsNullOrEmptyOrWhiteSpace())
                {
                    if (Nullable)
                    {
                        value = $@"null";
                    }
                    else
                    {
                        value = $@"{GetDefaultValue()}";
                    }
                }

                return $@"{value}";
            }
        }

        private string GetDefaultValue()
        {
            if (this is NativePropertyDM || this is NativePropertyVM)
            {
                if (Type == "bool" || Type == "Boolean") { return $@"false"; }
                else if (Type == "byte" || Type == "Byte") { return $@"0"; }
                else if (Type == "sbyte" || Type == "SByte") { return $@"0"; }
                else if (Type == "char" || Type == "Char") { return $@"'\0'"; }
                else if (Type == "decimal" || Type == "Decimal") { return $@"0"; }
                else if (Type == "double" || Type == "Double") { return $@"0"; }
                else if (Type == "float" || Type == "Single") { return $@"0"; }
                else if (Type == "int" || Type == "Int32") { return $@"0"; }
                else if (Type == "uint" || Type == "UInt32") { return $@"0"; }
                else if (Type == "nint") { return $@"nint.Zero"; }
                else if (Type == "IntPtr") { return $@"IntPtr.Zero"; }
                else if (Type == "nuint") { return $@"nuint.Zero"; }
                else if (Type == "UIntPtr") { return $@"UIntPtr.Zero"; }
                else if (Type == "long" || Type == "Int64") { return $@"0"; }
                else if (Type == "ulong" || Type == "UInt64") { return $@"0"; }
                else if (Type == "short" || Type == "Int16") { return $@"0"; }
                else if (Type == "ushort" || Type == "UInt16") { return $@"0"; }
                else if (Type == "object") { return $@"new object()"; }
                else if (Type == "Object") { return $@"new Object()"; }
                else if (Type == "string") { return $@"string.Empty"; }
                else if (Type == "String") { return $@"String.Empty"; }
            }
            else if (this is EnumPropertyDM || this is EnumPropertyVM)
            {
                return $@"new {Type}()";
            }
            else if (this is ReferencePropertyDM || this is ReferencePropertyVM)
            {
                return $@"new {Type}()";
            }
            else if (this is ListPropertyDM)
            {
                return $@"new DMList<{Type}>()";
            }
            else if (this is ListPropertyVM)
            {
                return $@"new VMList<{Type}>()";
            }

            return string.Empty;
        }
    }
}
