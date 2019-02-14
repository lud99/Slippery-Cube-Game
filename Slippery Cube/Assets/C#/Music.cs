using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    //string[] menuScenes;
    public string[] world1, world2;
    public bool playLevelMusicInMenu;
    public AudioClip[] music;
    public GameObject dropdown;
    AudioSource audioSource;
    GameManagerScript gMScript;

    //Start
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //Get audio source component (audio player)
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        DontDestroyOnLoad(this); //Be active in all scenes

        if (GameObject.FindGameObjectsWithTag("Music").Length > 1) Destroy(gameObject);
    }

    //Choose music
    public void SelectMenuMusic(int menuMusic)
    {
        if (dropdown == null) dropdown = GameObject.Find("MenuMusicDropdown");

        //Stop current music
        //audioSource.Stop();
        if (menuMusic == 0) //If random music is selected
        {
            audioSource.clip = music[RandomMusic()]; //Generate random number & Play choosen song
        }
        else //If specific music is selected
        {
            audioSource.clip = music[menuMusic - 1]; //Subtract 1 from 'menuMusic' to prevent non existant music from playing since array is 1 less than dropdown values
        }

        PlayerPrefs.SetInt("MenuMusic", menuMusic); //Save prefered music
        if (dropdown != null) dropdown.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("MenuMusic"); //Set value to match stored value 

        //If prefered music is not unlocked
        if (audioSource.clip == music[1] && !gMScript.LoadJson().levelDone[11]) //Lock "Gravity" song
        {
            PlayerPrefs.SetInt("MenuMusic", 0); //Reset music to random
            if (dropdown != null) dropdown.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("MenuMusic"); //Set dropdownvalue to match value (random)

            //Pick random music again
            audioSource.clip = music[RandomMusic()]; //Choose song
        }
    }

    //Pick random music function (Generate random number)
    int RandomMusic()
    {
        var random = 0; //Setup variable
        if (!gMScript.LoadJson().levelDone[11]) random = Random.Range(0, music.Length - 1); //Generate number if world 1 is not completed (only include 'Basics')
        if (gMScript.LoadJson().levelDone[11]) random = Random.Range(0, music.Length); //Generate number if world 2 is completed (Include 'Basics' and 'Gravity')
        return random; //Return value so it is accesable
    }

    //Play music
    void Update()
    {
        string scene = SceneManager.GetActiveScene().name; //Get scene name
        //Play the right track for each scene / world
        for (int i = 0; i < world1.Length; i++) //World 1
        {
            if (scene == world1[i]) //If in world 1
            {
                if (!audioSource.isPlaying || audioSource.clip != music[0]) //Only play music when no music is played (no music stacking) and stop other tracks that are not world 1
                {
                    audioSource.Stop();
                    audioSource.clip = music[0];
                    audioSource.Play();
                }
            }
            if (scene == world2[i]) //If in world 2
            {
                if (!audioSource.isPlaying || audioSource.clip != music[1]) //Only play music when no music is played (no music stacking) and stop other tracks that are not world 2
                {
                    audioSource.Stop();
                    audioSource.clip = music[1];
                    audioSource.Play();
                }
            }
            if (scene == "TitleScreen" || scene == "LevelSelect" || scene == "Customization") //If on title screen or level select
            {
                if (!audioSource.isPlaying || audioSource.clip != music[2] && !playLevelMusicInMenu) //Only play music when no music is played (no music stacking) and stop other tracks that are not menu loop
                {
                    /*changeMusic = false;
                    SelectMenuMusic(PlayerPrefs.GetInt("MenuMusic")); //Select menu music from stored preference*/
                    audioSource.clip = music[2]; //Select menu loop music
                    audioSource.Play(); //Play
                }
            }
        }
    }
}
