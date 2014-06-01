namespace Mnk.Library.Tests.Common
{
    static class Shared
    {
        public const string CompileMode =
#if DEBUG
            "DEBUG";
#else
            "Release";
#endif

    }
}
