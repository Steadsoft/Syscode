using Antlr4.Runtime;

namespace Syscode
{
    /// <summary>
    /// Defines a contract for objects that support applying a preprocessor replacement operation to identifiers
    /// that appear within references and expressions. 
    /// </summary>
    public interface IReplaceContainer
    {
        void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace);
    }
}