using UnityEngine;
using UnityEngine.UI;

public class Colors : MonoBehaviour
{
    public GameObject lockPar, outline, colorScrollbar, trailColorScrollbar, newButton;
    public int materialIndex;
    public float size;
    public bool lockColor, setScrollbarPosition = true, setTrailScrollbarPosition, trailColor, trailSize, markAsNew, displayPlayerColor, markAsNewAnyColor;
    bool outlineFollow;
    GameManagerScript gMScript;

    //Start
    void Start()
    {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get game manager

        //Lock color if it's not unlocked
        if (lockColor && !trailColor && !gMScript.LoadJson().colorUnlocked[materialIndex]) //Unlock if previos level is done, not current (which is why 1 is subtracted)
        {
            lockPar.SetActive(true);
        }
        //Lock trail color if it's not unlocked
        if (lockColor && trailColor && !gMScript.LoadJson().trailColorUnlocked[materialIndex]) //Unlock if previos level is done, not current (which is why 1 is subtracted)
        {
            lockPar.SetActive(true);
        }
        //Display 'New' object if color hasn't been used yet
        if (!markAsNew && newButton != null) newButton.SetActive(false);
        if (!lockPar.activeSelf) //If not locked
        {
            if (gMScript.LoadJson().colorUsed[materialIndex] && !trailColor && markAsNew) newButton.SetActive(false);
            if (gMScript.LoadJson().trailColorUsed[materialIndex] && trailColor && markAsNew) newButton.SetActive(false);
        } else
        {
            newButton.SetActive(false); //Disable 'New' if color is locked
        }

        if (materialIndex == gMScript.LoadJson().currentOutfit) outline.GetComponent<ColorOutline>().color = gameObject;
        if (trailColor && materialIndex == gMScript.LoadJson().trailMaterial) outline.GetComponent<ColorOutline>().color = gameObject;
        if (trailSize && size == gMScript.LoadJson().trailSize) outline.GetComponent<ColorOutline>().color = gameObject;
        if (setScrollbarPosition) colorScrollbar.GetComponent<Scrollbar>().value = gMScript.LoadJson().colorScrollPosition;
        if (setTrailScrollbarPosition) trailColorScrollbar.GetComponent<Scrollbar>().value = gMScript.LoadJson().trailColorScrollPosition;
        if (displayPlayerColor) DisplayPlayerColor();

        //Activate new button if any color has not been used
        if (markAsNewAnyColor)
        {
            GameManagerScript.Data data = gMScript.LoadJson(); //Load data
            for (int i = 0; i < gMScript.LoadJson().colorUsed.Length; i++) //Check if any color hasn't been used
            {
                if (!data.colorUsed[i] && data.colorUnlocked[i] || !data.trailColorUsed[i] && data.trailColorUnlocked[i]) newButton.SetActive(true);
            }
        }
    }

    //On enabled
    void OnEnable()
    {
        if (newButton != null) newButton.GetComponent<Animator>().SetTrigger("NewIconJump"); //Set new object to play jump animation when enabled
    }

    //Update selected color
    public void UpdateSelectedColor()
    {
        if (!lockPar.activeSelf)
        {
            outline.GetComponent<ColorOutline>().color = gameObject;
            gMScript.GetComponent<Customization>().ChangePlayerMaterial(materialIndex);
            gMScript.SaveJsonOutfits(materialIndex, materialIndex, true, true);
            gMScript.SaveJsonScrollbar(colorScrollbar.GetComponent<Scrollbar>().value, -1f, -1f);
            if (newButton.activeSelf && markAsNew) newButton.SetActive(false);
        }
    }

    //Update selected color (trail)
    public void UpdateSelectedColorTrail()
    {
        if (!lockPar.activeSelf)
        {
            outline.GetComponent<ColorOutline>().color = gameObject;
            gMScript.GetComponent<Customization>().ChangeParticleMaterial(materialIndex);
            gMScript.SaveJsonTrail(materialIndex, materialIndex, -1f, true, true);
            gMScript.SaveJsonScrollbar(-1f, colorScrollbar.GetComponent<Scrollbar>().value, -1f); 
            if (newButton.activeSelf && markAsNew) newButton.SetActive(false);
        }
    }

    //Update trail size
    public void UpdateTrailSize()
    {
        if (!lockPar.activeSelf)
        {
            outline.GetComponent<ColorOutline>().color = gameObject;
            gMScript.GetComponent<Customization>().ChangeParticleSize(size);
            gMScript.SaveJsonTrail(0, -1, size, true, true);
        }
    }

    //Display player color
    public void DisplayPlayerColor()
    {
        Color playerColor = GameObject.Find("Player").GetComponent<Renderer>().material.color;
        GetComponent<Image>().color = new Color(playerColor.r, playerColor.g, playerColor.b, 1f);
    }
}
