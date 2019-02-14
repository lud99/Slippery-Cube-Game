using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public GameObject fill, player;
    Slider slider;
    float playerStart, playerCurrent, levelEnd;

    //Initialize variables
    public void Start()
    {
        if (PlayerPrefs.GetInt("ShowProgressBar") == 0) this.gameObject.SetActive(false);
        if (player == null) player = GameObject.Find("Player");
        playerStart = player.transform.position.x;
        levelEnd = GameObject.Find("END Stop").transform.position.x;
        slider = GetComponent<Slider>();
        //UpdateColor(); //Update progress bar color
    }

    //Update score text
    void Update () {
        playerCurrent = player.transform.position.x;
        slider.value = -(playerCurrent - playerStart) / (playerStart - levelEnd);
	}

    //Update color to match player color
    public void UpdateColor()
    {
        player = GameObject.Find("Player");
        Material playerMat = player.GetComponent<Renderer>().material;
        fill.GetComponent<Animator>().enabled = false;
        fill.GetComponent<Image>().color = new Color(playerMat.color.r, playerMat.color.g, playerMat.color.b, 1f);
    }
}
 