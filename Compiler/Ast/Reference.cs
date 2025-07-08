using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class Reference : AstNode
    {
        private Reference innerReference = null; // only populated if this ref is the left of ref -> ref
        private List<Arguments> argumentsList = new();
        private BasicReference basic = null;
        private bool resolved = false;

        public Reference(ReferenceContext context) : base(context)
        {
        }
        public bool IsBasic
        {
            get { return InnerReference == null; }
        }

        public bool IsResolved { get => resolved; internal set => resolved = value; }
        public bool IsntResolved { get => !resolved; }

        public bool IsntBasic { get => !IsBasic; }

        public BasicReference Basic { get => basic; internal set => basic = value; }
        public Reference InnerReference { get => innerReference; set => innerReference = value; }
        public List<Arguments> ArgumentsList { get => argumentsList; set => argumentsList = value; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (InnerReference != null)
            {
                builder.Append(InnerReference.ToString());
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