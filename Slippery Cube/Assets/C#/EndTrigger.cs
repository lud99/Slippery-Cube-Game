using UnityEngine;

public class EndTrigger : MonoBehaviour {

    bool triggerOnce = true;

    //Reaching the end of the level
    void OnTriggerEnter ()
    {
        if (triggerOnce)
        {
            triggerOnce = false;
            FindObjectOfType<GameManagerScript>().CompleteLevel();
        }
    }
}
