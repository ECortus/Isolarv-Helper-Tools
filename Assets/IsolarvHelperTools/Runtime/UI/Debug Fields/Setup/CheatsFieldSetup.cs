using UnityEngine;

namespace IsolarvHelperTools.Runtime.UI
{
    public class CheatsFieldSetup : BaseFieldSetup
    {
        protected override void Setup()
        {
            FieldManager.RegisterDebugButton("Test", () => { DebugHelper.Log("--Test cheats--"); });
        }
    }
}