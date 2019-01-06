using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{

    public Text coinText;
    public GameObject coinImage;
    public int levelCoins, coins = 0;

    //Count coins in current level
    void Start ()
    {
        levelCoins = GameObject.FindGameObjectsWithTag("Coin").Length; //Get number of coins in level

        if (PlayerPrefs.GetInt("ShowCoins") == 0) //If coin counter is disabled
        {
            coinText.text = "";
            coinImage.SetActive(false);
        }
        else
        {
            coinText.text = coins.ToString() + " / " + levelCoins.ToString();
            coinImage.SetActive(true);
        }
    }

    //Add coins and update UI
    public void AddCoin()
    {
        coins += 1;
        if (coinText.text != "") coinText.text = coins.ToString() + " / " + levelCoins.ToString();
        GameObject.Find("Player").GetComponent<ParticleSystem>().Play();
    }
}