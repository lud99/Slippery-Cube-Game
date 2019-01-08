using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public GameObject animObject, backButton, lockPar;
    public GameObject[] levels;
    Animator anim, backButtonAnim;
    GameManagerScript gMScript;

    //Get components
    void Start()
    {
        anim = GetComponent<Animator>(); //Get animator
        if (backButton != null) backButtonAnim = backButton.GetComponent<Animator>();
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get game manager
    }

    //Jump 
    public void UIJump(bool self)
    {
        //If no levels is currently loading
        if (gMScript.asyncLoading == null)
        {
            if (!self) anim = animObject.GetComponent<Animator>(); else anim = GetComponent<Animator>(); //Set to run animation on selected object if specified
            anim.ResetTrigger("UIJumpBack"); //Reset trigger
            anim.ResetTrigger("UIJumpDefault"); //Reset trigger
            anim.ResetTrigger("UIJumpLocked"); //Reset trigger
            anim.SetTrigger("UIJump"); //Set trigger
        }
    }

    //Jump Back
    public void UIJumpBack(bool self)
    {
        //If no levels is currently loading
        if (gMScript.asyncLoading == null)
        {
            if (!self) anim = animObject.GetComponent<Animator>(); else anim = GetComponent<Animator>(); //Set to run animation on selected object if specified
            anim.ResetTrigger("UIJump"); //Reset trigger
            anim.ResetTrigger("UIJumpDefault"); //Reset trigger
            anim.ResetTrigger("UIJumpLocked"); //Reset trigger
            anim.ResetTrigger("UIJumpBack"); //Reset trigger
             anim.SetTrigger("UIJumpBack"); //Set trigger
        }
    }

    //Reset scale to 1
    public void UIJumpDefault(bool obj)
    {
        anim = GetComponent<Animator>(); //Set to run animation on selected object if specified
        anim.ResetTrigger("UIJump"); //Reset trigger
        anim.ResetTrigger("UIJumpDefault"); //Reset trigger
        anim.ResetTrigger("UIJumpLocked"); //Reset trigger
        anim.ResetTrigger("UIJumpBack"); //Reset trigger

        //If level/world is locked
        if (lockPar != null && lockPar.activeSelf)
        {
            if (obj) anim.SetTrigger("UIJumpLocked"); else anim.SetTrigger("UIJumpLockedLevel"); //Set trigger
        } else anim.SetTrigger("UIJumpDefault"); //Set trigger
    }

    //If level/world is locked
    public void UIJumpLocked(bool obj)
    {
        //If no levels is currently loading
        if (gMScript.asyncLoading == null)
        {
            anim = GetComponent<Animator>(); //Get animator
            anim.ResetTrigger("UIJump"); //Reset trigger
            anim.ResetTrigger("UIJumpBack"); //Reset trigger
            anim.ResetTrigger("UIJumpDefault"); //Reset trigger
            anim.ResetTrigger("UIJumpLocked"); //Reset trigger
            anim.ResetTrigger("UIJumpLockedLevel"); //Reset trigger
            if (obj) anim.SetTrigger("UIJumpLocked"); else anim.SetTrigger("UIJumpLockedLevel"); //Set trigger
        }
    }

    //Slide from world to levels
    public void WorldToLevels(int world)
    {
        //If no levels is currently loading and world is unlocked
        if (gMScript.asyncLoading == null && lockPar != null && !lockPar.activeSelf)
        {
            anim = animObject.GetComponent<Animator>(); //Get animator
            GameManagerScript gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get game manager

            //Update level objects to selected world
            for (int i = 0; i < 10; i++) //Loop through level objects
            {
                levels[i].SetActive(true); //Activate all levels objects
                levels[i].GetComponent<LevelUI>().level = i + 1 + ((world - 1) * 10); //Set level to i + the selected world (basically)
                levels[i].GetComponent<LevelUI>().Start(); //Update UI and deactivate if level isn't unlocked
            }
            anim.ResetTrigger("LevelsToWorld"); //Reset trigger
            anim.SetTrigger("WorldToLevels"); //Set trigger
            backButtonAnim.ResetTrigger("LevelsToWorld"); //Reset trigger
            backButtonAnim.SetTrigger("WorldToLevels"); //Set trigger
        }
    }

    //Slide from levels to world
    public void LevelsToWorld()
    {
        //If no levels is currently loading
        if (gMScript.asyncLoading == null)
        {
            anim = animObject.GetComponent<Animator>(); //Get animator
            anim.ResetTrigger("WorldToLevels"); //Reset trigger
            anim.SetTrigger("LevelsToWorld"); //Set trigger
            backButtonAnim.ResetTrigger("WorldToLevels"); //Reset trigger
            backButtonAnim.SetTrigger("LevelsToWorld"); //Set trigger
        }
    }
}
