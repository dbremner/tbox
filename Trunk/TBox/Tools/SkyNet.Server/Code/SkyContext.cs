using System;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Tools.SkyNet.Server.Code
{
    class SkyContext : ISkyContext
    {
        public void Dispose()
        {
        }

        public void Update(float value)
        {
        }

        public void Update(string caption, float value)
        {
        }

        public bool UserPressClose { get; private set; }
        public void Update(Func<int, string> caption, int current, int total)
        {
        }

        public void Reset()
        {
            UserPressClose = false;
        }

        public void Cancel()
        {
            UserPressClose = true;
        }
    }
}
