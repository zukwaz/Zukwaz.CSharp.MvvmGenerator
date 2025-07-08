using Lyxbux.Extensions;
using System.Text;

namespace Zukwaz.CSharp.MvvmGenerator
{
    public sealed class VMGenerator : Generator
    {
        private VMGenerator() { }

        public static string Generate(Namespace namespaceMvvm)
        {
            return GenerateNamespace(namespaceMvvm).ToString();
        }
        private static StringBuilder GenerateNamespace(Namespace namespaceMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($@"namespace {namespaceMvvm.Name}");
            builder.AppendLine($@"{{");

            for (int i = 0; i < namespaceMvvm.Classes.Count; i++)
            {
                builder.Append(GenerateClass(namespaceMvvm.Classes[i]));

                if (i < namespaceMvvm.Classes.Count - 1)
                {
                    builder.AppendLine();
                }
            }
            if (namespaceMvvm.Classes.Count == 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine($@"}}");

            return builder;
        }
        private static StringBuilder GenerateClass(Class classMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(1) + $@"public sealed class {classMvvm.RtName} : BaseVM, IViewModel<{classMvvm.RtName}>");
            builder.AppendLine(Tab(1) + $@"{{");

            for (int i = 0; i < classMvvm.Properties.Count; i++)
            {
                builder.Append(GenerateField(classMvvm.Properties[i]));
            }
            if (classMvvm.Properties.Count > 0)
            {
                builder.AppendLine();
            }
            for (int i = 0; i < classMvvm.Properties.Count; i++)
            {
                builder.Append(GenerateProperty(classMvvm.Properties[i]));
            }
            if (classMvvm.Properties.Count > 0)
            {
                builder.AppendLine();
            }

            builder.Append(GenerateConstructorT1(classMvvm));
            if (classMvvm is ClassVM classVM && classVM.HasDM)
            {
                builder.Append(GenerateConstructorT2(classVM));
                builder.Append(GenerateGetDMMethod(classVM));
            }
            builder.AppendLine();
            builder.Append(GenerateCloneMethod(classMvvm));
            builder.Append(GenerateCopyMethod(classMvvm));
            builder.Append(GenerateEqualsMethod(classMvvm));
            builder.Append(GenerateResetMethod(classMvvm));

            builder.AppendLine(Tab(1) + $@"}}");

            return builder;
        }
        private static StringBuilder GenerateField(Property propertyMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(2) + $@"private {propertyMvvm.RtType} {propertyMvvm.RtFieldName};");

            return builder;
        }
        private static StringBuilder GenerateProperty(Property propertyMvvm)
        {
            StringBuilder builder = new StringBuilder();

            string modifier = string.Empty;
            if (propertyMvvm is ReferencePropertyVM || propertyMvvm is ListPropertyVM)
            {
                modifier = $@"private ";
            }

            builder.AppendLine(Tab(2) + $@"public {propertyMvvm.RtType} {propertyMvvm.Name}");
            builder.AppendLine(Tab(2) + $@"{{");
            builder.AppendLine(Tab(3) + $@"{modifier}set");
            builder.AppendLine(Tab(3) + $@"{{");
            builder.AppendLine(Tab(4) + $@"if ({propertyMvvm.RtFieldName} != value)");
            builder.AppendLine(Tab(4) + $@"{{");
            builder.AppendLine(Tab(5) + $@"{propertyMvvm.RtFieldName} = value;");
            builder.AppendLine(Tab(5) + $@"OnPropertyChanged({DQ}{propertyMvvm.Name}{DQ});");
            builder.AppendLine(Tab(4) + $@"}}");
            builder.AppendLine(Tab(3) + $@"}}");
            builder.AppendLine(Tab(3) + $@"get");
            builder.AppendLine(Tab(3) + $@"{{");
            builder.AppendLine(Tab(4) + $@"return {propertyMvvm.RtFieldName};");
            builder.AppendLine(Tab(3) + $@"}}");
            builder.AppendLine(Tab(2) + $@"}}");

            return builder;
        }
        private static StringBuilder GenerateConstructorT1(Class classMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(2) + $@"public {classMvvm.RtName}()");
            builder.AppendLine(Tab(2) + $@"{{");

            foreach (Property propertyMvvm in classMvvm.Properties)
            {
                builder.AppendLine(Tab(3) + $@"{propertyMvvm.RtFieldName} = {propertyMvvm.RtValue};");
            }
            if (classMvvm.Properties.Count > 0)
            {
                builder.AppendLine();
            }
            foreach (Property propertyMvvm in classMvvm.Properties)
            {
                builder.AppendLine(Tab(3) + $@"OnPropertyChanged({DQ}{propertyMvvm.Name}{DQ});");
            }
            if (classMvvm.Properties.Count == 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine(Tab(2) + $@"}}");

            return builder;
        }
        private static StringBuilder GenerateConstructorT2(Class classMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(2) + $@"public {classMvvm.RtName}({classMvvm.RtOppositeName} dm)");
            builder.AppendLine(Tab(2) + $@"{{");

            foreach (Property propertyMvvm in classMvvm.Properties)
            {
                if (propertyMvvm is PropertyVM propertyVM)
                {
                    if (propertyVM is NativePropertyVM || propertyVM is EnumPropertyVM)
                    {
                        if (propertyVM.ConvertType.IsNullOrEmptyOrWhiteSpace())
                        {
                            builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = {propertyVM.RtValue};");
                        }
                        else
                        {
                            if (propertyVM.ConvertType == propertyVM.Type)
                            {
                                builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = dm.{propertyVM.Name};");
                            }
                            else
                            {
                                builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = {propertyVM.RtValue};");
                            }
                        }
                    }
                    else if (propertyVM is ReferencePropertyVM)
                    {
                        if (propertyVM.ConvertType.IsNullOrEmptyOrWhiteSpace())
                        {
                            builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = {propertyVM.RtValue};");
                        }
                        else
                        {
                            if (propertyVM.ConvertType == propertyVM.Type)
                            {
                                builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = dm.{propertyVM.Name};");
                            }
                            else
                            {
                                builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = new {propertyVM.Type}(dm.{propertyVM.Name});");
                            }
                        }
                    }
                    else if (propertyVM is ListPropertyVM)
                    {
                        if (propertyVM.ConvertType.IsNullOrEmptyOrWhiteSpace())
                        {
                            builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = {propertyVM.RtValue};");
                        }
                        else
                        {
                            if (propertyVM.ConvertType == propertyVM.Type)
                            {
                                builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = new VMList<{propertyVM.Type}>(dm.{propertyVM.Name});");
                            }
                            else
                            {
                                builder.AppendLine(Tab(3) + $@"{propertyVM.RtFieldName} = new VMList<{propertyVM.Type}>(dm.{propertyVM.Name}.Select(dmItem => new {propertyVM.Type}(dmItem)));");
                            }
                        }
                    }
                }
            }
            if (classMvvm.Properties.Count > 0)
            {
                builder.AppendLine();
            }
            foreach (Property propertyMvvm in classMvvm.Properties)
            {
                builder.AppendLine(Tab(3) + $@"OnPropertyChanged({DQ}{propertyMvvm.Name}{DQ});");
            }
            if (classMvvm.Properties.Count == 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine(Tab(2) + $@"}}");

            return builder;
        }
        private static StringBuilder GenerateGetDMMethod(Class classMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(2) + $@"public {classMvvm.RtOppositeName} GetDM()");
            builder.AppendLine(Tab(2) + $@"{{");

            builder.AppendLine(Tab(3) + $@"return new {classMvvm.RtOppositeName}()");
            builder.AppendLine(Tab(3) + $@"{{");

            for (int i = 0; i < classMvvm.Properties.Count; i++)
            {
                string comma = string.Empty;
                if (i < classMvvm.Properties.Count - 1)
                {
                    comma = ",";
                }

                Property propertyMvvm = classMvvm.Properties[i];
                if (propertyMvvm is PropertyVM propertyVM && !propertyVM.ConvertType.IsNullOrEmptyOrWhiteSpace())
                {
                    if (propertyVM is NativePropertyVM || propertyVM is EnumPropertyVM)
                    {
                        if (propertyVM.ConvertType == propertyVM.Type)
                        {
                            builder.AppendLine(Tab(4) + $@"{propertyVM.Name} = {propertyVM.Name}{comma}");
                        }
                        else
                        {
                            builder.AppendLine(Tab(4) + $@"{propertyVM.Name} = {propertyVM.Name}.GetDM(){comma}");
                        }
                    }
                    else if (propertyVM is ReferencePropertyVM)
                    {
                        if (propertyVM.ConvertType == propertyVM.Type)
                        {
                            builder.AppendLine(Tab(4) + $@"{propertyVM.Name} = {propertyVM.Name}{comma}");
                        }
                        else
                        {
                            builder.AppendLine(Tab(4) + $@"{propertyVM.Name} = {propertyVM.Name}.GetDM(){comma}");
                        }
                    }
                    else if (propertyVM is ListPropertyVM)
                    {
                        if (propertyVM.ConvertType == propertyVM.Type)
                        {
                            builder.AppendLine(Tab(4) + $@"{propertyVM.Name} = new DMList<{propertyVM.ConvertType}>({propertyVM.Name}){comma}");
                        }
                        else
                        {
                            builder.AppendLine(Tab(4) + $@"{propertyVM.Name} = new DMList<{propertyVM.ConvertType}>({propertyVM.Name}.Select(vmItem => vmItem.GetDM())){comma}");
                        }
                    }
                }
            }
            if (classMvvm.Properties.Count == 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine(Tab(3) + $@"}};");

            builder.AppendLine(Tab(2) + $@"}}");

            return builder;
        }
        private static StringBuilder GenerateCloneMethod(Class classMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(2) + $@"public {classMvvm.RtName} Clone()");
            builder.AppendLine(Tab(2) + $@"{{");
            builder.AppendLine(Tab(3) + $@"return new {classMvvm.RtName}()");
            builder.AppendLine(Tab(3) + $@"{{");

            for (int i = 0; i < classMvvm.Properties.Count; i++)
            {
                string comma = string.Empty;
                if (i < classMvvm.Properties.Count - 1)
                {
                    comma = ",";
                }

                Property propertyMvvm = classMvvm.Properties[i];
                if (propertyMvvm is NativePropertyVM || propertyMvvm is EnumPropertyVM)
                {
                    builder.AppendLine(Tab(4) + $@"{propertyMvvm.Name} = {propertyMvvm.Name}{comma}");
                }
                else if (propertyMvvm is ReferencePropertyVM || propertyMvvm is ListPropertyVM)
                {
                    builder.AppendLine(Tab(4) + $@"{propertyMvvm.Name} = {propertyMvvm.Name}.Clone(){comma}");
                }
            }
            if (classMvvm.Properties.Count == 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine(Tab(3) + $@"}};");
            builder.AppendLine(Tab(2) + $@"}}");

            return builder;
        }
        private static StringBuilder GenerateCopyMethod(Class classMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(2) + $@"public void Copy({classMvvm.RtName} model)");
            builder.AppendLine(Tab(2) + $@"{{");

            foreach (Property propertyMvvm in classMvvm.Properties)
            {
                if (propertyMvvm is NativePropertyVM || propertyMvvm is EnumPropertyVM)
                {
                    builder.AppendLine(Tab(3) + $@"{propertyMvvm.Name} = model.{propertyMvvm.Name};");
                }
                else if (propertyMvvm is ReferencePropertyVM || propertyMvvm is ListPropertyVM)
                {
                    builder.AppendLine(Tab(3) + $@"{propertyMvvm.Name}.Copy(model.{propertyMvvm.Name});");
                }
            }
            if (classMvvm.Properties.Count == 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine(Tab(2) + $@"}}");

            return builder;
        }
        private static StringBuilder GenerateEqualsMethod(Class classMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(2) + $@"public bool Equals({classMvvm.RtName} model)");
            builder.AppendLine(Tab(2) + $@"{{");
            builder.AppendLine(Tab(3) + $@"if (model == null) {{ return false; }}");

            foreach (Property propertyMvvm in classMvvm.Properties)
            {
                if (propertyMvvm is NativePropertyVM || propertyMvvm is EnumPropertyVM)
                {
                    builder.AppendLine(Tab(3) + $@"if ({propertyMvvm.Name} != model.{propertyMvvm.Name}) {{ return false; }}");
                }
                else if (propertyMvvm is ReferencePropertyVM || propertyMvvm is ListPropertyVM)
                {
                    builder.AppendLine(Tab(3) + $@"if(!{propertyMvvm.Name}.Equals(model.{propertyMvvm.Name})) {{ return false; }}");
                }
            }

            builder.AppendLine();
            builder.AppendLine(Tab(3) + $@"return true;");
            builder.AppendLine(Tab(2) + $@"}}");

            return builder;
        }
        private static StringBuilder GenerateResetMethod(Class classMvvm)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Tab(2) + $@"public void Reset()");
            builder.AppendLine(Tab(2) + $@"{{");

            foreach (Property propertyMvvm in classMvvm.Properties)
            {
                if (propertyMvvm is NativePropertyVM || propertyMvvm is EnumPropertyVM)
                {
                    builder.AppendLine(Tab(3) + $@"{propertyMvvm.Name} = {propertyMvvm.RtValue};");
                }
                else if (propertyMvvm is ReferencePropertyVM || propertyMvvm is ListPropertyVM)
                {
                    builder.AppendLine(Tab(3) + $@"{propertyMvvm.Name}.Reset();");
                }
            }
            if (classMvvm.Properties.Count == 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine(Tab(2) + $@"}}");

            return builder;
        }
    }
}
