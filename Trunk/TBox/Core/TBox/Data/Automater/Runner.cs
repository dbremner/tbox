﻿using PluginsShared.Automator;
using Solution.Scripts;

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
                    PathesToLibs = new[] { "..\\Libraries", "..\\packages" },
                    SectionToPreserveNewestNames = new[] { "Content", "None" },
                    TasksToRemove = new[]{"AjaxMin"},
                    DisableBuildEvents = true,
                }.Run(new ScriptContext());
        }
    }
}
