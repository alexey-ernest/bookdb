using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BookDb.Infrastructure.Threading
{
    public static class TaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoWarning(this Task task)
        {
        }
    }
}