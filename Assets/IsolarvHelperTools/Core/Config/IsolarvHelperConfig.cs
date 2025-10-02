using System;

namespace IsolarvHelperTools
{
    [Serializable]
    public class IsolarvHelperConfig
    {
        static IsolarvHelperConfig _i = null;
        static IsolarvHelperConfig Instance
        {
            get
            {
                if (_i == null)
                    _i = IsolarvHelperConfigHandler.GetEditorSettings();
                return _i;
            }
        }
        
        public bool enableProjectDebug = false;

        #region Logging

        public bool enableLogging = false;
        
        public static bool ENABLE_LOGGING => Instance.enableProjectDebug && Instance.enableLogging;

        #endregion
    }
}