using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static SyscodeParser;

namespace Syscode
{
    public static class SyscodeExtensions
    {
        private static HashSet<System.Type> excludedTypes;
        static SyscodeExtensions()
        {
            excludedTypes = new HashSet<System.Type>
            {
                typeof(EmptyLinesContext),
                typeof(StatementSeparatorContext),
                typeof(MemberSeparatorContext) ,
                typeof(PreambleContext),
                typeof(EndOfFileContext)
            };
        }


        // TODO: There's confusion here as to whether we want an exact type match or a compatible type match this must be made clearer.

        public static bool HasToken(this ParserRuleContext context, int tokenType)
        {
            if (context.children.Where(c => c is TerminalNodeImpl).Where(t => ((TerminalNodeImpl)(t)).Symbol.Type == tokenType).Count() == 1)
            {
                return true;
            }

            return false;
        }
        public static T GetTerminal<T>(this ParserRuleContext context) where T : TerminalNodeImpl
        {
            return (T)context.children.Where(child => child is T).Single();
        }

        /// <summary>
        /// Gets a node of type T or any class derived from T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T GetDerivedNode<T>(this ParserRuleContext context) where T : ParserRuleContext
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (context.children == null)
                throw new InvalidOperationException("Expected child node is not present.");

            var matches = context.GetChildren().Where(child => child is T).ToList();

            if (matches.Any() == false)
                throw new InvalidOperationException("Expected child node is not present.");

            if (matches.Count() > 1)
                throw new InvalidOperationException("More than one matching child node is present.");

            return (T)matches.Single();
        }

        public static T GetExactNode<T>(this ParserRuleContext context) where T : ParserRuleContext
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (context.children == null)
                throw new InvalidOperationException("Expected child node is not present.");

            var matches = context.GetChildren().Where(child => child.GetType() == typeof(T)).ToList();

            if (matches.Any() == false)
                throw new InvalidOperationException("Expected child node is not present.");

            if (matches.Count() > 1)
                throw new InvalidOperationException("More than one matching child node is present.");

            return (T)matches.Single();
        }
        public static bool TryGetExactNode<T>(this ParserRuleContext context, out T node) where T : ParserRuleContext
        {
            node = null;

            if (context == null)
                return false;

            if (context.children == null)
                return false;

            var matches = context.GetChildren().Where(child => child.GetType() == typeof(T)).ToList();

            if (matches.Any() == false)
                return false;

            if (matches.Count() > 1)
                throw new InvalidOperationException("More than one matching child node is present.");

            node = (T)matches.Single();

            return true;

        }
        public static bool TryGetDerivedNode<T>(this ParserRuleContext context, out T node) where T : ParserRuleContext
        {
            node = null;

            if (context == null)
                return false;

            if (context.children == null)
                return false;

            var matches = context.GetChildren().Where(child => child is T).ToList();

            if (matches.Any() == false)
                return false;

            if (matches.Count() > 1)
                throw new InvalidOperationException("More than one matching child node is present.");

            node = (T)matches.Single();

            return true;

        }

        public static List<T> GetExactNodes<T>(this ParserRuleContext context) where T : ParserRuleContext
        {
            if (context.children == null)
                return new List<T>(); ;

            var matches = context.GetChildren().Where(child => child.GetType() == typeof(T)).Cast<T>().ToList();

            if (matches.Any() == false)
                return new List<T>();

            return matches;
        }
        public static List<T> GetDerivedNodes<T>(this ParserRuleContext context) where T : ParserRuleContext
        {
            if (context.children == null)
                return new List<T>(); ;

            var matches = context.GetChildren().Where(child => child is T).Cast<T>().ToList();

            if (matches.Any() == false)
                return new List<T>();

            return matches;
        }

        public static string GetLabelText(this ParserRuleContext context, string Label)
        {
            return ((ParserRuleContext)(context.GetType().GetField(Label).GetValue(context))).GetText();
        }
        public static List<ParserRuleContext> GetChildren(this ParserRuleContext context)
        {
            if (context.children == null)
                return new List<ParserRuleContext>();

            return context.children.Where(c => (c is ParserRuleContext) && !excludedTypes.Contains(c.GetType())).Cast<ParserRuleContext>().ToList();
        }
    }
}