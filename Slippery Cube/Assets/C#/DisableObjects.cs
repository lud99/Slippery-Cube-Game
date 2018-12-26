using UnityEngine;

public class DisableObjects : MonoBehaviour {

    GameObject player;
    MeshRenderer rend;
    public bool disable, disableRb, disableShadows, disableLights, disableParticles;
    public Rigidbody rb;

    // Improve performance
    void Start () {

        //Get components / objects
        player = GameObject.Find("Player");
        rend = GetComponent<MeshRenderer>();

        //Disable rigidbody on objects on start
        if (disableRb)
        {
            rb.detectCollisions = false;
            rb.isKinematic = true;
        }
        //Disable particles
        if (PlayerPrefs.GetInt("Particles") == 0 && disableParticles)
        {
            if (GetComponent<ParticleSystem>() != null) GetComponent<ParticleSystem>().Stop();
        }
        //Disable lights
        if (PlayerPrefs.GetInt("Lights") == 0 && disableLights)
        {
            this.gameObject.SetActive(false);
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
            //Disable components on objects that are in front of player visible
            if (transform.position.x > player.transform.position.x - 50) //Infront of Player
            {
                rb.detectCollisions = true;
                rb.isKinematic = false;
            }
            if (transform.position.x > player.transform.position.x + 10) //Behind player
            {
                rb.detectCollisions = false;
                rb.isKinematic = true;
            }
        }
    }
}
