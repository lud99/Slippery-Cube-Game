using UnityEngine;

public class DeathText : MonoBehaviour
{
    //Start
    void Start()
    {
        if (PlayerPrefs.GetInt("ShowDeaths") == 0) this.gameObject.SetActive(false); else this.gameObject.SetActive(true);
    }
}
