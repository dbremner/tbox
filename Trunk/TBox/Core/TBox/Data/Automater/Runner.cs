using Mnk.TBox.Core.PluginsShared.Automator;
using Mnk.Library.Solution.Scripts;

namespace Solution
{
    static class Runner
    {
        public static void Main()
        {
            new OptimizeSolution
                {
                    Solutions = new[]
                        {
                            @"d:\",
                        },
                    PathsToLibs = new[] { "..\\Libraries", "..\\packages" },
                    SectionToPreserveNewestNames = new[] { "Content", "None" },
                    TasksToRemove = new[]{"AjaxMin"},
                    DisableBuildEvents = true,
                }.Run(new ScriptContext());
        }
    }
}
