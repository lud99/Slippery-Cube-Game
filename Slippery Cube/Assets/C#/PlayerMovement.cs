using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    float heading = -90f, ScreenWidth, hinput;
    public float forwardForce = 8000f, sidewayForce = 100f; //0.01 fixed timestep
    public Rigidbody rb;
    public Vector3 gravity, camOffset;
    Transform cam;

    //Start
    void Start()
    {
        cam = Camera.main.GetComponent<Transform>();
        ScreenWidth = Screen.width;
        Physics.gravity = gravity;
        Camera.main.GetComponent<FollowPlayer>().Start(); //Set camera to find player
        Camera.main.GetComponent<FollowPlayer>().offset = camOffset; //Set camera offset

        //Chane applied force depending on physics level
        //8000 ff, 100 sf = 0.02 (default)
        float physics = 1f / PlayerPrefs.GetInt("Physics") * 100f;
        forwardForce = 8000f / physics;
    }

    //Update
    void Update()
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
        //Set camera rotation
        cam.rotation = Quaternion.Euler(0, heading, 0);

        //Check if player is falling of ground
        if (rb.position.y < 0f)
        {
            FindObjectOfType<GameManagerScript>().EndGame();
            this.enabled = false;
        }
    }

    //Update Physics
    void FixedUpdate()
    {
        rb.AddForce(cam.transform.forward * forwardForce * Time.deltaTime);  //Add forward force in cameras front direction
        if (Input.GetAxis("Horizontal") != 0) rb.AddForce(Input.GetAxis("Horizontal") * cam.transform.right * sidewayForce * Time.deltaTime, ForceMode.VelocityChange); //Add sideways force if button is down
        if (hinput != 0) rb.AddForce(hinput * cam.transform.right * sidewayForce * Time.deltaTime, ForceMode.VelocityChange); //Add sideways force (touch / mobile)
    }
}