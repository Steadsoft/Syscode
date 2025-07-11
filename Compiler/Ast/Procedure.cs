﻿using Antlr4.Runtime;
using Syscode.Ast;

namespace Syscode
{
    public class Procedure : Declare, IContainer
    {
        public string Spelling;
        private bool isFunction;
        private string @as;
        private List<string> parameters = new List<string>();
        private List<AstNode> statements = new List<AstNode>();
        private List<Symbol> symbols = new List<Symbol>();
        private IContainer container;
        public Procedure(ParserRuleContext context, IContainer Container) : base(context)
        {
            this.Container = Container;
        }

        public List<AstNode> Statements { get => statements; set => statements = value; }
        public IContainer Container { get => container;  set => container = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
        public bool IsFunction { get => isFunction; set => isFunction = value; }
        public string As { get => @as; set => @as = value; }
        public List<string> Parameters { get => parameters; set => parameters = value; }

        public override string ToString()
        {
            return $"procedure {Spelling}";
        }
    }
}