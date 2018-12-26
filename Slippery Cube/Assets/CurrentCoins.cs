using UnityEngine;
using UnityEngine.UI;

public class CurrentCoins : MonoBehaviour {

    Text text;
    GameManagerScript gMScript;

	//Start
	void Start () {
        Text text = GetComponent<Text>();
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        text.text = gMScript.LoadJson().currentCoins.ToString();
	}
}
