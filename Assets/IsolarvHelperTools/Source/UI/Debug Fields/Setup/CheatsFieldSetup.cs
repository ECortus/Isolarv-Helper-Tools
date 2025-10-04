using UnityEngine;

namespace IsolarvHelperTools.Source.UI
{
    public class CheatsFieldSetup : BaseFieldSetup
    {
        protected override void Setup()
        {
            FieldManager.RegisterDebugButton("Test", () => { DebugHelper.Log("--Test cheats--"); });
        }
    }
}