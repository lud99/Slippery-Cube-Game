using UnityEngine;

public class DisableObjects : MonoBehaviour {

    GameObject player;
    MeshRenderer rend;
    ParticleSystem parSys;
    public bool disable, disableRb, disableShadows, disableLights, disableParticles;
    public int disableRbDistanceInfront = 50, disableRbDistanceBehind = 10, pDistanceInfront = 30;
    public Rigidbody rb;

    // Improve performance
    void Start () {

        //Get components / objects
        player = GameObject.Find("Player");
        rend = GetComponent<MeshRenderer>();
        parSys = GetComponent<ParticleSystem>();

        //Disable rigidbody on objects on start
        if (disableRb)
        {
            rb.detectCollisions = false;
            rb.isKinematic = true;
        }
        //Activate particles on start and disable if particles are disabled
        if (disableParticles && parSys != null)
        {
            parSys.Stop();
        }
        //Disable lights
        if (PlayerPrefs.GetInt("Lights") == 0 && disableLights)
        {
            gameObject.SetActive(false);
        }
        //Disable shadow casting if graphics are set to low
        if ((QualitySettings.GetQualityLevel() == 0 || QualitySettings.GetQualityLevel() == 1) && disableShadows)
        {
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        } else if (rend != null) rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    //Update
    void Update()
    {
        if (disableRb)
        {
            //Disable rigidbody on objects that are in front of player
            if (transform.position.x > player.transform.position.x - disableRbDistanceInfront) //Infront of Player
            {
                //Activate Rigidbody
                rb.detectCollisions = true;
                rb.isKinematic = false;
            }
            if (transform.position.x > player.transform.position.x + disableRbDistanceBehind) //Behind player
            {
                //Disable rigidbody
                rb.detectCollisions = false;
                rb.isKinematic = true;
            }
        }
        if (disableParticles)
        {
            //Disable particles on objects that are in front of player
            if (transform.position.x > player.transform.position.x - pDistanceInfront) //Infront of Player
            {
                //Play particles
                if (!parSys.isPlaying) parSys.Play();
            }
            if (transform.position.x > player.transform.position.x + 10) //Behind player
            {
                //Stop particles
                if (parSys.isPlaying) parSys.Stop();
            }
        }
    }
}
