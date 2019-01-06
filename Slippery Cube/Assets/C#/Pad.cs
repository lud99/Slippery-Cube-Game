using UnityEngine;

public class Pad : MonoBehaviour {

    public Vector3 gravity, offset, camForward, camRight; //Vector3's
    public Quaternion playerRotation, cameraRotation; //Rotation
    public float forwardForce, sidewayForce = 100; //Player movement force
    public bool rotateCam = false; //Rotate camera
}
