using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{

    public Text coinText;
    public int levelCoins, coins = 0;

    //Count coins in current level
    void Start ()
    {
        if (PlayerPrefs.GetInt("ShowCoins") == 0) coinText.text = "";
        levelCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        if (coinText.text != "") coinText.text = "Coins: " + coins.ToString() + " / " + levelCoins.ToString();
    }

    //Add coins and update UI
    public void addCoin()
    {
        coins += 1;
        if (coinText.text != "") coinText.text = "Coins: " + coins.ToString() + " / " + levelCoins.ToString();
        GameObject.Find("Player").GetComponent<ParticleSystem>().Play();
    }
}