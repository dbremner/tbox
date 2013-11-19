using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Common.Base.Log;
using Common.Communications.Interprocess;
using Common.Communications.Network;
using Common.Tools;
using Interface;
using Localization.Plugins.TeamManager;
using PluginsShared.ScriptEngine;
using ScriptEngine;
using SkyNet.Code.Settings;
using SkyNet.Common.Configurations;
using SkyNet.Common.Contracts.Agent;
using SkyNet.Common.Contracts.Server;
using WPFControls.Code;
using WPFControls.Tools;

namespace SkyNet
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : ISettings
    {
        private readonly ILog log = LogManager.GetLogger<Settings>();
        public LazyDialog<ScriptsConfigurator> ScriptsConfigurator { get; set; }

        public Settings()
        {
            InitializeComponent();
        }

        private NetworkClient<ISkyNetServer> CreateServerClient(AgentConfig config)
        {
            return new NetworkClient<ISkyNetServer>(new Uri(config.ServerEndpoint));
        }

        private InterprocessClient<IConfigProvider<AgentConfig>> CreateAgentConfigProvider()
        {
            return new InterprocessClient<IConfigProvider<AgentConfig>>(AgentService.ServiceName);
        }

        private InterprocessClient<IConfigProvider<ServerConfig>> CreateServerConfigProvider()
        {
            return new InterprocessClient<IConfigProvider<ServerConfig>>(ServerService.ServiceName); 
        }

        public void Init(IPluginContext context)
        {
            AgentService.ServicePath = Path.Combine(context.DataProvider.ToolsPath, "SkyNet.Agent.exe");
            ServerService.ServicePath = Path.Combine(context.DataProvider.ToolsPath, "SkyNet.Server.exe");
            AgentSettingsNeedRefresh(null, new DependencyPropertyChangedEventArgs());
            ServerSettingsNeedRefresh(null, new DependencyPropertyChangedEventArgs());
        }

        public IList<string> FilePathes { get; set; }
        internal IScriptConfigurator ScriptConfigurator { get; set; }
        public UserControl Control { get { return this; } }

        private void ChangeAgentSettingsClick(object sender, RoutedEventArgs e)
        {
            SetConfig(CreateAgentConfigProvider(), AgentConfiguration.DataContext);
        }

        private void AgentSettingsNeedRefresh(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!AgentConfiguration.IsEnabled)
            {
                AgentConfiguration.DataContext = null;
                return;
            }
            var config = GetConfig(CreateAgentConfigProvider());
            AgentConfiguration.DataContext = config;
        }

        private void ChangeServerSettingsClick(object sender, RoutedEventArgs e)
        {
            SetConfig(CreateServerConfigProvider(), ServerConfiguration.DataContext);
        }

        private void ServerSettingsNeedRefresh(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!ServerConfiguration.IsEnabled)
            {
                ServerConfiguration.DataContext = null;
                return;
            }
            ServerConfiguration.DataContext = GetConfig(CreateServerConfigProvider());
        }

        private void SetConfig<T>(InterprocessClient<IConfigProvider<T>> cl, object config)
            where T : class
        {
            if (config == null)return;
            try
            {
                cl.Instance.Set((T)config);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't update configuration");
            }
        }

        private T GetConfig<T>(InterprocessClient<IConfigProvider<T>> cl)
            where T: class
        {
            try
            {
                return cl.Instance.Get();
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't get configuration");
            }
            return null;
        }

        private void RefreshInfoClick(object sender, RoutedEventArgs e)
        {
            var config = AgentConfiguration.DataContext as AgentConfig;
            if (config == null) return;
            try
            {
                var agentClient = new NetworkClient<ISkyNetAgent>(Environment.MachineName, config.Port);
                AgentInfo.DataContext = agentClient.Instance.GetCurrentTask();

                var serverClient = CreateServerClient(config);
                ConnectedAgents.ItemsSource =
                    serverClient.Instance.GetAgents()
                        .Select(x => string.Format("{0}\t{1}\t{2}", x.Endpoint, x.State, x.TotalCores));
                ExistTasks.ItemsSource =
                    serverClient.Instance.GetTasks()
                        .Select(x => string.Format("{0}\t{1}\t{2}", x.Owner, x.Progress, x.CreatedTime));
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't retreive information");
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
            ScriptsConfigurator.Value.ShowDialog(op, ScriptConfigurator, this.GetParentWindow());
        }

        private SingleFileOperation GetSelectedOperation(object sender)
        {
            var selectedKey = ((TextBlock)((DockPanel)((Button)sender).Parent).Children[3]).Text;
            var cfg = (Config)DataContext;
            var id = cfg.Operations.GetExistIndexByKeyIgnoreCase(selectedKey);
            return cfg.Operations[id];
        }

        public void Dispose()
        {
            ScriptsConfigurator.Dispose();
        }

        private void OnCheckChangedEvent(object sender, RoutedEventArgs e)
        {
            Ops.OnCheckChangedEvent(sender, e);
        }
    }
}
