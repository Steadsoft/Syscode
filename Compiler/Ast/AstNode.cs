using Antlr4.Runtime;

namespace Syscode
{
    public class AstNode
    {
        public readonly int StartLine;
        public readonly int StartColumn;
        public readonly int StopLine;
        public readonly int StopColumn;
        public  int StartToken;
        public  int EndToken;
        public bool preprocessor;
        /// <summary>
        /// Indicates what the last reported diagnostic was when processing a node. 
        /// </summary>
        public int ReportedError = 0;

        public AstNode(ParserRuleContext context)
        {
            if (context != null)
            {
                StartLine = context.Start.Line;
                StartColumn = context.Start.Column;
                StopLine = context.Stop.Line;
                StopColumn = context.Stop.Column;
                StartToken = context.Start.TokenIndex;
                EndToken = context.Stop.TokenIndex;
            }
        }

        public AstNode(IToken context)
        {
            if (context != null)
            {
                StartLine = context.Line;
                StartColumn = context.Column;
                StopLine = context.Line;
                StopColumn = context.Column;
            }
        }

        public AstNode()
        {

        }

    }
}