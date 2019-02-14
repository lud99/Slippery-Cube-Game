using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        UpdateColor();
    }

    //Update color
    public void UpdateColor()
    {
        Material playerMat = player.GetComponent<Renderer>().material;
        GetComponent<Light>().color = new UnityEngine.Color(playerMat.color.r, playerMat.color.g, playerMat.color.b, 1f);
    }
}
