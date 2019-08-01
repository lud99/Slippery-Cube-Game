using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public GameObject fill, player;
    public float endX;
    public bool flip = false;
    Slider slider;
    float playerStart, playerCurrent, levelEnd, minOffset;

    //Initialize variables
    public void Start()
    {
        if (PlayerPrefs.GetInt("ShowProgressBar") == 0) gameObject.SetActive(false);
        if (player == null) player = GameObject.Find("Player");
        playerStart = player.transform.position.x;
        levelEnd = GameObject.Find("END Stop").transform.position.x;
        if (endX == 0f) endX = levelEnd;
        //minOffset = Mathf.Abs(endX);
        minOffset = Mathf.Abs(playerStart);

        slider = GetComponent<Slider>();

        slider.minValue = 0;
        slider.maxValue = endX != levelEnd ? Mathf.Abs(endX) + minOffset : playerStart + minOffset;
    }

    //Update progress bar value
    void Update () {
        playerCurrent = CurrentPlayerX();

        if (flip)
        {
            minOffset = Mathf.Abs(playerStart);

            //slider.maxValue = endX != levelEnd ? Mathf.Abs(endX) + minOffset : playerStart + minOffset;
        }
        slider.value = !flip ? playerStart - playerCurrent : playerCurrent;
        //slider.value = negative ? -1 : 1 * (playerCurrent - playerStart) / (playerStart - levelEnd);
    }

    //Update color to match player color
    public void UpdateColor()
    {
        player = GameObject.Find("Player");
        Material playerMat = player.GetComponent<Renderer>().material;
        fill.GetComponent<Animator>().enabled = false;
        fill.GetComponent<Image>().color = new Color(playerMat.color.r, playerMat.color.g, playerMat.color.b, 1f);
    }

    //Flip
    public void Flip()
    {
        playerStart = CurrentPlayerX();

        slider.maxValue = /*endX != levelEnd ? */Mathf.Abs(endX);// + minOffset; playerStart + minOffset;
        flip = !flip;
    }

    //Flip (new endX)
    public void Flip(float newEndX)
    {
        endX = newEndX;
        playerStart = CurrentPlayerX();

        slider.maxValue = endX != levelEnd ? Mathf.Abs(endX) + minOffset : playerStart + minOffset;
        flip = !flip;
    }

    //Set endX to the specified position
    public void SetEnd(float end)
    {
        endX = end;
    }

    //Set endX to the Level Stop object's x
    public void SetEndToLevelEnd()
    {
        endX = levelEnd;
    }

    //Get player's current x position
    float CurrentPlayerX()
    {
        return player.transform.position.x;
    }
}
 