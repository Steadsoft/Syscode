using static SyscodeParser;

namespace Syscode
{
    public class Leave : AstNode
    {
        private Reference reference;
        public Leave(LeaveContext context) : base(context)
        {
        }

        public Reference Reference { get => reference; internal set => reference = value; }

        public override string ToString()
        {
            return $"leave {Reference}";
        }
    }

}
