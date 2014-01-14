using System.Collections.Generic;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;

namespace Mnk.TBox.Tools.SkyNet.Common
{
    public interface ISkyScript
    {
        // this part will be executed on the each server
        #region Server
        string[] DivideTasks(ServerAgent[] agents);
        void Update(int agentId, string data);
        //this method should return report
        string BuildResult(IDictionary<int,string> results);
        #endregion Server

        // this part will be executed on the each agent
        #region Agent
        //this method should return report
        string Execute(string data, ISkyContext context);
        #endregion Agent
    }
}
