using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour {

    Transform Player;
    PlayerMovement playerMovement;
    public Vector3 offset, camForward, camRight;
    public Quaternion targetRotation;
    public bool rotate = false, setForward = true;
    public float rotationSpeed = 10f;

    //Get player
    public void Awake()
    {
        //Setup
        string scene = SceneManager.GetActiveScene().name;
        if (GameObject.Find("Player")) //If player exists
        {
            Player = GameObject.Find("Player").transform;
            playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
            //Follow player
            offset = GameObject.Find("Player").GetComponent<PlayerMovement>().camOffset; //Set camera offset
            //Set camera rotation
            transform.rotation = Quaternion.Euler(playerMovement.camRotX, playerMovement.camRotY, 0);
        }

        //Set camera rotation
        rotate = false;
        camForward = transform.forward;
        camRight = transform.right;
    }

    //Follow players position
    void Update ()
    {
        //Update position to follow player
        if (Player != null)
        {
            transform.position = Player.position + offset;
        }
        
        //Rotate
        if (rotate)
        {
            if (setForward) //Set forward and right directions
            {
                playerMovement.camForward = camForward;
                playerMovement.camRight = camRight;
                setForward = false; //only execute once
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); //Smoothly rotate camera
            if (targetRotation.y < -0.7f && transform.rotation.y < -90f) //Set 'rotate' to false if finished
            {
                rotate = false;
                transform.rotation = Quaternion.Euler(0, 90, 0);
                //playerMovement.enabled = true;
            }
            if (targetRotation.y >= -0.7f && transform.rotation.y > targetRotation.y) //Set 'rotate' to false if finished 
            {
                rotate = false;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                //playerMovement.enabled = true;
            }
        }
    }
}
