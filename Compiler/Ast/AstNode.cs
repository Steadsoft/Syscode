﻿using Antlr4.Runtime;

namespace Syscode
{
    public class AstNode
    {
        public readonly int StartLine;
        public readonly int StartColumn;
        public readonly int StopLine;
        public readonly int StopColumn;
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
            }
        }
    }
}