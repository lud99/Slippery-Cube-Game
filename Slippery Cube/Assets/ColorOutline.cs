using UnityEngine;

public class ColorOutline : MonoBehaviour
{
    public GameObject color;

    //Update
    void Update()
    {
        transform.position = color.transform.position;
    }
}
