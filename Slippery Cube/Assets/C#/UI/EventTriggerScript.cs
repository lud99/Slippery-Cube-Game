using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerScript : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    UIAnimation anim;

    //Get Animator
    void Start()
    {
        anim = GetComponent<UIAnimation>();
    }
    //Detect mouse click
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        anim.UIJump(true);
    }
    //Mouse exit
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.UIJumpBack(true);
    }
}
