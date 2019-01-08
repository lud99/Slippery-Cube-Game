using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    //Update
    void Update()
    {
        if (QualitySettings.GetQualityLevel() >= 1) transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
    }
}
