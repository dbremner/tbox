using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages
{
    public sealed class MultiTestsFixture : IDisposable
    {
        private readonly ILog log = LogManager.GetLogger<MultiTestsFixture>();
        private readonly ITestsConfig[] configs;
        private readonly int assembliesInParallel;
        private IList<ExecutionContext> executionContexts = new ExecutionContext[0];

        public MultiTestsFixture(ITestsConfig[] configs, int assembliesInParallel)
        {
            this.configs = configs;
            this.assembliesInParallel = assembliesInParallel;
        }

        public IList<ExecutionContext> Refresh()
        {
            Dispose();
            return executionContexts = ((assembliesInParallel > 1) ?
                configs.AsParallel().Select(Collect) :
                configs.Select(Collect)
                ).ToArray();
        }

        public void Run(ITestsUpdater testsUpdater, Action<ExecutionContext> onEnd)
        {
            if (assembliesInParallel > 1)
            {
                Parallel.ForEach(executionContexts,
                    new ParallelOptions { MaxDegreeOfParallelism = assembliesInParallel },
                    assembly => RunTest(assembly, testsUpdater, onEnd)
                    );
            }
            else
            {
                foreach (var context in executionContexts)
                {
                    RunTest(context, testsUpdater, onEnd);
                }
            }
        }

        private static void RunTest(ExecutionContext context, ITestsUpdater testsUpdater, Action<ExecutionContext> onEnd)
        {
            context.StartTime = Environment.TickCount;
            context.Results = context.TestsFixture.Run(context.Config, context.Results, testsUpdater);
            if (context.Results.Metrics.FailedCount > 0)
            {
                context.RetValue = -2;
            }
            onEnd(context);
        }

        private ExecutionContext Collect(ITestsConfig config)
        {
            var context = new ExecutionContext
            {
                Path = config.TestDllPath,
                Config = config,
                RetValue = 0,
                Container = ServicesRegistrar.Register(),
            };
            context.TestsFixture = context.Container.GetInstance<ITestsFixture>();
            if (!context.TestsFixture.EnsurePathIsValid(context.Config))
            {
                log.Write("Incorrect path: " + config.TestDllPath);
                context.RetValue = -3;
            }
            else
            {
                context.Results = context.TestsFixture.Refresh(context.Config);
                if (!context.Results.IsFailed) return context;
                log.Write("Can't calculate tests count");
                context.RetValue = -3;
            }
            return context;
        }

        public void Dispose()
        {
            foreach (var context in executionContexts)
            {
                context.Dispose();
            }
            executionContexts = new ExecutionContext[0];
        }
    }
}
