using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public bool up, forward;
    Vector3 direction;

    //Start
    void Start()
    {
        if (up) direction = transform.up; else direction = transform.forward;
    }

    //Update
    void Update()
    {
        if (QualitySettings.GetQualityLevel() >= 1) transform.RotateAround(transform.position, direction, rotationSpeed * Time.deltaTime);
    }
}
