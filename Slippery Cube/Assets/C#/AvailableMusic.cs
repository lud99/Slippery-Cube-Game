using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableMusic : MonoBehaviour {

    List<string> defaultOptions = new List<string> { "Random", "Basics" };
    List<string> world2 = new List<string> { "Gravity" };
    Dropdown dropdown;
    GameManagerScript gMScript;
    GameObject musicObject;
    public Music music;

	//Start
	void Start () {
        musicObject = GameObject.Find("Music"); //Find music object
        dropdown = GetComponent<Dropdown>(); //Get dropdown
        music = musicObject.GetComponent<Music>(); //Get music script
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get game manager

        dropdown.ClearOptions(); //Clear all dropdown options

        //Add default options (Random and Basics)
        dropdown.AddOptions(defaultOptions);

        //Only show music from worlds when they are completed

        //If world 2 is completed
        if (gMScript.LoadJson().levelDone[11]) 
        {
            dropdown.AddOptions(world2); //Add 'Gravity'
        }

        dropdown.value = PlayerPrefs.GetInt("MenuMusic"); //Update default selected option to stored value
    }

    //Trigger Music object's SelectMenuMusic function. When switching scenes the dropdown forgets the music object
    public void ChangeMusicTrigger(int value)         //but it can't forget itself so it triggers an internal function                                             
    {                                                 //instead which triggers the music object's funtion
        if (PlayerPrefs.GetInt("MenuMusic") != value) music.SelectMenuMusic(value); //Select music function
    }
}
