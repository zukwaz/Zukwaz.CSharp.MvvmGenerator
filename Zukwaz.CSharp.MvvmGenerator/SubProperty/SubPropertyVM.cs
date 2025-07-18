namespace Zukwaz.CSharp.MvvmGenerator
{
    public abstract class SubPropertyVM
    {
        public string Type { set; get; } = string.Empty;
        public string Name { set; get; } = string.Empty;
        public bool Nullable { set; get; } = false;

        public string RtType
        {
            get
            {
                string type = $@"{Type}";

                if (this is SubListPropertyVM)
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
    }
}
