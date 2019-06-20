using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventTriggerScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    public GameObject viewport;
    public bool scrollX, scrollY, jumpBackAfterClick, jumpDefault = true;
    public int scrollAmount = 500;
    public float jumpBackAfterClickTime;
    bool selected = false, doDeselectAnim = true;
    EventSystem eventSystem;
    RectTransform rectTrans;
    UIAnimation anim;

    //Get Animator
    void Start()
    {
        anim = GetComponent<UIAnimation>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if (viewport != null) rectTrans = viewport.GetComponent<RectTransform>();
    }
    //Ignore deselect animation since it can cause objects to get stuck in an animation
    public void IgnoreDeselectAnimation()
    {
        doDeselectAnim = false;
    }
    //Detect mouse click
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (jumpDefault)
            anim.UIJumpDefault(true);

        selected = true;
    }
    //Detect mouse click
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (selected)
        { 
            GetComponent<Button>().onClick.Invoke();
            if (jumpBackAfterClick)
                StartCoroutine(OnSelectWait(jumpBackAfterClickTime));
        }
    }
    //Mouse select
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        anim.UIJump(true);

        selected = true;
        eventSystem.SetSelectedGameObject(gameObject);
    }
    //Mouse exit
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.UIJumpBack(true);

        selected = false;
    }
    //Detect if selected
    public void OnSelect(BaseEventData eventData = null)
    {
        if (anim != null)
            anim.UIJump(true);

        selected = true;
        doDeselectAnim = true;
    }
    //Deselected
    public void OnDeselect(BaseEventData eventData)
    {
        if (doDeselectAnim) anim.UIJumpBackSmall();

        selected = false;
    }
    //Wait
    IEnumerator OnSelectWait(float time)
    {
        yield return new WaitForSeconds(time);
        if (selected) OnSelect();
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
