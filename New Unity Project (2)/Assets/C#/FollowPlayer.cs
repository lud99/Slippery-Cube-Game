using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    Transform Player;
    public Vector3 offset;

    //Get player
    public void Start()
    {
        if (GameObject.Find("Player") != null) Player = GameObject.Find("Player").transform;
    }

    //Follow players position
    void Update ()
    {
        if (Player != null) transform.position = Player.position + offset;
	}
}
