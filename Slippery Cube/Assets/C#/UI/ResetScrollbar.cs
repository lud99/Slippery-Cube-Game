using UnityEngine;
using UnityEngine.UI;

public class ResetScrollbar : MonoBehaviour {

    public float value;

	//Reset scrollbar to starting value
	void Start ()
    {
        GetComponent<Scrollbar>().value = value;
	}
}
