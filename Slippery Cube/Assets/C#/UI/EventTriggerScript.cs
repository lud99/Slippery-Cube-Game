using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerScript : MonoBehaviour, /*IPointerDownHandler, IPointerExitHandler, */ISelectHandler, IDeselectHandler
{
    public GameObject viewport;
    public bool scrollX, scrollY;
    public int scrollAmount = 500;
    bool selected = false, doDeselectAnim = true;
    RectTransform rectTrans;
    UIAnimation anim;

    //Get Animator
    void Start()
    {
        anim = GetComponent<UIAnimation>();
        if (viewport != null) rectTrans = viewport.GetComponent<RectTransform>();
    }
    //Ignore deselect animation since it can cause objects to get stuck in an animation
    public void IgnoreDeselectAnimation()
    {
        doDeselectAnim = false;
    }
    /*/Detect mouse click
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        anim.UIJump(true);
    }*/
    /*/Mouse exit
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.UIJumpBack(true);
    }*/
    //Detect if selected
    public void OnSelect(BaseEventData eventData)
    {
        if (anim != null)
        {
            anim.UIJump(true);
        }
        selected = true;
        doDeselectAnim = true;
    }
    //Deselected
    public void OnDeselect(BaseEventData eventData)
    {
        if (doDeselectAnim) anim.UIJumpBackSmall();
        selected = false;
    }
    //Update
    void Update()
    {
        //Scroll right in scrollrect
        if (Input.GetKeyDown("right") && selected && scrollX)
        {
            rectTrans.position += new Vector3(-scrollAmount, 0, 0);
        }
        //Scroll left in scrollrect
        if (Input.GetKeyDown("left") && selected && scrollX)
        {
            rectTrans.position += new Vector3(scrollAmount, 0, 0);
        }
        //Scroll down in scrollrect
        if (Input.GetKeyDown("up") && selected && scrollY)
        {
            rectTrans.position += new Vector3(0, -scrollAmount, 0);
        }
        //Scroll up in scrollrect
        if (Input.GetKeyDown("down") && selected && scrollY)
        {
            rectTrans.position += new Vector3(0, scrollAmount, 0);
        }
    }
}
