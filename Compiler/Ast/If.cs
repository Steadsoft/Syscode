using Syscode.Ast;
using static SyscodeParser;

namespace Syscode
{
    public class If : AstNode, ISymbol
    {
        private readonly Expression condition;
        private List<AstNode> thenStatements = new();
        private List<AstNode> elseStatements = new();
        private List<Elif> elifStatements = new();
        private readonly string label = string.Empty;
        public string Label => label;
        public Expression Condition { get => condition;}
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }
        public List<AstNode> ElseStatements { get => elseStatements; set => elseStatements = value; }
        public List<Elif> ElifStatements { get => elifStatements; set => elifStatements = value; }

        public string Spelling => label;

        public DataType Type => DataType.LABEL;

        public StorageClass StorageClass => StorageClass.Static;

        public StorageScope StorageScope => StorageScope.Internal;

        public Alignment Alignment => throw new NotImplementedException();

        public int Length => throw new NotImplementedException();

        public int Bytes => throw new NotImplementedException();

        public If(IfContext context, AstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.ExprThen.Exp);
            thenStatements = builder.GenerateStatements(context.ExprThen.Then._Statements);

            if (context.Else != null)
            {
                elseStatements = builder.GenerateStatements(context.Else.Then._Statements);
            }

            if (context.Elif != null)  // at least one 'elif' is present
            {
                elifStatements = context.Elif._ExprThen.Select(builder.CreateElif).ToList();
            }

            if (context.Name != null)
                label = context.Name.Spelling.GetText();
        }
        public override string ToString()
        {
            return $"{nameof(If)} {label}: ";
        }
    }
}