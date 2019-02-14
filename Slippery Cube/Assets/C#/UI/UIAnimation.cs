using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public GameObject animObject, backButton, lockPar, selectedWorld, scrollbar;
    public GameObject[] levels;
    public bool SetTriggerOnStart, worldToLevelsTransitionOnStart;
    public string animTrigger;
    Animator anim, backButtonAnim;
    GameManagerScript gMScript;

    //Get components
    void Start()
    {
        anim = GetComponent<Animator>(); //Get animator
        if (backButton != null) backButtonAnim = backButton.GetComponent<Animator>();
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get game manager
        if (SetTriggerOnStart) SetSelectedTrigger(animTrigger);
        if (PlayerPrefs.HasKey("WorldLevelSelect") && worldToLevelsTransitionOnStart) WorldToLevels(PlayerPrefs.GetInt("WorldLevelSelect"));
    }

    //Jump 
    public void UIJump(bool self)
    {
        if (!self) anim = animObject.GetComponent<Animator>(); else anim = GetComponent<Animator>(); //Set to run animation on selected object if specified
        //If no levels is currently loading
        if (gMScript.asyncLoading == null && !anim.GetBool("WorldLevelsTransition"))
        {
            anim.ResetTrigger("UIJumpBack"); //Reset trigger
            anim.ResetTrigger("UIJumpBackSmall"); //Reset trigger
            anim.ResetTrigger("UIJumpDefault"); //Reset trigger
            anim.ResetTrigger("UIJumpLocked"); //Reset trigger
            anim.SetTrigger("UIJump"); //Set trigger
        }
    }
    //Jump Small
    public void UIJumpSmall()
    {
        anim = GetComponent<Animator>(); //Get animator
        //If no levels is currently loading
        if (gMScript.asyncLoading == null && !anim.GetBool("WorldLevelsTransition"))
        {
            anim.SetTrigger("UIJumpSmall"); //Set trigger
        }
    }

    //Jump Back
    public void UIJumpBack(bool self)
    {
        if (!self) anim = animObject.GetComponent<Animator>(); else anim = GetComponent<Animator>(); //Set to run animation on selected object if specified
        //If no levels is currently loading
        if (gMScript.asyncLoading == null && !anim.GetBool("WorldLevelsTransition"))
        {
            anim.ResetTrigger("UIJump"); //Reset trigger
            anim.ResetTrigger("UIJumpDefault"); //Reset trigger
            anim.ResetTrigger("UIJumpLocked"); //Reset trigger
            anim.ResetTrigger("UIJumpBack"); //Reset trigger
            anim.SetTrigger("UIJumpBack"); //Set trigger
        }
    }
    //Jump Back Small
    public void UIJumpBackSmall()
    {
        anim = GetComponent<Animator>(); //Get animator
        //If no levels is currently loading
        if (gMScript.asyncLoading == null && !anim.GetBool("WorldLevelsTransition"))
        {
            anim.SetTrigger("UIJumpBackSmall"); //Set trigger
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
        anim = GetComponent<Animator>(); //Get animator
        //If no levels is currently loading
        if (gMScript.asyncLoading == null && !anim.GetBool("WorldLevelsTransition"))
        {
            anim.ResetTrigger("UIJump"); //Reset trigger
            anim.ResetTrigger("UIJumpBack"); //Reset trigger
            anim.ResetTrigger("UIJumpDefault"); //Reset trigger
            anim.ResetTrigger("UIJumpLocked"); //Reset trigger
            anim.ResetTrigger("UIJumpLockedLevel"); //Reset trigger
            if (obj) anim.SetTrigger("UIJumpLocked"); else anim.SetTrigger("UIJumpLockedLevel"); //Set trigger
        }
    }

    //Unlock level animation
    public void UIUnlockLevel(bool disableMask)
    {
        anim = GetComponent<Animator>(); //Get animator
        //If no levels is currently loading
        //if (gMScript.asyncLoading == null && !anim.GetBool("WorldLevelsTransition"))
        //{
            anim.ResetTrigger("UIJump"); //Reset trigger
            anim.ResetTrigger("UIJumpBack"); //Reset trigger
            anim.ResetTrigger("UIJumpDefault"); //Reset trigger
            anim.ResetTrigger("UIJumpLocked"); //Reset trigger
            anim.ResetTrigger("UIJumpLockedLevel"); //Reset trigger
            if (!disableMask) animObject.GetComponent<Animator>().SetTrigger("LevelUnlock");
            anim.SetTrigger("LevelUnlock"); //Set trigger
        //}
    }

    //Slide from world to levels
    public void WorldToLevels(int world)
    {
        animObject.GetComponent<UIAnimation>().selectedWorld = gameObject;
        anim = animObject.GetComponent<Animator>(); //Get animator

        //If no levels is currently loading,  world is unlocked and not transitioning 
        if (gMScript.asyncLoading == null && lockPar != null && !lockPar.activeSelf && !anim.GetBool("WorldLevelsTransition"))
        {
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
            anim.SetBool("WorldLevelsTransition", true); //Set transitioning to true
            anim.SetBool("OnLevels", true);
            backButtonAnim.ResetTrigger("LevelsToWorld"); //Reset trigger
            backButtonAnim.SetTrigger("WorldToLevels"); //Set trigger
            gMScript.SaveJsonScrollbar(-1f, -1f, scrollbar.GetComponent<UnityEngine.UI.Scrollbar>().value); //Save world scrollbar position
        }
    }

    //Set previous world as selected
    public void SetPreviousWorldAsSelected()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(selectedWorld); //Set specified object to be selected
    }

    //Slide from levels to world
    public void LevelsToWorld()
    {
        anim = animObject.GetComponent<Animator>(); //Get animator
        //If no levels is currently loading
        if (gMScript.asyncLoading == null && !anim.GetBool("WorldLevelsTransition"))
        {
            anim.ResetTrigger("WorldToLevels"); //Reset trigger
            anim.SetTrigger("LevelsToWorld"); //Set trigger
            anim.SetBool("WorldLevelsTransition", true); //Set transitioning to true
            anim.SetBool("OnLevels", false);
            backButtonAnim.ResetTrigger("WorldToLevels"); //Reset trigger
            backButtonAnim.SetTrigger("LevelsToWorld"); //Set trigger
        }
    }

    //Set selected trigger
    public void SetSelectedTrigger(string trigger)
    {
        anim = GetComponent<Animator>(); //Get animator
        anim.SetTrigger(trigger); //Set trigger
    }

    //Set 'WorldLevelsTransition' from animation event
    public void WorldLevelsTransition(int state)
    {
        bool stateBool ;
        if (state == 0) stateBool = false; else stateBool = true; //Convert to bool
        animObject.GetComponent<Animator>().SetBool("WorldLevelsTransition", stateBool); //Set transitioning to whatever is specified (0 = false, 1 = true)
        //if (stateBool == false) 
    }
    //Play level unlock animation on levels
    public void SetLevelAsUnlocked()
    {
        //Update level objects to selected world
        for (int i = 0; i < 10; i++) //Loop through level objects
        {
            levels[i].SetActive(true); //Activate all levels objects
            levels[i].GetComponent<LevelUI>().PlayUnlockAnimation(); //Update UI and deactivate if level isn't unlocked
        }
    }
    //Deactivate Lock
    public void DeactivateLock()
    {
        if (transform.Find("Lock") != null) transform.Find("Lock").gameObject.SetActive(false);
        PlayerPrefs.DeleteKey("LevelToUnlock"); //Delete level to unlock
        PlayerPrefs.DeleteKey("WorldToUnlock"); //Delete world to unlock
    }
    //Set 'UIPopupFadeBool' from animation event
    public void UIPopupFadeBool(int state)
    {
        bool stateBool;
        if (state == 0) stateBool = false; else stateBool = true; //Convert to bool
        animObject.GetComponent<Animator>().SetBool("UIPopupFadeBool", stateBool); //Set transitioning to whatever is specified (0 = false, 1 = true)
    }
}
