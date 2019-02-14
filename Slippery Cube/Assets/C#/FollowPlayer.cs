using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour {

    Transform Player;
    PlayerMovement playerMovement;
    public Vector3 offset, camForward, camRight;
    public Quaternion targetRotation;
    public bool rotate = false, setForward = true, onlyFollowPlayer = false;
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
            if (!onlyFollowPlayer)
            {
                offset = GameObject.Find("Player").GetComponent<PlayerMovement>().camOffset; //Set camera offset from player
                //Set camera rotation
                transform.rotation = Quaternion.Euler(playerMovement.camRotX, playerMovement.camRotY, playerMovement.camRotZ);
                if (playerMovement.setDefaultForward) SetOldForward();
                //Save default directions
                camForward = transform.forward;
                camRight = transform.right;
            }
        }
    }

    //Set directions (from start)
    public void SetOldForward()
    {
        //Set directions
        playerMovement.camForward = camForward;
        playerMovement.camRight = camRight;
    }
    //Set directions (current)
    public void SetNewForward()
    {
        //Set directions
        playerMovement.camForward = transform.forward;
        playerMovement.camRight = transform.right;
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
        if (rotate && !onlyFollowPlayer)
        {   
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); //Smoothly rotate camera

            //Set directions
            playerMovement.camForward = camForward;
            playerMovement.camRight = camRight;
        }
    }
}
