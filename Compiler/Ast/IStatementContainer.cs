namespace Syscode
{
    public interface IStatementContainer
    {
        List<AstNode> Statements { get; internal set; }
    }
}