using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    public GameObject obj;
    public float rotationSpeed = 50f;

    //Update
    void Update()
    {
        //Rotate around selected object
        transform.RotateAround(obj.transform.position, obj.transform.up, rotationSpeed * Time.deltaTime);
        //Look at selected object
        transform.LookAt(obj.transform.position, obj.transform.up);
    }
}
