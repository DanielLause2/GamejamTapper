using UnityEditor;

namespace PrimitiveFactory.ScriptableObjectSuite
{
    public class ScriptableObjectSuiteMenu
    {
        [MenuItem("Primitive/Scriptable Object Suite/Class Object Wizard")]
        public static void ShowWindow()
        {
            ScriptableObjectWizard.ShowWindow();
        }

        [MenuItem("Primitive/Scriptable Object Suite/Refresh all Load on Demand References")]
        public static void RefreshAllLoDReferences()
        {
            ScriptableObjectExtended.RefreshAllLoDReferences();
        }
    }
}
