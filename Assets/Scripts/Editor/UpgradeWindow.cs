using PrimitiveFactory.ScriptableObjectSuite;
using UnityEditor;

public class MeeleUpgradeWindow : ScriptableObjectEditorWindow<MeeleUpgrade>
{
    // Name of the object (Display purposes)
    protected override string c_ObjectName { get { return "Upgrade"; } }
    // Relative path from Project Root
    protected override string c_ObjectFullPath { get { return "Assets/Resources/MeeleUpgrade"; } }

    [MenuItem("ScriptableObjects/Upgrades/Meele")]
    public static void ShowWindow()
    {
        MeeleUpgradeWindow window = GetWindow<MeeleUpgradeWindow>("Meele Upgrade Editor");
        window.Show();
    }
}

public class RangeUpgradeWindow : ScriptableObjectEditorWindow<RangeUpgrade>
{
    // Name of the object (Display purposes)
    protected override string c_ObjectName { get { return "Upgrade"; } }
    // Relative path from Project Root
    protected override string c_ObjectFullPath { get { return "Assets/Resources/RangeUpgrade"; } }

    [MenuItem("ScriptableObjects/Upgrades/Range")]
    public static void ShowWindow()
    {
        RangeUpgradeWindow window = GetWindow<RangeUpgradeWindow>("Range Upgrade Editor");
        window.Show();
    }
}

public class EffectUpgradeWindow : ScriptableObjectEditorWindow<UpgradeEffect>
{
    // Name of the object (Display purposes)
    protected override string c_ObjectName { get { return "Upgrade"; } }
    // Relative path from Project Root
    protected override string c_ObjectFullPath { get { return "Assets/Resources/EffectUpgrade"; } }

    [MenuItem("ScriptableObjects/Upgrades/Effect")]
    public static void ShowWindow()
    {
        EffectUpgradeWindow window = GetWindow<EffectUpgradeWindow>("Effect Upgrade Editor");
        window.Show();
    }
}