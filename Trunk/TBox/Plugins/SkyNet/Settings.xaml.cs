using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Common.Base.Log;
using Mnk.Library.Common.Communications.Interprocess;
using Mnk.Library.Common.Communications.Network;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Locales.Localization.Plugins.TeamManager;
using Mnk.TBox.Core.PluginsShared.ScriptEngine;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Plugins.SkyNet.Code.Settings;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Agent;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;
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
        public LazyDialog<ScriptsConfigurator> ScriptsConfigurator { get; set; }

        public Settings()
        {
            InitializeComponent();
        }

        private static NetworkClient<ISkyNetServer> CreateServerClient(AgentConfig config)
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
            using (var cl = CreateAgentConfigProvider())
            {
                SetConfig(cl, AgentConfiguration.DataContext);
            }
        }

        private void AgentSettingsNeedRefresh(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!AgentConfiguration.IsEnabled)
            {
                AgentConfiguration.DataContext = null;
                return;
            }
            using (var cl = CreateAgentConfigProvider())
            {
                var config = GetConfig(cl);
                AgentConfiguration.DataContext = config;
            }
        }

        private void ChangeServerSettingsClick(object sender, RoutedEventArgs e)
        {
            using (var cl = CreateServerConfigProvider())
            {
                SetConfig(cl, ServerConfiguration.DataContext);
            }
        }

        private void ServerSettingsNeedRefresh(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!ServerConfiguration.IsEnabled)
            {
                ServerConfiguration.DataContext = null;
                return;
            }
            using (var cl = CreateServerConfigProvider())
            {
                ServerConfiguration.DataContext = GetConfig(cl);
            }
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

                using (var serverClient = CreateServerClient(config))
                {
                    ConnectedAgents.ItemsSource =
                        serverClient.Instance.GetAgents()
                            .Select(x => string.Format("{0}\t{1}\t{2}", x.Endpoint, x.State, x.TotalCores));
                    ExistTasks.ItemsSource =
                        serverClient.Instance.GetTasks()
                            .Select(x => string.Format("{0}\t{1}\t{2}", x.Owner, x.Progress, x.CreatedTime));
                }
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
