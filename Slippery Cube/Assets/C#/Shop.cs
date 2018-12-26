using UnityEngine;

public class Shop : MonoBehaviour {

    GameObject gameManager;
    int spentCoins;
    public int[] itemList, itemCost;

	//Start
	void Start () {
        gameManager = GameObject.Find("GameManager");
	}
	
    //Buy function
    public void Buy(int itemID)
    {
        if (PlayerPrefs.GetInt("CurrentCoins") >= itemCost[itemID])
        {
            PlayerPrefs.SetInt("SpentCoins", PlayerPrefs.GetInt("SpentCoins") + itemCost[itemID]);
            gameManager.GetComponent<GameManagerScript>().CurrentCoins();
        } else
        {
            Debug.Log("Not enough coins! Collect more and come back later");
        }

    }
}
