using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public PlayerController PlayerController;
    public TextMeshProUGUI MaxHpText;
    public TextMeshProUGUI MaxASText;
    public TextMeshProUGUI MaxMSText;
    public TextMeshProUGUI MaxATKText;

    private int maxHp = 0;
    private int maxAS = 0;
    private int maxMS = 0;
    private int maxATK = 0;
    private Dictionary<string, int> prices;
    // Start is called before the first frame update
    public void BuyMaxHP()
    {
        PlayerController.BoughtUpgrades.Remove("MaxHP");
        PlayerController.BoughtUpgrades.Add("MaxHP", maxHp++);
        prices.Remove("MaxHP");
        int price  = PriceCalculation(maxHp);
        prices.Add("MaxHP", price);
        MaxHpText.text = "Max Health cost:" + price;

    }
    public void BuyMaxAS()
    {
        PlayerController.BoughtUpgrades.Remove("MaxAS");
        PlayerController.BoughtUpgrades.Add("MaxAS", maxAS++);
        prices.Remove("MaxAS");
        int price = PriceCalculation(maxAS);
        prices.Add("MaxAS", price);
        MaxHpText.text = "Attackspeed cost:" + price;
    }
    public void BuyMaxMS()
    {
        PlayerController.BoughtUpgrades.Remove("MaxMS");
        PlayerController.BoughtUpgrades.Add("MaxMS", maxMS++);
        prices.Remove("MaxMS");
        int price = PriceCalculation(maxMS);
        prices.Add("MaxMS", price);
        MaxHpText.text = "Max Movementspeed cost:" + price;
    }
    public void BuyMaxATK()
    {
        PlayerController.BoughtUpgrades.Remove("MaxATK");
        PlayerController.BoughtUpgrades.Add("MaxATK", maxATK++);
        prices.Remove("MaxATK");
        int price = PriceCalculation(maxATK);
        prices.Add("MaxATK", price);
        MaxHpText.text = "Max ATK cost:" + price;
    }
    public void Awake()
    {
        PlayerController.BoughtUpgrades.TryGetValue("MaxHP", out maxHp);
        PlayerController.BoughtUpgrades.TryGetValue("MaxAS", out maxAS);
        PlayerController.BoughtUpgrades.TryGetValue("MaxMS", out maxMS);
        PlayerController.BoughtUpgrades.TryGetValue("MaxATK", out maxATK);
        prices.Add("MaxHP", PriceCalculation(maxHp));
        prices.Add("MaxAS", PriceCalculation(maxAS));
        prices.Add("MaxMS", PriceCalculation(maxMS));
        prices.Add("MaxATK", PriceCalculation(maxATK));

    }
    public void FixedUpdate()
    {
        //PlayerController.BoughtUpgrades.TryGetValue("MaxHP", out int hp);
    }

    private int PriceCalculation(int basisWert)
    {
        return (int)((Mathf.Pow((float)1.1, basisWert) * 10) + 10);
    }
}
