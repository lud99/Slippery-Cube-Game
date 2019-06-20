using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    GameObject unlockPopup;
    GameManagerScript gMScript;
    Animator anim;
    Text unlockName, unlockText;
    Image unlockImage;

    //Start
    void Start()
    {
        gMScript = GetComponent<GameManagerScript>(); //Get game manager script
        unlockPopup = GameObject.Find("UnlockPopup"); //Get popup object
        unlockPopup.SetActive(false); //Deactivate popup
        anim = unlockPopup.GetComponent<Animator>(); //Get popup animator
        unlockName = unlockPopup.transform.GetChild(0).GetComponent<Text>(); //Get popup name text
        unlockText = unlockPopup.transform.GetChild(1).GetComponent<Text>(); //Get popup unlocked text
        unlockImage = unlockPopup.transform.GetChild(2).GetComponent<Image>(); //Get popup image
    }

    //Update
    void Update()
    {
        //Load data
        GameManagerScript.Data data = gMScript.LoadJson();

        //Check if any requirements to unlock stuff are met

        //Levels
        //Complete level 1
        if (data.levelDone[0] && !data.colorUnlocked[4] && !anim.GetBool("UIPopupFadeBool")) //Unlock Dark red
        {
            Unlock("First level!", "Unlocked new Color!", true, new Color32(135, 18, 18, 255));
            gMScript.SaveJsonOutfits(4, -1, true, false); //Unlock color
        }
        //Complete level 3
        if (data.levelDone[2] && !data.colorUnlocked[7] && !anim.GetBool("UIPopupFadeBool")) //Unlock Pink
        {
            Unlock("Pink", "Unlocked new Color!", true, new Color32(255, 105, 180, 255));
            gMScript.SaveJsonOutfits(7, -1, true, false); //Unlock color
        }
        //Complete level 5
        if (data.levelDone[4] && !data.trailColorUnlocked[3] && !anim.GetBool("UIPopupFadeBool")) //Unlock Orange (trail)
        {
            Unlock("5th level", "Unlocked new Trail Color!", true, new Color32(23, 120, 23, 255));
            gMScript.SaveJsonTrail(3, -1, -1f, true, false); //Unlock trail color
        }
        //Complete level 7
        if (data.levelDone[6] && !data.colorUnlocked[13] && !anim.GetBool("UIPopupFadeBool")) //Unlock Brown
        {
            Unlock("Definitely not poop", "Unlocked new Color!", true, new Color32(160, 82, 45, 255));
            gMScript.SaveJsonOutfits(13, -1, true, false); //Unlock color
        }
        //Complete level 9
        if (data.levelDone[8] && !data.trailColorUnlocked[26] && !anim.GetBool("UIPopupFadeBool")) //Unlock White trail
        {
            Unlock("The next level is very hard", "Unlocked new Trail Color!", true, new Color32(124, 37, 183, 255));
            gMScript.SaveJsonTrail(26, -1, -1f, true, false); //Unlock trail color
        }
        //Complete level 10
        if (data.levelDone[9] && !data.colorUnlocked[29] && !anim.GetBool("UIPopupFadeBool")) //Unlock Cyan (transparent)
        {
            Unlock("Legend", "Unlocked new Transparent Color!", true, new Color32(95, 255, 239, 150));
            gMScript.SaveJsonOutfits(29, -1, true, false); //Unlock color
        }
        //Complete level 11
        if (data.levelDone[10] && !data.colorUnlocked[12] && !anim.GetBool("UIPopupFadeBool")) //Unlock Orange
        {
            Unlock("Gravity", "Unlocked new Color!", true, new Color32(255, 129, 35, 255));
            gMScript.SaveJsonOutfits(12, -1, true,  false); //Unlock color
        }
        //Complete level 13
        if (data.levelDone[12] && !data.colorUnlocked[24] && !anim.GetBool("UIPopupFadeBool")) //Unlock Light gray
        {
            Unlock("You're getting closer", "Unlocked new Color!", true, new Color32(128, 128, 128, 255));
            gMScript.SaveJsonOutfits(24, -1, true, false); //Unlock color
        }
        //Complete level 15
        if (data.levelDone[14] && !data.colorUnlocked[15] && !anim.GetBool("UIPopupFadeBool")) //Unlock Box (transparent)
        {
            Unlock("I'm a transparent box", "Unlocked new Transparent Color!", true, new Color32(214, 111, 47, 100));
            gMScript.SaveJsonOutfits(15, -1, true, false); //Unlock color
        }
        //Complete level 17
        if (data.levelDone[16] && !data.colorUnlocked[15] && !anim.GetBool("UIPopupFadeBool")) //Unlock Light yellow
        {
            Unlock("Almost there!", "Unlocked new Color!", true, new Color32(135, 18, 18, 255));
            gMScript.SaveJsonOutfits(15, -1, true, false); //Unlock color
        }
        //Complete level 19
        if (data.levelDone[18] && !data.colorUnlocked[15] && !anim.GetBool("UIPopupFadeBool")) //Unlock Chalk white
        {
            Unlock("One more to go!", "Unlocked new Color!", true, new Color32(224, 219, 209, 255));
            gMScript.SaveJsonOutfits(15, -1, true, false); //Unlock color
        }
        //Complete level 20
        if (data.levelDone[19] && !data.colorUnlocked[27] && !anim.GetBool("UIPopupFadeBool")) //Unlock Rainbow
        {
            Unlock("Finish the game", "Unlocked new Color!", true, new Color32(135, 18, 18, 255));
            gMScript.SaveJsonOutfits(27, -1, true, false); //Unlock color
        }

        //Coins
        //All coins in any level

        //All coins in level 1
        if (data.levelCoins[0] == 4 && !data.trailColorUnlocked[6] && !anim.GetBool("UIPopupFadeBool")) //Unlock Dark blue trail
        {
            Unlock("Coins", "Unlocked new Color!", true, new Color32(44, 75, 159, 255));
            gMScript.SaveJsonTrail(6, -1, -1f, true, false); //Unlock trail color
        }
        //All coins in level 2
        if (data.levelCoins[1] == 4 && !data.colorUnlocked[3] && !anim.GetBool("UIPopupFadeBool")) //Unlock Dark green
        {
            Unlock("Hard?", "Unlocked new Color!", true, new Color32(23, 120, 23, 255));
            gMScript.SaveJsonOutfits(3, -1, true, false); //Unlock color
        }
        //All coins in level 3
        if (data.levelCoins[2] == 6 && !data.trailColorUnlocked[9] && !anim.GetBool("UIPopupFadeBool")) //Unlock purple trail
        {
            Unlock("Purple", "Unlocked new Color!", true, new Color32(124, 37, 183, 255));
            gMScript.SaveJsonTrail(9, -1, -1f, true, false); //Unlock color
        }
        //All coins in level 4
        if (data.levelCoins[3] == 10 && !data.trailColorUnlocked[4] && !anim.GetBool("UIPopupFadeBool")) //Unlock Dark red trail
        {
            Unlock("You like coins?", "Unlocked new Color!", true, new Color32(135, 18, 18, 255));
            gMScript.SaveJsonTrail(4, -1, -1f, true, false); //Unlock color
        }
        //All coins in level 5
        if (data.levelCoins[4] == 8 && !data.trailColorUnlocked[13] && !anim.GetBool("UIPopupFadeBool")) //Unlock Brown trail
        {
            Unlock("Did you forget to wipe your butt Alex?", "Unlocked new Trail Color!", true, new Color32(160, 82, 45, 255));
            gMScript.SaveJsonTrail(13, -1, -1f, true, false); //Unlock color
        }
        //All coins in level 6
        if (data.levelCoins[5] == 9 && !data.colorUnlocked[9] && !anim.GetBool("UIPopupFadeBool")) //Unlock Purple
        {
            Unlock("Why am i even collecting coins?, Sam asked", "Unlocked new Color!", true, new Color32(124, 37, 183, 255));
            gMScript.SaveJsonOutfits(9, -1, true, false); //Unlock color
        }
        //All coins in level 7
        if (data.levelCoins[6] == 15 && !data.trailColorUnlocked[19] && !anim.GetBool("UIPopupFadeBool")) //Unlock Light green trail
        {
            Unlock("Ramp to heaven", "Unlocked new Color!", true, new Color32(110, 202, 89, 255));
            gMScript.SaveJsonTrail(19, -1, -1f, true, false); //Unlock color
        }
        //All coins in level 8
        if (data.levelCoins[7] == 12 && !data.colorUnlocked[19] && !anim.GetBool("UIPopupFadeBool")) //Unlock Light green 
        {
            Unlock("Too easy", "Unlocked new Color!", true, new Color32(110, 202, 89, 255));
            gMScript.SaveJsonOutfits(19, -1, true, false); //Unlock color
        }
        //All coins in level 9
        if (data.levelCoins[8] == 17 && !data.trailColorUnlocked[29] && !anim.GetBool("UIPopupFadeBool")) //Unlock Cyan trail (transparent)
        {
            Unlock("Avoid the ramp", "Unlocked new Trail Color!", true, new Color32(95, 255, 239, 255));
            gMScript.SaveJsonTrail(29, -1, -1f, true, false); //Unlock color
        }
        //All coins in level 10
        if (data.levelCoins[9] == 42 && !data.colorUnlocked[25] && !anim.GetBool("UIPopupFadeBool")) //Unlock Chalk white
        {
            Unlock("Absolute madlad", "Unlocked new Color!", true, new Color32(224, 219, 209, 255));
            gMScript.SaveJsonOutfits(25, -1, true, false); //Unlock color
        }
        //All coins in level 11
        if (data.levelCoins[10] == 16 && !data.colorUnlocked[25] && !anim.GetBool("UIPopupFadeBool")) //Unlock Chalk white
        {
            Unlock("Absolute madlad", "Unlocked new Color!", true, new Color32(224, 219, 209, 255));
            gMScript.SaveJsonOutfits(25, -1, true, false); //Unlock color
        }
        //All coins in level 12
        if (data.levelCoins[11] == 15 && !data.colorUnlocked[25] && !anim.GetBool("UIPopupFadeBool")) //Unlock Chalk white
        {
            Unlock("Absolute madlad", "Unlocked new Color!", true, new Color32(224, 219, 209, 255));
            gMScript.SaveJsonOutfits(25, -1, true, false); //Unlock color
        }
        //All coins in level 13
        if (data.levelCoins[12] == 15 && !data.colorUnlocked[25] && !anim.GetBool("UIPopupFadeBool")) //Unlock Chalk white
        {
            Unlock("Absolute madlad", "Unlocked new Color!", true, new Color32(224, 219, 209, 255));
            gMScript.SaveJsonOutfits(25, -1, true, false); //Unlock color
        }
        //All coins in level 14
        if (data.levelCoins[13] == 11 && !data.colorUnlocked[25] && !anim.GetBool("UIPopupFadeBool")) //Unlock Chalk white
        {
            Unlock("Absolute madlad", "Unlocked new Color!", true, new Color32(224, 219, 209, 255));
            gMScript.SaveJsonOutfits(25, -1, true, false); //Unlock color
        }
        //All coins in level 15
        if (data.levelCoins[14] == 15 && !data.colorUnlocked[25] && !anim.GetBool("UIPopupFadeBool")) //Unlock Chalk white
        {
            Unlock("Absolute madlad", "Unlocked new Color!", true, new Color32(224, 219, 209, 255));
            gMScript.SaveJsonOutfits(25, -1, true, false); //Unlock color
        }
        //All coins in world 1
        if (data.worldCoins[0] == 127 && !data.trailColorUnlocked[21] && !anim.GetBool("UIPopupFadeBool")) //Unlock Black trail
        {
            Unlock("Collect all coins in World 1", "Unlocked new Trail Color!", true, new Color32(20, 20, 20, 255));
            gMScript.SaveJsonTrail(21, -1, -1f, true, false); //Unlock color
        }
        //All coins in world 2
        if (data.worldCoins[1] == 300 && !data.colorUnlocked[27] && !anim.GetBool("UIPopupFadeBool")) //Unlock White
        {
            Unlock("Collect all coins in World 2", "Unlocked new Color!", true, new Color32(255, 255, 255, 255));
            gMScript.SaveJsonTrail(21, -1, -1f, true, false); //Unlock color
        }
        //All coins in the game (world 1 & world 2)
        if (data.worldCoins[0] == 127 && data.worldCoins[1] == 300 && !data.colorUnlocked[17] && !anim.GetBool("UIPopupFadeBool")) //Unlock Yellow
        {
            Unlock("Collect all coins in the game", "Unlocked new Color!", true, new Color32(245, 189, 24, 255));
            gMScript.SaveJsonOutfits(17, -1, true, false); //Unlock color
        }
        //Deaths
        //First death (1)
        //10th death (10)
        //50th death (50)
        //100th death (100)


    }

    //Unlock funcition
    void Unlock(string name, string text, bool changeColor, Color32 color)
    {
        unlockName.text = name; //Change name text
        unlockText.text = text; //Change unlocked text
        if (changeColor) unlockImage.color = color; //Change image color
        unlockPopup.SetActive(true); //Activate popup
        anim.SetTrigger("UIPopupFade"); //Activate animation
        anim.SetBool("UIPopupFadeBool", true);
    }
}
