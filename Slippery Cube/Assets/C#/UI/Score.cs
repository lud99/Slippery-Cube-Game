using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    GameObject player;
    Slider slider;
    float playerStart, playerCurrent, levelEnd;

    //Initialize variables
    void Start()
    {
        if (PlayerPrefs.GetInt("ShowProgressBar") == 0) this.gameObject.SetActive(false);
        player = GameObject.Find("Player");
        playerStart = player.transform.position.x;
        levelEnd = GameObject.Find("END Stop").transform.position.x;
        slider = GetComponent<Slider>();
    }

    //Update score text
    void Update () {
        playerCurrent = player.transform.position.x;
        slider.value = -(playerCurrent - playerStart) / (playerStart - levelEnd);
	}
}
 