using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUi : MonoBehaviour

{
 
    public PlayerController PlayerController;

    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI LevelText;
    public int Level;
    public int Gold;
    public int DebugSelectedUpgrade;
    public Upgrade SelectedUpgrade;
    public UpgradeSelect UpgradeSelection;

    public void SetGoldText()
    {
        //GoldText.text =  PlayerController.Gold.ToString();
        GoldText.text = Gold.ToString();
    }
    public void SetLevelText()
    {
        LevelText.text = "Level: " + Level;
    }

    public void SelectUpgradeButton1()
    {
        UpgradeSelection.Button1();
        SelectedUpgrade = UpgradeSelection.SelectedUpgrade;
        Gold = 1;
    }
    public void SelectUpgradeButton2()
    {
        UpgradeSelection.Button2();
        SelectedUpgrade = UpgradeSelection.SelectedUpgrade;
        Gold = 2;
    }
    public void SelectUpgradeButton3()
    {
        UpgradeSelection.Button3();
        SelectedUpgrade = UpgradeSelection.SelectedUpgrade;
        Gold = 3;
    }
    public void Awake()
    {
        SetGoldText();
        SetLevelText();
        DebugSelectedUpgrade = 0;
        UpgradeSelection = new UpgradeSelect();

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetGoldText();
        SetLevelText();
    }

    
}
