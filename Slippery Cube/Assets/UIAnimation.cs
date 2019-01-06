using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public GameObject viewport;
    public GameObject[] levels;
    Animator anim;

    //Jump 
    public void UIJump()
    {
        anim = GetComponent<Animator>(); //Get animator
        anim.enabled = true; //Enable animator
        anim.ResetTrigger("UIJumpBack"); //Reset trigger
        anim.SetTrigger("UIJump"); //Set trigger
    }

    //Jump Back
    public void UIJumpBack()
    {
        anim = GetComponent<Animator>(); //Get animator
        anim.ResetTrigger("UIJump"); //Reset trigger
        anim.SetTrigger("UIJumpBack"); //Set trigger
    }

    //Reset scale to 1
    public void UIJumpDefault()
    {
        anim = GetComponent<Animator>(); //Get animator
        anim.ResetTrigger("UIJump"); //Reset trigger
        anim.ResetTrigger("UIJumpBack"); //Reset trigger
        anim.SetTrigger("UIJumpDefault"); //Set trigger
    }

    //Slide from world to levels
    public void WorldToLevels(int world)
    {
        anim = viewport.GetComponent<Animator>(); //Get animator
        GameManagerScript gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get game manager

        //Update level objects to selected world
        for (int i = world * 10; i < gMScript.LoadJson().levelCoins.Length + (world * 10); i++)
        {
            //Do stuff
            Debug.Log(i);
            /*gMScript.LoadJson().levelCoins[i]; //Get coins
            gMScript.LoadJson().levelDeaths[i]; //Get deaths*/
        }
        anim.ResetTrigger("LevelsToWorld"); //Reset trigger
        anim.SetTrigger("WorldToLevels"); //Set trigger
    }

    //Slide from levels to world
    public void LevelsToWorld(int world)
    {
        anim = viewport.GetComponent<Animator>(); //Get animator
        anim.ResetTrigger("World1ToLevels"); //Reset trigger
        anim.SetTrigger("LevelsToWorld1"); //Set trigger
    }
}
