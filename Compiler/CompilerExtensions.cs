namespace Syscode
{
    public static class CompilerExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static void Load<T>(this List<T> target, IEnumerable<T> source)
        {
            target.Clear();
            target.AddRange(source);
        }

    }
}
