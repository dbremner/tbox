using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Common.Base.Log;
using Mnk.Library.Common.Communications.Network;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.PluginsShared.ScriptEngine;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Plugins.SkyNet.Code;
using Mnk.TBox.Plugins.SkyNet.Code.Settings;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Agent;
using Mnk.Library.WPFControls.Code;
using Mnk.Library.WPFControls.Tools;

namespace Mnk.TBox.Plugins.SkyNet
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : ISettings
    {
        private readonly ILog log = LogManager.GetLogger<Settings>();
        public LazyDialog<ScriptsConfigurator> ScriptConfiguratorDialog { get; set; }
        public ServicesFacade ServicesFacade { get; set; }

        public Settings()
        {
            InitializeComponent();
        }

        public void Init(IPluginContext context)
        {
            AgentService.ServiceName = ServicesFacade.AgentServiceName;
            AgentService.ServicePath = Path.Combine(context.DataProvider.ToolsPath, "Mnk.TBox.Tools.SkyNet.Agent.exe");
            ServerService.ServiceName = ServicesFacade.ServerServiceName;
            ServerService.ServicePath = Path.Combine(context.DataProvider.ToolsPath, "Mnk.TBox.Tools.SkyNet.Server.exe");
            AgentSettingsNeedRefresh(null, new DependencyPropertyChangedEventArgs());
            ServerSettingsNeedRefresh(null, new DependencyPropertyChangedEventArgs());
        }

        public IList<string> FilePathes { get; set; }
        internal IScriptConfigurator ScriptConfigurator { get; set; }
        public UserControl Control { get { return this; } }

        private void ChangeAgentSettingsClick(object sender, RoutedEventArgs e)
        {
            ServicesFacade.SetAgentConfig((AgentConfig)AgentConfiguration.DataContext);
        }

        private void AgentSettingsNeedRefresh(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!AgentConfiguration.IsEnabled)
            {
                AgentConfiguration.DataContext = null;
                return;
            }
            AgentConfiguration.DataContext = ServicesFacade.GetAgentConfig();
        }

        private void ChangeServerSettingsClick(object sender, RoutedEventArgs e)
        {
            ServicesFacade.SetServerConfig((ServerConfig)ServerConfiguration.DataContext);
        }

        private void ServerSettingsNeedRefresh(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!ServerConfiguration.IsEnabled)
            {
                ServerConfiguration.DataContext = null;
                return;
            }
            ServerConfiguration.DataContext = ServicesFacade.GetServerConfig();
        }

        private void RefreshInfoClick(object sender, RoutedEventArgs e)
        {
            var config = AgentConfiguration.DataContext as AgentConfig;
            if (config == null) return;
            try
            {
                using (var agentClient = new NetworkClient<ISkyNetAgentService>(Environment.MachineName, config.Port))
                {
                    AgentInfo.DataContext = agentClient.Instance.GetCurrentTask();

                    using (var serverClient = ServicesFacade.CreateServerClient(config))
                    {
                        ConnectedAgents.ItemsSource =
                            serverClient.Instance.GetAgents()
                                .Select(x => string.Format("{0}\t{1}\t{2}", x.Endpoint, x.State, x.TotalCores));
                        ExistTasks.ItemsSource =
                            serverClient.Instance.GetTasks()
                                .Select(x => string.Format("{0}\t{1}\t{2}", x.Owner, x.Progress, x.CreatedTime));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't retrieve information");
            }
        }

        private void BtnSetOperationParametersClick(object sender, RoutedEventArgs e)
        {
            var op = GetSelectedOperation(sender);
            if (string.IsNullOrEmpty(op.Path))
            {
                MessageBox.Show("{PleaseSpecifyScriptPath}");
                return;
            }
            ScriptConfiguratorDialog.Value.ShowDialog(op, ScriptConfigurator, this.GetParentWindow());
        }

        private SingleFileOperation GetSelectedOperation(object sender)
        {
            var selectedKey = (string)((Button)sender).DataContext;
            var cfg = (Config)DataContext;
            var id = cfg.Operations.GetExistIndexByKeyIgnoreCase(selectedKey);
            return cfg.Operations[id];
        }

        public void Dispose()
        {
            ScriptConfiguratorDialog.Dispose();
        }

        private void OnCheckChangedEvent(object sender, RoutedEventArgs e)
        {
            Ops.OnCheckChangedEvent(sender, e);
        }
    }
}
