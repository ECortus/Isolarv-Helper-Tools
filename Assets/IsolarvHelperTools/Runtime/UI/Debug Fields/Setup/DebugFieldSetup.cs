namespace IsolarvHelperTools.Runtime.UI
{
    public class DebugFieldSetup : BaseFieldSetup
    {
        protected override void Setup()
        {
            FieldManager.RegisterDebugToggle("Enable Logging", (val) =>
            {
                IsolarvDebugConfig.SetChanges((ref IsolarvDebugConfig config) =>
                {
                    config.ENABLE_MANUAL_LOGGING = val;
                });
            }, IsolarvDebugConfig.GetConfig().ENABLE_MANUAL_LOGGING);
        }
    }
}