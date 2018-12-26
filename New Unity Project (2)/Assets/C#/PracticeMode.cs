using UnityEngine;
using System.IO;

public class PracticeMode : MonoBehaviour {

    Rigidbody rb;
    FollowPlayer FollowPlayerScript;

    //Get components
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FollowPlayerScript = Camera.main.GetComponent<FollowPlayer>();
        SaveCheckpoint(); //Set default checkpoint on start
    }

    //Check for input
    void Update () {
        //If left mouse button is pressed (not hold)
        if (Input.GetButtonDown("Fire1")) //Set checkpoint
        {
            if (GameObject.Find("CompleteLevelUI") == null) //Only save if level is not complete
            {
                SaveCheckpoint(); //Save checkpoint
            }
        }
        //If right mouse button is pressed (not hold)
        if (Input.GetButtonDown("Fire2")) //Load checkpoint
        {
            if (GameObject.Find("CompleteLevelUI") == null && File.Exists(Application.dataPath + "/practicesave.json")) //Only load if level is not complete and
            {
                LoadCheckpoint(); //Load Checkpoint
            }
        }
        //If middle mouse button is pressed (not hold)
        if (Input.GetButtonDown("Fire3")) //Load checkpoint
        {
            if (GameObject.Find("CompleteLevelUI") == null) //Only load if level is not complete and
            {
                DeleteCheckpoint(); //Load Checkpoint
            }
        }
    }

    //Data to save
    private class Data
    {
        public Vector3 practicePosition, practiceVelocity, practiceGravity, practiceCamera;
        public Quaternion practiceRotation;
    }

    //Save checkpoint (Json)
    void SaveCheckpoint()
    {
        //Setup
        Data data = new Data(); //Data to save

        //Data to save
        data.practicePosition = transform.position; //Position
        data.practiceVelocity = rb.velocity; //Velocity
        data.practiceRotation = transform.rotation; //Rotation
        data.practiceGravity = Physics.gravity; //Gravity
        data.practiceCamera = FollowPlayerScript.offset; //Camera

        string json = JsonUtility.ToJson(data); //Convert data to json
        File.WriteAllText(Application.dataPath + "/practicesave.json", json); //Write data to file
    }

    //Delete checkpoint (Json)
    public void DeleteCheckpoint()
    {
        //Setup
        Data data = new Data(); //Data to save

        //Data to delete
        data.practicePosition = new Vector3(0, 0, 0); //Position
        data.practiceVelocity = new Vector3(0, 0, 0); //Velocity
        data.practiceRotation = Quaternion.Euler(0, 0, 0); //Rotation
        data.practiceGravity = new Vector3(0, 0, 0); //Gravity
        data.practiceCamera = new Vector3(0, 0, 0); //Camera

        string json = JsonUtility.ToJson(data); //Convert data to json
        File.WriteAllText(Application.dataPath + "/practicesave.json", json); //Write data to file
        File.Delete(Application.dataPath + "/practicesave.json");
    }

    //Load checkpoint (Json)
    Data LoadCheckpoint()
    {
        string json = File.ReadAllText(Application.dataPath + "/practicesave.json"); //Read from file
        Data loadedData = JsonUtility.FromJson<Data>(json); //Convert json to data

        //Data to load
        transform.position = loadedData.practicePosition; //Position
        rb.velocity = loadedData.practiceVelocity; //Velocity
        transform.rotation = loadedData.practiceRotation; //Rotation
        Physics.gravity = loadedData.practiceGravity; //Gravity
        FollowPlayerScript.offset = loadedData.practiceCamera; //Camera

        return loadedData;
    }
}