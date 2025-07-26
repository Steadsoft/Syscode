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

    }
}
