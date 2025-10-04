namespace IsolarvHelperTools.Source.UI
{
    public class ExampleFieldSetup : BaseFieldSetup
    {
        protected override void Setup()
        {
            FieldManager.RegisterDebugToggle("Example Toggle", (val) => DebugHelper.Log("Example Toggle"), false);
            FieldManager.RegisterDebugButton("Example Button", () => DebugHelper.Log("Example Button"));
        }
    }
}