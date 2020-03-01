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


    public void SetGoldText()
    {
        //GoldText.text =  PlayerController.Gold.ToString();
        GoldText.text = Gold.ToString();
    }
    public void SetLevelText()
    {
        LevelText.text = "Level: " + Level;
    }
    public void Awake()
    {
        SetGoldText();
        SetLevelText();

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
