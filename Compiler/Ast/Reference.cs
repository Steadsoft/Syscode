using System.Text;
using static SyscodeParser;

namespace Syscode
{

    public class Reference : AstNode
    {
        private Reference? preceding = null; // only populated if this ref is the left of ref -> ref
        private readonly List<Arguments> argumentsList = new();
        private readonly BasicReference basic;
        private bool resolved = false;
        private Report? report = null;
        public Reference(ReferenceContext context, AstBuilder builder) : base(context)
        {
            // A Reference might contain another Reference...

            if (context.Pointer != null)
            {
                preceding = builder.CreateReference(context.Pointer);
            }

            if (context.ArgsList != null)
            {
                var argsList = context.ArgsList._ArgsSet; /* one or more 'arguments' always present */

                foreach (var arguments in argsList)
                {
                    var argsast = new Arguments(arguments);

                    if (arguments.TryGetExactNode<SubscriptCommalistContext>(out var subscriptCommalist))
                    {
                        var expressions = subscriptCommalist.GetDerivedNodes<ExpressionContext>().Select(builder.CreateExpression).ToList();

                        argsast.ExpressionList.AddRange(expressions);
                    }

                    argumentsList.Add(argsast);
                }
            }

            // TODO: process the optional ArgList list..

            basic = builder.CreateBasicReference(context.Basic);
        }
        /// <summary>
        /// Indicates whether the reference has a preceding 'pointer' qualifier.
        /// </summary>
        public bool IsJustBasicReference
        {
            get { return Pointer == null; }
        }
        public bool IsResolved { get => resolved; internal set => resolved = value; }
        public bool IsntResolved { get => !resolved; }
        public bool IsntJustBasicReference { get => !IsJustBasicReference; }
        public BasicReference BasicReference { get => basic;  }
        public Reference Pointer { get => preceding; }
        public IReadOnlyList<Arguments> ArgumentsList { get => argumentsList;  }
        /// <summary>
        /// This is a diagnostic that must be reported if present, it is only ever present on qualified references. 
        /// </summary>
        public Report? Report { get => report; set => report = value; }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (Pointer != null)
            {
                builder.Append(Pointer.ToString());
                builder.Append(" -> ");
            }

           builder.Append(BasicReference.ToString());

           foreach (var arg in ArgumentsList)
            {
                builder.Append(arg.ToString());
            }

           return builder.ToString();
        }
    }
}