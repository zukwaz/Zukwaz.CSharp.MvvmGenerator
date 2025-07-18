namespace Zukwaz.CSharp.MvvmGenerator
{
    public abstract class PropertyVM : Property
    {
        public string ConvertType { set; get; } = string.Empty;
        public List<SubPropertyVM> SubProperties { get; } = new List<SubPropertyVM>();
    }
}
