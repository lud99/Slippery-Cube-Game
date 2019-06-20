using UnityEngine;

public class LevelEdge : MonoBehaviour
{
    public bool active;

    //Start
    void Start()
    {
        gameObject.SetActive(active);
    }
}
