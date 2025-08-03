namespace Syscode
{
    public interface IContainer
    {
        List<AstNode> Statements { get; }
        List<Symbol> Symbols { get; internal set; }
        IContainer Container { get;  }
        bool HasNoStatements { get => Statements.Any() == false; }
        bool HasNoSymbols { get => Symbols.Any() == false; }

    }

    public interface ISpelling
    {
        string Spelling { get; }
    }
}