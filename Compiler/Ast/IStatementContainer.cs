namespace Syscode
{
    public interface IStatementContainer
    {
        List<AstNode> Statements { get; internal set; }
        List<Symbol> Symbols { get; internal set; }

        bool Empty { get => Statements.Any() == false; }
    }
}