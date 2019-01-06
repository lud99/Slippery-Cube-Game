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
        for (int i = 0; i < 10; i++) { //Loop through level objects
            levels[i].SetActive(true); //Activate all levels objects
            levels[i].GetComponent<LevelUI>().level = i + 1 +((world - 1) * 10); //Set level to i + the selected world (basically)
            levels[i].GetComponent<LevelUI>().Start(); //Update UI and deactivate if level isn't unlocked
        }
        anim.ResetTrigger("LevelsToWorld"); //Reset trigger
        anim.SetTrigger("WorldToLevels"); //Set trigger
    }

    //Slide from levels to world
    public void LevelsToWorld(int world)
    {
        anim = viewport.GetComponent<Animator>(); //Get animator
        anim.ResetTrigger("WorldToLevels"); //Reset trigger
        anim.SetTrigger("LevelsToWorld"); //Set trigger
    }
}
