using PrimitiveFactory.ScriptableObjectSuite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelect : ScriptableObjectExtended
{
    public Upgrade SelectedUpgrade;

    private Random randomValue;
    private List<Upgrade> selectableUpgrades;
    private List<Upgrade> allUpgrades; //Stun, Lifesteal, Poision, Slow, Aoe

    public void Button1()
    {
        SelectedUpgrade = selectableUpgrades[0];
    }
    public void Button2()
    {
        SelectedUpgrade = selectableUpgrades[1];
    }
    public void Button3()
    {
        SelectedUpgrade = selectableUpgrades[2];
    }
    private void Awake()
    {
        randomValue = new Random();
        allUpgrades = new List<Upgrade>();
    }
    private void generatePossibleUpgradeEffects()
    {
        //Daniel sagt ja ohne reflection also hier umständlich:
        Upgrade tmp = new Upgrade();
        tmp.Effect.Stun = true;
        tmp.Effect.Name = "StunEffect";
        tmp.Name = "Stun";
        allUpgrades.Add(tmp);

        tmp = new Upgrade();
        tmp.Effect.Lifesteal = true;
        tmp.Effect.Name = "Lifesteal";
        tmp.Name = "Lifesteal";
        allUpgrades.Add(tmp);

        tmp = new Upgrade();
        tmp.Effect.Dot = true;
        tmp.Effect.Name = "Poision";
        tmp.Effect.Duration = 5;
        tmp.Effect.Damage = 3;
        tmp.Name = "Poision";
        allUpgrades.Add(tmp);

        tmp = new Upgrade();
        tmp.Effect.Slow = true;
        tmp.Effect.Name = "Slow";
        tmp.Effect.Duration = 3;
        tmp.Name = "Slow";
        allUpgrades.Add(tmp);

        tmp = new Upgrade();
        tmp.Effect.Aoe = true;
        tmp.Effect.Name = "Aoe";
        tmp.Effect.Radius = 3;
        tmp.Name = "Bumm";
        allUpgrades.Add(tmp);

        tmp = new Upgrade();
        tmp.Effect.Slow = true;
        tmp.Effect.Name = "Burn";
        tmp.Effect.Duration = 3;
        tmp.Effect.Damage = 5;
        tmp.Name = "BurnDot";
        allUpgrades.Add(tmp);

    }
    private void randomizeUpgrades(int Power)
    {
        selectableUpgrades = new List<Upgrade>(3);
        for (int i = 0; i < 3; i++)
        {
            Upgrade upgrade = (allUpgrades[Random.Range(0, allUpgrades.Count)]);
            upgrade.Effect.Damage *= Power;
            selectableUpgrades.Add(upgrade);
        }
    }
}
