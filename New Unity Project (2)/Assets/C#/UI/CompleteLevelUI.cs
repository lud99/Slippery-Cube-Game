﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CompleteLevelUI : MonoBehaviour {

    CoinText coinText;
    Text coinTotal, deathText;
    GameManagerScript gMScript;
    public bool levelComplete, worldComplete, addCoinsDone = false;
    public int bonusCoins;
    int levelCoins, addedCoins, coins, sceneBuildIndex;

    //Start
    void Start () {
        //Find objects
        coinText = GameObject.Find("CoinText").GetComponent<CoinText>(); //Collected coins
        coinTotal = GameObject.Find("CoinTotal").GetComponent<Text>(); //Coin text
        deathText = GameObject.Find("DeathTotal").GetComponent<Text>(); //Death text
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //GameManager

        //Get and display deaths
        sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        deathText.GetComponent<Text>().text = gMScript.LoadJson().localDeaths.ToString();

        if (levelComplete)
        {
            //Display coins
            coinTotal.GetComponent<Text>().text = "Coins: " + coinText.coins.ToString() + " / " + coinText.levelCoins.ToString();
        }
        if (worldComplete)
        {
            //Display coins
            levelCoins = coinText.levelCoins + bonusCoins; //Get level coins + extra coins
            coins = coinText.coins;
            coinTotal.GetComponent<Text>().text = "Coins: " + coinText.coins.ToString() + " / " + levelCoins.ToString();

            //Save (bonus coins)
            if (coins > gMScript.LoadJson().levelCoins[sceneBuildIndex] - bonusCoins) //Only save if coins are higher than ones stored 
            {
                gMScript.SaveJson(sceneBuildIndex, coins + bonusCoins, gMScript.LoadJson().levelDeaths[sceneBuildIndex], gMScript.LoadJson().localDeaths, true, -1, -1, -1);
            }
        }
    }

    //Add coins when world is completed (trigger)
    public void AddCoinTrigger()
    {
        gMScript.addCoinsDone = false;
        StartCoroutine(AddCoins());
    }

    //Mark animation as done
    public void AnimationDone()
    {
        gMScript.completeLevelDone = true;
        if (levelComplete) gMScript.addCoinsDone = true;
    }

    //Add coins when world is completed (function)
    IEnumerator AddCoins()
    {
        while (addedCoins < bonusCoins)
        {
            coins += 1;
            addedCoins += 1;
            coinTotal.GetComponent<Text>().text = "Coins: " + coins.ToString() + " / " + levelCoins.ToString();
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.1f);
        }
        if (addedCoins == bonusCoins) gMScript.addCoinsDone = true;
    }

}