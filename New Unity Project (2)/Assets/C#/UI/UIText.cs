using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour {

    //Variables
    public Text coinText;
    public GameObject deathText;
    GameManagerScript gMScript;

    public string levelCoins;
    public int level;

    //Start
    void Start()
    {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get GameManager

        //Get stored coins and display them
        coinText.text = gMScript.LoadJson().levelCoins[level - 1].ToString() + " / " + levelCoins;

        //Get stored deaths and display them
        deathText.GetComponent<Text>().text = gMScript.LoadJson().levelDeaths[level - 1].ToString();
    }
}
