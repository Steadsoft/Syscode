using Antlr4.Runtime;
using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class For : Loop, IReplaceContainer
    {
        public Reference Ref { get; private set; }
        public Expression From { get; private set; }
        public Expression To { get; private set; }
        public Expression? By { get; private set; }
        public Expression? UntilCondition { get; private set; }
        public Expression? WhileCondition { get; private set; }

        public For(LoopForContext context, SyscodeAstBuilder builder) : base(context)
        {
            // Compulsory
            Ref = builder.CreateReference(context.For.For);
            From = builder.CreateExpression(context.For.From);
            To = builder.CreateExpression(context.For.To);
            Statements = builder.GenerateStatements(context.For._Statements);

            // Optional
            By = context.For.By?.SafeCreate(builder.CreateExpression);
            WhileCondition = context.For.While?.Exp.SafeCreate(builder.CreateExpression);
            UntilCondition = context.For.Until?.Exp.SafeCreate(builder.CreateExpression);
            this.Label = context.For.Name?.GetText().Replace("@", "");
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            if (By == null) 
                builder.Append($"for {Ref} = {From} to {To}");
            else
                builder.Append($"for {Ref} = {From} to {To} by {By}");

            if (WhileCondition != null)
                builder.Append($" while {WhileCondition}");
            
            if (UntilCondition != null)
                builder.Append($" until {UntilCondition}");

            return builder.ToString();
        }

        public override void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            From.ApplyPreprocessorReplace(tokens, replace);
            To.ApplyPreprocessorReplace(tokens, replace);
            By?.ApplyPreprocessorReplace(tokens, replace);
            UntilCondition?.ApplyPreprocessorReplace(tokens, replace);
            WhileCondition?.ApplyPreprocessorReplace(tokens, replace);
            base.ApplyPreprocessorReplace(tokens, replace);
        }
    }
}
