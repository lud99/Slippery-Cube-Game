using UnityEngine;

public class CheckpointUI : MonoBehaviour
{
    PracticeMode practice;

    //Start
    void Start()
    {
        practice = GameObject.FindGameObjectWithTag("PracticeMode").GetComponent<PracticeMode>();
    }

    //Save checkpoint
    public void SaveCheckpoint()
    {
        practice.SaveCheckpoint();
    }

    //Remove checkpoint
    public void RemoveCheckpoint()
    {
        practice.DeleteCheckpoint();
    }
}
