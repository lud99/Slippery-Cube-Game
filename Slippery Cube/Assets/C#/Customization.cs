using UnityEngine;

public class Customization : MonoBehaviour
{
    public Material[] playerMats;
    public GameObject[] colorImages;
    GameObject player;
    GameManagerScript gMScript;
    Renderer playerRenderer;
    PlayerMovement playerMovement;
    FollowPlayer followPlayer;
    Transform playerLight;

    ////This component should be on the game manager

    //Start
    void Start()
    {
        gMScript = GetComponent<GameManagerScript>(); //Get game manager
        player = GameObject.Find("Player"); //Get player
        //Only execute if player exists
        if (player != null)
        {
            playerLight = player.transform.GetChild(0); //Get player light
            playerRenderer = player.GetComponent<Renderer>(); //Get player renderer
            playerMovement = player.GetComponent<PlayerMovement>(); //Get player movement
            followPlayer = Camera.main.GetComponent<FollowPlayer>(); //Get follow player
            ChangePlayerMaterial(gMScript.LoadJson().currentOutfit); //Update player material with saved outfit
        }
    }

    //Change player material
    public void ChangePlayerMaterial(int materialIndex)
    {
        //Only execute if player exists
        if (playerRenderer != null)
        {
            Animator anim = player.GetComponent<Animator>(); //Get animator

            //Reset animations
            anim.Rebind();

            playerRenderer.material = playerMats[materialIndex]; //Set player's material to selected one from array
            gMScript.SaveJsonOutfits(0, materialIndex); //Save selected outfit

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
                anim.SetTrigger("Rainbow"); //Play rainbow animation
                if (colorImages[0] != null) //If on customization scene
                {
                    colorImages[0].GetComponent<Animator>().Rebind(); //Reset animation
                    colorImages[0].GetComponent<Animator>().SetTrigger("Rainbow2D"); //Play rainbow animation on color image
                }
            }

            //Update player light whith new material color
            playerLight.GetComponent<PlayerLight>().UpdateColor();
        }
    }

    //Test customization
    public void TestCustomization()
    {
        //Disable rotation around cube
        Camera.main.GetComponent<RotateAroundObject>().enabled = false;
        //Enable standard camera
        followPlayer.enabled = true;
        followPlayer.targetRotation = Quaternion.Euler(0, -90, 0);
        followPlayer.rotate = true;
        //Enable player movement
        playerMovement.enabled = true;
    }
}
