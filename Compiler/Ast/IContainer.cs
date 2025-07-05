namespace Syscode
{
    public interface IContainer
    {
        List<AstNode> Statements { get; internal set; }
        List<Symbol> Symbols { get; internal set; }
        IContainer Container { get; internal set; }
        bool HasNoStatements { get => Statements.Any() == false; }
        bool HasNoSymbols { get => Symbols.Any() == false; }

    }
}