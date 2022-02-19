using System.Collections;

namespace FluentAssertions.Properties.Tests.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable AsNonGenericEnumerable(this IEnumerable source)
        {
            foreach (object o in source)
            {
                yield return o;
            }
        }

        public static int Count(this IEnumerable source)
        {
            int count = 0;

            foreach (object o in source)
            {
                count++;
            }

            return count;
        }
    }
}
