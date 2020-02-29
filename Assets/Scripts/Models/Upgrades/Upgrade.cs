using PrimitiveFactory.ScriptableObjectSuite;
using UnityEngine;

public class Upgrade : ScriptableObjectExtended
{
    public Sprite Icon;
    public string Name;

    public float DamageMultiplication = 1;
    public float AttackSpeedMultiplication = 1;

    public UpgradeEffect Effect;
}
