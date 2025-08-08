using Syscode.Ast;
using static SyscodeParser;

namespace Syscode
{
    public abstract class Loop : AstNode, IContainer, ISymbol
    {
        private List<AstNode> statements;
        private List<Symbol> symbols;
        private string? label;
        public Loop(LoopContext context) : base(context)
        {
        }
        public string? Label { get => label; protected set => label = value; }
        public List<AstNode> Statements { get => statements; protected set => statements = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
        public IContainer Container { get => null; set { } }

        string ISymbol.Spelling => label;

        DataType ISymbol.Type => DataType.LABEL;

        StorageClass ISymbol.StorageClass => StorageClass.Static;

        StorageScope ISymbol.StorageScope => StorageScope.Internal;

        Alignment ISymbol.Alignment => throw new NotImplementedException();

        int ISymbol.Length => throw new NotImplementedException();

        int ISymbol.Bytes => throw new NotImplementedException();

        public override string ToString()
        {
            return "do loop";
        }
    }
}
