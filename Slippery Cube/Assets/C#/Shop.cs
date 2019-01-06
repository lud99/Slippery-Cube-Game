using UnityEngine;

public class Shop : MonoBehaviour {

    GameManagerScript gMScript;
    int spentCoins;
    public string[] itemName;
    public int[] itemCost;

	//Start
	void Start () {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
	}
	
    //Buy function
    public void Buy(int itemID)
    {
        if (gMScript.LoadJson().currentCoins >= itemCost[itemID])
        {
            gMScript.SaveJson(0, -1, -1, -1, gMScript.LoadJson().levelDone[0], -1, gMScript.LoadJson().spentCoins + itemCost[itemID], - 1);
            gMScript.CurrentCoins();
            Debug.Log("Succesfully bought " + itemName[itemID] + " for " + itemCost[itemID] + " coins");
        } else
        {
            Debug.Log("Not enough coins! Collect more and come back later");
        }

    }
}
