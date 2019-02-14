using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    GameManagerScript gMScript;

    //Get game manager
    void Start() { gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); }

    //When colliding with something
    void OnCollisionEnter(Collision collisionInfo)
    {
        //Obstacle
        if (collisionInfo.collider.tag == "Obstacle")
        {
            movement.enabled = false; //Disable movement
            GetComponent<PracticeMode>().enabled = false; //Disable practice mode
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().EndGame(); //End game
        }
    }

    //When touching a trigger 
    void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Coin": //Coin
                {
                    //Destroy and add coin
                    GetComponent<AudioSource>().Play(); //Play coins sound
                    Destroy(other.gameObject); //Destroy coin
                    GameObject coinTxt = GameObject.Find("CoinText"); //Get coin text
                    if (coinTxt != null) coinTxt.GetComponent<CoinText>().AddCoin(); //If coin text is found add coin
                    break;
                }
            case "Obstacle": //Death trigger
                {
                    movement.enabled = false; //Disable movement
                    GetComponent<PracticeMode>().enabled = false; //Disable practice mode
                    FindObjectOfType<GameManagerScript>().EndGame(); //Restart Scene
                    break;
                }
            case "Movement Pad": //Change Movement
                {
                    Pad pad = other.GetComponent<Pad>(); //Get Pad component
                    float physics = 1f / PlayerPrefs.GetInt("Physics") * 100f; //Get fixed timestep
                    movement.forwardForce = pad.forwardForce / physics; //Change forward force
                    movement.sidewayForce = pad.sidewayForce; //Change sideway force
                    if (pad.resetZVelocity) movement.rb.velocity = new Vector3(movement.rb.velocity.x, movement.rb.velocity.y, 0); //Reset sideways velocity if choosen
                    break;
                }
            case "Reset Rotation": //Change Player Rotation
                {
                    Rigidbody rb = GetComponent<Rigidbody>();
                    //Freeze rotation
                    rb.freezeRotation = true;
                    transform.rotation = other.GetComponent<Pad>().playerRotation;
                    rb.freezeRotation = false;
                    break;
                }
            case "Grav Pad": //Change Gravity
                {
                    Physics.gravity = other.GetComponent<Pad>().gravity; //Reverse gravity for all objects
                    break;
                }
            case "Cam Pad": //Change Camera Offset
                {
                    Pad pad = other.GetComponent<Pad>(); //Get pad
                    FollowPlayer followPlayer = Camera.main.GetComponent<FollowPlayer>(); //Get follow player
                    followPlayer.offset = pad.offset; //Change camera offset
                    followPlayer.targetRotation = Quaternion.Euler(pad.cameraRotX, pad.cameraRotY, pad.cameraRotZ); //Set camera target rotation
                    followPlayer.rotate = pad.rotateCam; //Set camera to rotate
                    followPlayer.camForward = pad.camForward;
                    followPlayer.camRight = pad.camRight;
                    float lightX = 0f, lightY = 0f, lightZ = 0f;
                    if (pad.changeLightX) lightX = pad.offset.x;
                    if (pad.changeLightY) lightY = pad.offset.y;
                    if (pad.changeLightZ) lightZ = pad.offset.z;
                    gameObject.transform.GetChild(0).transform.localPosition = new Vector3(lightX, lightY, lightZ);
                    break;
                }
            case "Activate Objects Pad": //Activate / Deactivate objects
                {
                    ButtonScript buttonScript = other.GetComponent<ButtonScript>(); //Get button script
                    buttonScript.UIToggle();
                    break;
                }
        }
    }
}