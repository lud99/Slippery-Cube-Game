using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvenSystemHistory : MonoBehaviour
{
    EventSystem eventSystem;
    public List<GameObject> selectionHistory = new List<GameObject>();

    //Start
    void Start()
    {
        eventSystem = GetComponent<EventSystem>();
        selectionHistory[0] = eventSystem.firstSelectedGameObject;
    }

    //Update
    void Update()
    {
        //If a new object has been selected
        if (eventSystem.currentSelectedGameObject != selectionHistory[selectionHistory.Count - 1] &&
            eventSystem.currentSelectedGameObject != null)
        {
            selectionHistory.Add(eventSystem.currentSelectedGameObject);
        }

        if (eventSystem.currentSelectedGameObject == null && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            //Set selected to previous selected
            if (selectionHistory[selectionHistory.Count - 1].activeSelf) eventSystem.SetSelectedGameObject(selectionHistory[selectionHistory.Count - 1]);
            else
            {
                foreach (GameObject selection in selectionHistory)
                {
                    if (selection.activeSelf)
                    {
                        eventSystem.SetSelectedGameObject(selection);
                        break;
                    }
                }
            }
        }
    }
}
