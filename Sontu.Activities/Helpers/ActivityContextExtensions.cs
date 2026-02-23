using System.Activities;
using System.Net;
using UiPath.Robot.Activities.Api;

namespace Sontu.Activities.Helpers
{
    public static class GlobalAuthStore
    {
        public static CookieContainer CookieContainer { get; set; }
        public static string UserEmail { get; set; }
        public static bool IsScopeActive { get; set; }
    }
    public static class ActivityContextExtensions
    {
        public static IExecutorRuntime GetExecutorRuntime(this ActivityContext context) => context.GetExtension<IExecutorRuntime>();

    }
}