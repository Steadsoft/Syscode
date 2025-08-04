﻿using static SyscodeParser;

namespace Syscode
{
    public class Always : Loop
    {
        public Always(LoopAlwaysContext context, AstBuilder builder) : base(context)
        {
            this.Statements = builder.GenerateStatements(context.Always._Statements);
            this.Label = context.Always.Name?.GetText().Replace("@", "");
        }

        public override string ToString()
        {
            return $"do @{Label} loop";
        }
    }
}
