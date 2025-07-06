namespace Zukwaz.CSharp.MvvmGenerator
{
    public abstract class Class
    {
        public string Name { set; get; } = string.Empty;
        public List<Property> Properties { get; } = new List<Property>();

        public string RtName
        {
            get
            {
                if (this is ClassDM)
                {
                    return $@"{Name}DM";
                }
                else if (this is ClassVM)
                {
                    return $@"{Name}VM";
                }

                return $@"{Name}";
            }
        }
        public string RtOppositeName
        {
            get
            {
                if (this is ClassDM)
                {
                    return $@"{Name}VM";
                }
                else if (this is ClassVM)
                {
                    return $@"{Name}DM";
                }

                return $@"{Name}";
            }
        }
    }
}
