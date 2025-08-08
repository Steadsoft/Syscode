using static SyscodeParser;

namespace Syscode
{
    public static class CompilerExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        /// <summary>
        /// Alternative way to populate a list rather than having to call ToList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void Load<T>(this List<T> target, IEnumerable<T> source)
        {
            target.Clear();
            target.AddRange(source);
        }

        /// <summary>
        /// Can't be called via a nullable reference
        /// </summary>
        /// <param name="context"></param>
        /// <param name="Creator"></param>
        /// <returns></returns>
        public static Expression SafeCreate(this ExpressionContext context,Func<ExpressionContext,Expression> Creator)
        {
            return Creator(context);
        }

        /// <summary>
        /// Can't be called via a nullable reference
        /// </summary>
        /// <param name="context"></param>
        /// <param name="Creator"></param>
        /// <returns></returns>
        public static Reference SafeCreate(this ReferenceContext context, Func<ReferenceContext, Reference> Creator)
        {
            return Creator(context);
        }

    }
}
