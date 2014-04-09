using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mnk.Library.WPFControls.Dialogs.StateSaver;
using Mnk.TBox.Core.Interface;

namespace Mnk.TBox.Plugins.PasswordsStorage.Code.Settings
{
    [Serializable]
    public class Config : IConfigWithDialogStates
    {
        public IDictionary<string, DialogState> States { get; set; }
        public ObservableCollection<Profile> Profiles { get; set; }
        public string SelectedProfile { get; set; }

        public Config()
        {
            Profiles = new ObservableCollection<Profile>
            {
                new Profile
                {
                    Key = "Sample",
                    LoginInfos = new ObservableCollection<LoginInfo>
                    {
                        new LoginInfo{Key = "Hello", Login = "world", Password = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAiGPZ1NNPb0m2IuWEoZhEewAAAAACAAAAAAAQZgAAAAEAACAAAACwPE2JgKU3irD1nlQOKu1ODMQK07kkr26Fj5mnRoOLMAAAAAAOgAAAAAIAACAAAABJSlu0gqIxoW9aQLOKw1Aq5jy6Q0bkiYJAcP+USxtZ4RAAAAAXjz373f99PhjSIES1nzwrQAAAADNwHJIhnH/dG2au0UqVlCV21DvUMRCrBsDRxuTaIROsUp16iYm5OnwIbIOYn4lwhmPyuBX9uLyFYVulmjLSES8="}
                    }
                }
            };
            States = new Dictionary<string, DialogState>();
        }
    }
}
