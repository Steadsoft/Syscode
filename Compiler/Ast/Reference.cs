using Antlr4.Runtime;
using System.Text;
using static SyscodeParser;

namespace Syscode
{

    public class Reference : AstNode, IReplaceContainer
    {
        private readonly Reference? preceding = null; // only populated if this ref is the left of ref -> ref
        private readonly List<Arguments> argumentsList = new();
        private readonly BasicReference basic;
        private bool resolved = false;
        private Report? report = null;
        public Reference(ReferenceContext context, SyscodeAstBuilder builder) : base(context)
        {
            // A Reference might contain another Reference...

            if (context.Pointer != null)
            {
                preceding = builder.CreateReference(context.Pointer);
            }

            if (context.ArgsList != null)
            {
                argumentsList = context.ArgsList._ArgsSet.Select(a => new Arguments(a, builder)).ToList();
            }

            basic = builder.CreateBasicReference(context.Basic);
        }
        public bool IsSimpleIdenitifer
        {
            get
            {
                if (preceding == null && argumentsList.Count == 0)
                {
                    if (basic.IsntQualified)
                        return true;
                }

                return false;
            }
        }

        public string SimpleIdentifer
        {
            get
            {
                if (!IsSimpleIdenitifer)
                    throw new InvalidOperationException("This operation is not possible on this reference");

                return BasicReference.Spelling;
            }
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

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            foreach (var args in argumentsList)
            {
                args.ApplyPreprocessorReplace(tokens, replace);
            }

            preceding?.ApplyPreprocessorReplace(tokens, replace);
            basic.ApplyPreprocessorReplace(tokens, replace);
        }

        public override string ToString()
        {
            StringBuilder builder = new();

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