using System.Text;

namespace Zukwaz.CSharp.MvvmGenerator
{
    public sealed class DMGenerator : Generator
    {
        private DMGenerator() { }

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

            if (classMvvm is ClassDM classDM)
            {
                builder.AppendLine(Tab(1) + $@"[XmlRoot({DQ}{classDM.RtXmlName}{DQ})]");
                builder.AppendLine(Tab(1) + $@"public sealed class {classDM.RtName} : BaseDM, IDataModel<{classDM.RtName}>");
                builder.AppendLine(Tab(1) + $@"{{");

                for (int i = 0; i < classDM.Properties.Count; i++)
                {
                    builder.Append(GenerateProperty(classDM.Properties[i]));

                    if (i < classDM.Properties.Count - 1)
                    {
                        builder.AppendLine();
                    }
                }
                if (classDM.Properties.Count == 0)
                {
                    builder.AppendLine();
                }

                builder.AppendLine(Tab(1) + $@"}}");
            }

            return builder;
        }
        private static StringBuilder GenerateProperty(Property propertyMvvm)
        {
            StringBuilder builder = new StringBuilder();

            string xmlAttribute = $@"XmlAttribute";
            if (propertyMvvm is ReferencePropertyDM || propertyMvvm is ListPropertyDM)
            {
                xmlAttribute = $@"XmlElement";
            }

            if (propertyMvvm is PropertyDM propertyDM)
            {
                builder.AppendLine(Tab(2) + $@"[{xmlAttribute}({DQ}{propertyDM.RtXmlName}{DQ})]");
                builder.AppendLine(Tab(2) + $@"[JsonPropertyName({DQ}{propertyDM.RtJsonName}{DQ})]");
                builder.AppendLine(Tab(2) + $@"public {propertyDM.RtType} {propertyDM.Name} {{ set; get; }} = {propertyDM.RtValue};");
            }

            return builder;
        }
    }
}
