using Lyxbux.Extensions;

namespace Zukwaz.CSharp.MvvmGenerator
{
    public sealed class ClassDM : Class
    {
        public string XmlName { set; get; } = string.Empty;

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
    }
}
