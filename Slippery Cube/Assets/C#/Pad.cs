using UnityEngine;

public class Pad : MonoBehaviour {

    public Vector3 gravity, offset, camForward, camRight; //Vector3's
    public Quaternion playerRotation; //Rotation
    public float forwardForce, sidewayForce = 100; //Player movement force
    public bool rotateCam = false, resetZVelocity = false, changeLightX, changeLightY, changeLightZ; //Rotate camera and reset sideways velocity
    public int cameraRotX, cameraRotY, cameraRotZ; //Camer rotation that is easily understandable
}
