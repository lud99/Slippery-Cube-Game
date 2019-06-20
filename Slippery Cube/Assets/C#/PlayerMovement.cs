using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    float ScreenWidth, hinput;
    public float forwardForce = 8000f, sidewayForce = 100f, camRotX = 0, camRotY = -90, camRotZ = 0, dieY = 0; //0.01 fixed timestep
    public bool setDefaultRotation = true, setDefaultForward = true, onlyRunStart = false;
    public Rigidbody rb;
    public Vector3 gravity, camOffset, camForward, camRight, originalPosition;
    Transform cam;
    FollowPlayer followPlayer;

    //Start
    void Start()
    {
        cam = Camera.main.GetComponent<Transform>(); //Get camera transform
        followPlayer = Camera.main.GetComponent<FollowPlayer>(); //Get follow player
        ScreenWidth = Screen.width; //Get screen width
        Physics.gravity = gravity; //Set gravity
        originalPosition = transform.position;
        if (setDefaultForward) followPlayer.SetOldForward(); //Set camera to find player and set default rotation
        followPlayer.rotate = false; //Don'r rotate on start

        //Change applied force depending on physics level
        //8000 ff, 100 sf = 0.02 (default)
        float physics = 1f / PlayerPrefs.GetInt("Physics") * 100f;
        forwardForce = 8000f / physics;
    }

    //Update
    void Update()
    {
        if (!onlyRunStart)
        {
            //Loop over every touch (mobile)
            int i = 0;
            while (i < Input.touchCount)
            {
                //If touching right side of screen
                if (Input.GetTouch(i).position.x > ScreenWidth / 2)
                {
                    hinput = 0.8f; //Move right
                }
                //if touching left side of screen
                if (Input.GetTouch(i).position.x < ScreenWidth / 2)
                {
                    hinput = -0.8f; //Move left
                }
                ++i;
            }
            if (Input.touchCount == 0) hinput = 0f; //Reset when not touching

            //Check if player is falling of ground
            if (rb.position.y < dieY)
            {
                FindObjectOfType<GameManagerScript>().EndGame();
                enabled = false;
            }
        }
    }

    //Update Physics
    void FixedUpdate()
    {
        if (!onlyRunStart)
        {
            //Add forward force in cameras front direction
            rb.AddForce(camForward * forwardForce * Time.deltaTime);
            //Add sideways force if button is down (normal horizontal axis)
            if (Input.GetAxis("Horizontal") != 0) rb.AddForce(Input.GetAxis("Horizontal") * camRight * sidewayForce * Time.deltaTime, ForceMode.VelocityChange);
            //Add sideways force if button is down (numpad horizontal axis)
            if (Input.GetAxis("Horizontal Numpad") != 0) rb.AddForce(Input.GetAxis("Horizontal Numpad") * camRight * sidewayForce * Time.deltaTime, ForceMode.VelocityChange);
            //Add sideways force (touch / mobile)
            if (hinput != 0) rb.AddForce(hinput * camRight * sidewayForce * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}