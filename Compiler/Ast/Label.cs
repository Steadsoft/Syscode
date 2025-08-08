using Syscode.Ast;
using static SyscodeParser;

namespace Syscode
{
    public class Label : AstNode, ISymbol
    {
        public string spelling;
        public string? subscript;
        public Label(AlabelContext context) : base(context)
        {
            spelling = context.Name.Spelling.GetText();
            subscript = context.Subscript?.Literal.GetText();
        }
        public string? Subscript => subscript;

        #region ISymbol
        string ISymbol.Spelling => spelling;
        DataType ISymbol.Type => DataType.LABEL;
        StorageClass ISymbol.StorageClass => StorageClass.Static;
        StorageScope ISymbol.StorageScope => StorageScope.Unspecified;
        Alignment ISymbol.Alignment => default;
        int ISymbol.Length => 0;
        int ISymbol.Bytes => 0;
        int? ISymbol.LabelSubscript => null;
        #endregion

        public override string ToString()
        {
            return "@" + spelling;
        }
    }
}
