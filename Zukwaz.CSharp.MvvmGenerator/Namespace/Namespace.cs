namespace Zukwaz.CSharp.MvvmGenerator
{
    public sealed class Namespace
    {
        public string Name { set; get; } = string.Empty;
        public List<Class> Classes { get; } = new List<Class>();
    }
}
