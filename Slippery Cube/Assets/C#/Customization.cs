using UnityEngine;

public class Customization : MonoBehaviour
{
    public Material[] playerMats;
    public GameObject[] colorImages;
    public GameObject progressBar, fill;
    GameObject player;
    GameManagerScript gMScript;
    Renderer playerRenderer;
    ParticleSystem trail;
    ParticleSystemRenderer trailRender;
    PlayerMovement playerMovement;
    FollowPlayer followPlayer;
    Transform playerLight;
    Animator anim, trailAnim;

    //Start
    public void Awake()
    {
        gMScript = GetComponent<GameManagerScript>(); //Get game manager
        player = GameObject.Find("Player"); //Get player

        //Only execute if player exists
        if (player != null)
        {
            playerLight = player.transform.GetChild(0); //Get player light
            playerRenderer = player.GetComponent<Renderer>(); //Get player renderer
            trail = GameObject.Find("Trail").GetComponent<ParticleSystem>(); //Get trail particle system
            trailRender = trail.GetComponent<ParticleSystemRenderer>(); //Get trail particle system
            anim = player.GetComponent<Animator>(); //Get animator
            trailAnim = trail.GetComponent<Animator>(); //Get trail animator
            playerMovement = player.GetComponent<PlayerMovement>(); //Get player movement
            followPlayer = Camera.main.GetComponent<FollowPlayer>(); //Get follow player
            ChangePlayerMaterial(gMScript.LoadJson().currentOutfit); //Update player material with saved outfit
            ChangeParticleMaterial(gMScript.LoadJson().trailMaterial); //Update trail material with saved
            ChangeParticleSize(gMScript.LoadJson().trailSize); //Update trail size with saved
        }
    }

    //Change player material
    public void ChangePlayerMaterial(int materialIndex)
    {
        //Only execute if player exists
        if (playerRenderer != null)
        {
            //Reset animations
            anim.Rebind();

            playerRenderer.material = playerMats[materialIndex]; //Set player's material to selected one from array
            gMScript.SaveJsonOutfits(materialIndex, materialIndex, true, true); //Save selected outfit

            //'Restart' Player to update material
            anim.enabled = false;
            player.SetActive(false);
            player.SetActive(true);

            //Play color images animations
            if (colorImages[0] != null) colorImages[0].GetComponent<Animator>().SetTrigger("Rainbow2D"); //Play rainbow animation on color image

            //Play animations depending on materialIndex
            if (materialIndex == 27)
            {
                anim.enabled = true; //Enable animator
                anim.SetTrigger("Rainbow"); //Play rainbow animation on player

                //Play rainbow animation on progress bar
                if (fill == null) fill = GameObject.Find("Fill"); //Find progress bar if it's not assigned in inspector

                fill.GetComponent<Animator>().enabled = true; //Enabled animator
                fill.GetComponent<Animator>().SetTrigger("Rainbow2D"); //If assigned in inspector

                if (colorImages[0] != null) //If on customization scene
                {
                    colorImages[0].GetComponent<Animator>().Rebind(); //Reset animation
                    colorImages[0].GetComponent<Animator>().SetTrigger("Rainbow2D"); //Play rainbow animation on color image
                }
            } else
            {
                //Set progress bar to player color
                if (progressBar == null) progressBar = GameObject.Find("ProgressBar"); //Find progress bar if it's not assigned in inspector
                progressBar.GetComponent<Score>().UpdateColor();
            }

            //Update player light whith new material color
            playerLight.GetComponent<PlayerLight>().UpdateColor();
        }
    }

    //Change particle material
    public void ChangeParticleMaterial(int materialIndex)
    {
        //Only execute if player trail exists
        if (trailRender != null)
        {
            trailRender.material = playerMats[materialIndex]; //Set player's material to selected one from array
            gMScript.SaveJsonTrail(materialIndex, materialIndex, -1f, true, true); //Save selected outfit

            //'Restart' Trail to update material
            //Reset animations
            if (trailAnim != null)
            {
                trailAnim.Rebind();
                trailAnim.enabled = false;
            }

            trail.gameObject.SetActive(false);
            trail.gameObject.SetActive(true);

            //Play rainbow animation
            if (colorImages[0] != null) colorImages[1].GetComponent<Animator>().SetTrigger("Rainbow2D");

            //If rainbow material
            if (materialIndex == 27)
            {
                trailAnim.enabled = true; //Enable trail animator
                trailAnim.SetTrigger("TrailRainbow"); //Play rainbow animation on trail

                if (colorImages[1] != null) //If on customization scene
                {
                    colorImages[1].GetComponent<Animator>().Rebind(); //Reset animation
                    colorImages[1].GetComponent<Animator>().SetTrigger("Rainbow2D");
                }
            }
        }
    }
    //Change particle size
    public void ChangeParticleSize(float size)
    {
        //Only execute if player trail exists
        if (trail != null)
        {
            var main = trail.main; //Main component of particle system
            main.startSize = size; //Set particle size to 'size'
            gMScript.SaveJsonTrail(0, -1, size, true, true); //Save selected outfit
        }
    }

    //Test customization
    public void TestCustomization()
    {
        //Toggle UI
        GameObject.Find("Canvas").GetComponent<ButtonScript>().UIToggle(); //Toggle UI
        progressBar.SetActive(true); //Activate progressBar
        progressBar.GetComponent<Score>().UpdateColor(); //Update progressbar color
        //Change player material to saved
        ChangePlayerMaterial(gMScript.LoadJson().currentOutfit);
        //Disable rotation around cube
        Camera.main.GetComponent<RotateAroundObject>().enabled = false;
        //Enable standard camera
        followPlayer = Camera.main.GetComponent<FollowPlayer>(); //Get follow player
        followPlayer.enabled = true;
        followPlayer.targetRotation = Quaternion.Euler(0, -90, 0);
        followPlayer.rotate = true;
        //Enable player movement
        playerMovement.onlyRunStart = false;
    }
}
