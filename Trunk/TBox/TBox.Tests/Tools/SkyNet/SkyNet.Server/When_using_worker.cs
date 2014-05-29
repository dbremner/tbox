using Mnk.Library.ScriptEngine.Core.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Server.Code.Processing;
using NUnit.Framework;
using Rhino.Mocks;
using ScriptEngine.Core.Params;

namespace Mnk.TBox.Tests.Tools.SkyNet.SkyNet.Server
{
    [TestFixture]
    class When_using_worker
    {
        private IScriptCompiler<ISkyScript> compiler;
        private ISkyContext context;
        private ISkyAgentLogic agentLogic;
        private IWorker worker;
        private ServerAgent[] serverAgents;
        private ServerTask serverTask;
        private ISkyScript script;
        
        [SetUp]
        public void SetUp()
        {
            compiler = MockRepository.GenerateStrictMock<IScriptCompiler<ISkyScript>>();
            context = MockRepository.GenerateStrictMock<ISkyContext>();
            agentLogic = MockRepository.GenerateStrictMock<ISkyAgentLogic>();
            worker = new Worker(compiler,context,agentLogic);
            serverAgents = new[] { new ServerAgent { } };
            serverTask = new ServerTask
            {
                ScriptParameters = "[]",
                Script = "Script",
                ZipPackageId = "ZipPackageId"
            };
            script = MockRepository.GenerateMock<ISkyScript>();
            compiler.Stub(x => x.Compile(serverTask.Script, new Parameter[0])).Return(script);
        }

        [TearDown]
        public void TearDown()
        {
            compiler.VerifyAllExpectations();
            context.VerifyAllExpectations();
            agentLogic.VerifyAllExpectations();
        }

        [Test]
        public void Should_process_task()
        {
            //Arrange
            agentLogic.Stub(x => x.IsAlive(serverAgents[0]))
                .Return(true);
            var saw = new[] {new SkyAgentWork
            {
                Agent = serverAgents[0], 
                Config = "AGENTCONFIG", 
                Report = "REPORT"
            }};
            script.Stub(x => x.ServerBuildAgentsData(serverAgents, context))
                .Return(saw);
            var wt = new WorkerTask
            {
                Agent = serverAgents[0],
                Task = new AgentTask
                {
                    Config = saw[0].Config,
                    Script = serverTask.Script,
                    ZipPackageId = serverTask.ZipPackageId
                }
            };
            agentLogic.Stub(x => x.CreateWorkerTask(serverAgents[0], saw[0].Config, serverTask ))
                .Return(wt);

            agentLogic.Stub(x => x.IsDone(wt))
                .Return(true);

            agentLogic.Stub(x => x.BuildReport(wt))
                .Return(saw[0]);

            script.Stub(x => x.ServerBuildResultByAgentResults(saw))
                .Return("FULLREPORT");

            //Act
            worker.ProcessTask(serverTask, serverAgents);

            //Assert
            Assert.AreEqual("FULLREPORT", serverTask.Report);

        }
    }
}
