namespace Syscode
{
    public interface IContainer
    {
        List<AstNode> Statements { get; }
        List<Symbol> Symbols { get; internal set; }
        IContainer Container { get;  }
        bool HasNoStatements { get => Statements.Count != 0 == false; }
        bool HasNoSymbols { get => Symbols.Count != 0 == false; }

    }

    public interface ISpelling
    {
        string Spelling { get; }
    }
}