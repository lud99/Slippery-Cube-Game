using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public PlayerMovement movement;
    GameObject player;
    Rigidbody player_rb;

    //When colliding with something
    void OnCollisionEnter(Collision collisionInfo)
    {
        //Obstacle
        if (collisionInfo.collider.tag == "Obstacle")
        {
            movement.enabled = false;
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().EndGame();
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
                    GetComponent<AudioSource>().Play();
                    Destroy(other.gameObject);
                    GameObject coinTxt = GameObject.Find("CoinText");
                    if (coinTxt != null) coinTxt.GetComponent<CoinText>().addCoin();
                    break;
                }
            case "Obstacle": //Death trigger
                {
                    movement.enabled = false;
                    GetComponent<PracticeMode>().enabled = false;
                    FindObjectOfType<GameManagerScript>().EndGame(); //Restart Scene
                    break;
                }
            case "Normal Pad": //Normal Pad
                {
                    //Normal movement
                    float physics = 1f / PlayerPrefs.GetInt("Physics") * 100f;
                    movement.forwardForce = 8000f / physics;
                    movement.sidewayForce = 100f;
                    break;
                }
            case "Stop Pad": //Stop Pad
                {
                    //Stop movement
                    movement.forwardForce = 0f;
                    movement.sidewayForce = 0f;
                    break;
                }
            case "Reset Rotation": //Reset Player Rotation
                {
                    player = GameObject.Find("Player");
                    player_rb = player.GetComponent<Rigidbody>();
                    //Freeze rotation
                    player_rb.freezeRotation = true;
                    player.transform.rotation = Quaternion.identity;
                    player_rb.freezeRotation = false;
                    break;
                }
            case "Grav Pad": //Change Gravity Pad
                {
                    Physics.gravity = other.GetComponent<PadGravity>().gravity; //Reverse gravity for all objects
                    break;
                }
            case "Cam Pad":
                {
                    Camera.main.GetComponent<FollowPlayer>().offset = other.GetComponent<PadGravity>().offset; //Change camera offset
                    break;
                }
        }
    }
}