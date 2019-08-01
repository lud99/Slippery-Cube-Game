using UnityEngine;

public class CheckpointUI : MonoBehaviour
{
    PracticeMode practice;

    //Start
    void Start()
    {
        GameObject practiceObject = GameObject.FindGameObjectWithTag("PracticeMode");

        if (practiceObject != null)
            practice = practiceObject.GetComponent<PracticeMode>();
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
