using PrimitiveFactory.ScriptableObjectSuite;
using UnityEngine;

public class UpgradeEffect : ScriptableObjectExtended
{
    public Sprite Icon;
    public string Name;

    //Effects
    public bool Dot;
    public bool Lifesteal;
    public bool Aoe;
    public bool Stun;
    public bool Slow;

    [Range(0, 1)]
    public float EffectChance;

    //Dot
    public float Duration;
    public float Damage;

    //Lifesteal
    [Range(0, 1)]
    public float HealDamagePercent;

    //Aoe
    public float Radius;
}
