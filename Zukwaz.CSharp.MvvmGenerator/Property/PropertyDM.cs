using Lyxbux.Extensions;

namespace Zukwaz.CSharp.MvvmGenerator
{
    public abstract class PropertyDM : Property
    {
        public string XmlName { set; get; } = string.Empty;
        public string JsonName { set; get; } = string.Empty;

        public string RtXmlName
        {
            get
            {
                if (XmlName.IsNullOrEmptyOrWhiteSpace())
                {
                    return $@"{Name}";
                }

                return $@"{XmlName}";
            }
        }
        public string RtJsonName
        {
            get
            {
                if (JsonName.IsNullOrEmptyOrWhiteSpace())
                {
                    return $@"{Name}";
                }

                return $@"{JsonName}";
            }
        }
    }
}
