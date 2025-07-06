using System.Text;

namespace Zukwaz.CSharp.MvvmGenerator
{
    public abstract class Generator
    {
        protected const char DQ = '\"'; // Double Quotes

        protected static string Tab(int total)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < total; i++)
            {
                builder.Append($@"    ");
            }

            return builder.ToString();
        }
    }
}
