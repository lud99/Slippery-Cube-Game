using UnityEngine;
using UnityEngine.UI;

public class DeathText : MonoBehaviour
{
    public GameObject deathText;

    //Start
    void Start()
    {
        if (PlayerPrefs.GetInt("ShowDeaths") == 0) gameObject.SetActive(false); else gameObject.SetActive(true);
    }

    //Update UI
    void Update()
    {
        deathText.GetComponent<Text>().text = GameObject.Find("GameManager").GetComponent<GameManagerScript>().LoadJson().localDeaths.ToString(); //Display local deaths
    }
}
