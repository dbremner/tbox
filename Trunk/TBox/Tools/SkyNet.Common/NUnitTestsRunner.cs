using System;
using System.Collections.Generic;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;

namespace Mnk.TBox.Tools.SkyNet.Common
{
    public class NUnitTestsRunner : ISkyScript
    {
        [File]
        public string TestDllPath { get; set; }
        [StringList("*.dll", "*.config", CanBeEmpty = false)]
        public string[] CopyMasks { get; set; }
        [Bool(true)]
        public bool RanAsx86 { get; set; }
        [Bool(true)]
        public bool Multithreaded { get; set; }
        [Integer(2, Max = 4, Min = 1)]
        public int MaxProcessCount { get; set; }
        [Bool]
        public bool UseCategories { get; set; }
        [Bool(true)]
        public bool IncludeCategories { get; set; }
        [StringList(CanBeEmpty=true)]
        public string[] Categories { get; set; }

        public string[] DivideTasks(ServerAgent[] agents)
        {
            throw new NotImplementedException();
        }

        public void Update(int agentId, string data)
        {
            throw new NotImplementedException();
        }

        public string BuildResult(IDictionary<int, string> results)
        {
            throw new NotImplementedException();
        }


        public string Execute(string data, ISkyContext context)
        {
            throw new NotImplementedException();
        }
    }
}
