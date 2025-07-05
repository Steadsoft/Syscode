using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class Reference : AstNode
    {
        public Reference reference = null; // only populated if this ref is the left of ref -> ref
        public List<Arguments> ArgumentsList= new();
        private BasicReference basic = null;

        public Reference(ReferenceContext context) : base(context)
        {
        }
        public bool IsBasic
        {
            get { return reference == null; }
        }

        public bool IsntBasic { get => !IsBasic; }

        public BasicReference Basic { get => basic; internal set => basic = value; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (reference != null)
            {
                builder.Append(reference.ToString());
                builder.Append(" -> ");
            }

           builder.Append(Basic.ToString());

           foreach (var arg in ArgumentsList)
            {
                builder.Append(arg.ToString());
            }

           return builder.ToString();
        }
    }
}