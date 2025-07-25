﻿using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Goto : AstNode
    {
        private Reference reference;
        public Goto(GotoContext context) : base(context)
        {
        }

        public Reference Reference { get => reference; internal set => reference = value; }

        public override string ToString()
        {
            return $"goto {Reference}";
        }
    }

}
