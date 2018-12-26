using UnityEngine;

public class EndTrigger : MonoBehaviour {

    //Reaching the end of the level
    void OnTriggerEnter ()
    {
        FindObjectOfType<GameManagerScript>().CompleteLevel();
    }
}
