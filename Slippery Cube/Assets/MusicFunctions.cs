using UnityEngine;

public class MusicFunctions : MonoBehaviour
{
    Music music;

    //Start
    void Start()
    {
        music = GameObject.Find("Music").GetComponent<Music>();
    }

    //Set to continue playing or stop playing level music in menu
    public void LevelMusicInMenu(bool state)
    {
        music.playLevelMusicInMenu = state;
    }
}
