using System.Collections.Generic;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;

namespace Mnk.TBox.Tools.SkyNet.Common
{
    public interface ISkyScript
    {
        string DataFolder { get; set; }
        string[] CopyMasks { get; set; }

        // this part will be executed on the each server
        #region Server
        string[] ServerDivideTasks(ServerAgent[] agents, ISkyContext context);
        //this method should return report
        string ServerBuildResult(IDictionary<string,string> results);
        #endregion Server

        // this part will be executed on the each agent
        #region Agent
        //this method should return report
        string AgentExecute(string data, ISkyContext context);
        #endregion Agent
    }
}
