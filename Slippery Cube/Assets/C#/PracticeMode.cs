using System.Collections.Generic;
using UnityEngine;

public class PracticeMode : MonoBehaviour {

    public GameObject checkpointPrefab;

    public GameObject[] obstacles, boxes, levelEdges;
    public List<GameObject> checkpointObjects = new List<GameObject>();
    public List<PracticeData> checkpoints = new List<PracticeData>();

    Rigidbody rb;
    FollowPlayer FollowPlayerScript;
    GameManagerScript gManager;

    //Data to save
    public class PracticeData
    {
        public Vector3 playerPosition, playerVelocity, playerAngularVelocity, gravity, cameraForward, cameraRight, cameraOffset;
        public Quaternion playerRotation, cameraRotation;
        public float playerForwardForce, playerSidewayForce;

        public Vector3[] obstaclePosition, obstacleVelocity, obstacleAngularVelocity,
                         boxPosition, boxVelocity, boxAngularVelocity;
        public Quaternion[] obstacleRotation, boxRotation;
        public bool[] obstacleRbEnabled, boxRbEnabled, levelEdgeEnabled = new bool[4];
    }

    //Get components
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("PracticeMode").Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    public void NewScene(GameObject[] levelEdgeArray, bool setCheckpoint = true)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) rb = player.GetComponent<Rigidbody>();

        levelEdges = levelEdgeArray;
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        boxes = GameObject.FindGameObjectsWithTag("Box");
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        FollowPlayerScript = Camera.main.GetComponent<FollowPlayer>();

        checkpoints = new List<PracticeData>();

        if (setCheckpoint) SaveCheckpoint(true);
    }

    //Re-enable player movement, as if nothing happened
    public void ResetDeath()
    {
        if (gManager.gameHasEnded)
        {
            gManager.gameHasEnded = false;
            rb.GetComponent<PlayerMovement>().enabled = true;
        }
    }

    //Check for input
    void Update () {
        //Save checkpoint
        if ((Input.GetButtonDown("Save Checkpoint") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) 
            && GameObject.Find("CompleteLevelUI") == null && !gManager.gameHasEnded) //Set checkpoint
        {
            SaveCheckpoint(); //Save checkpoint
        }
        //Remove checkpoint
        if (Input.GetButtonDown("Remove Checkpoint")) //Remove checkpoint
        {
            if (GameObject.Find("CompleteLevelUI") == null && gManager.asyncLoading == null) //Only load if level is not complete and not loading any other scene (async)
            {
                DeleteCheckpoint(); //Remove Checkpoint
            }
        }
    }
     
    //Save checkpoint
    public void SaveCheckpoint(bool saveOriginalData = false)
    {
        if (GameObject.Find("CompleteLevelUI") != null || gManager.gameHasEnded) return;

        PlayerMovement move = rb.GetComponent<PlayerMovement>();
        PracticeData data = new PracticeData();

        //Data to save
        //Player
        data.playerPosition = rb.transform.position; //Position
        data.playerVelocity = rb.velocity; //Velocity
        data.playerAngularVelocity = rb.angularVelocity; //Angular velocity
        data.playerRotation = rb.transform.rotation; //Rotation
        data.playerForwardForce = move.forwardForce; //Forward force
        data.playerSidewayForce = move.sidewayForce; //Sideway force

        //Gravity
        data.gravity = Physics.gravity;

        //Camera
        data.cameraOffset = FollowPlayerScript.offset; //Camera offset
        data.cameraRotation = FollowPlayerScript.rotate ? FollowPlayerScript.targetRotation : FollowPlayerScript.transform.rotation; //Camera rotation
        data.cameraForward = move.camForward; //Camera forward
        data.cameraRight = move.camRight; //Camera right

        //Level edge
        for (int i = 0; i < levelEdges.Length; i++)
            data.levelEdgeEnabled[i] = levelEdges[i].activeSelf;

        //Setup arrays to be the same size as the number of objects in scene

        //Obstacles
        int obsLen = obstacles.Length;
        data.obstaclePosition = new Vector3[obsLen];
        data.obstacleVelocity = new Vector3[obsLen];
        data.obstacleAngularVelocity = new Vector3[obsLen];
        data.obstacleRotation = new Quaternion[obsLen];
        data.obstacleRbEnabled = new bool[obsLen];

        //Boxes
        int boxLen = boxes.Length;
        data.boxPosition = new Vector3[boxLen];
        data.boxVelocity = new Vector3[boxLen];
        data.boxAngularVelocity = new Vector3[boxLen];
        data.boxRotation = new Quaternion[boxLen];
        data.boxRbEnabled = new bool[boxLen];

        //Loop through all obstacles in scene
        for (int i = 0; i < obsLen; i++)
        {
            Rigidbody _rb = obstacles[i].GetComponent<Rigidbody>();
            if (_rb != null)
            {
                data.obstaclePosition[i] = obstacles[i].transform.position; //Save position
                data.obstacleRotation[i] = obstacles[i].transform.rotation; //Save rotation
                data.obstacleVelocity[i] = _rb.velocity; //Save velocity
                data.obstacleAngularVelocity[i] = _rb.angularVelocity; //Save angular velocity
                data.obstacleRbEnabled[i] = _rb.detectCollisions;
            }
        }

        //Loop through all boxes in scene
        for (int i = 0; i < boxLen; i++)
        {
            Rigidbody _rb = boxes[i].GetComponent<Rigidbody>();
            if (_rb != null)
            {
                data.boxPosition[i] = boxes[i].transform.position; //Save position
                data.boxRotation[i] = boxes[i].transform.rotation; //Save rotation
                data.boxVelocity[i] = _rb.velocity; //Save velocity
                data.boxAngularVelocity[i] = _rb.angularVelocity; //Save angular velocity
                data.boxRbEnabled[i] = _rb.detectCollisions;
            }
        }

        if (saveOriginalData) checkpoints.Add(data);

        //Add to the checkpoints list
        else checkpoints.Add(data);

        //Instantiate the checkpoint prefab
        checkpointObjects.Add(Instantiate(checkpointPrefab, move.transform.position, FollowPlayerScript.rotate ? FollowPlayerScript.targetRotation : FollowPlayerScript.transform.rotation));

        Debug.Log("Saved Checkpoint");
    }

    //Delete checkpoint
    public void DeleteCheckpoint()
    {
        //If there is only one checkpoint
        if (checkpoints.Count <= 1)
        {
            Debug.Log("Can not remove the initial checkpoint");
            return;
        }

        //Remove the previous checkpoint
        checkpoints.RemoveAt(checkpoints.Count - 1); //Checkpoint data
        Destroy(checkpointObjects[checkpointObjects.Count - 1]); //Destroy checkpoint object
        checkpointObjects.RemoveAt(checkpointObjects.Count - 1); //Remove checkpoint object from the list
        Debug.Log("Deleted Checkpoint");
    }

    //Load checkpoint
    public void LoadCheckpoint()
    {
        //Get the last checkpoint
        PracticeData data = checkpoints[checkpoints.Count > 1 ? checkpoints.Count - 1 : 0];

        if (GameObject.Find("CompleteLevelUI") != null) return;
        if (data.obstaclePosition == null)
        {
            Debug.Log("No checkpoint saved");
            return;
        }

        //Freeze player
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        PlayerMovement move = rb.GetComponent<PlayerMovement>();

        //Data to load

        //Player
        rb.velocity = data.playerVelocity; //Velocity
        rb.transform.position = data.playerPosition; //Position
        rb.angularVelocity = rb.angularVelocity; //Angular velocity
        rb.transform.rotation = data.playerRotation; //Rotation
        move.forwardForce = data.playerForwardForce; //Forward force
        move.sidewayForce = data.playerSidewayForce; //Sideway force
        move.camForward = data.cameraForward; //Camera forward
        move.camRight = data.cameraRight; //Camera right

        //Gravity
        Physics.gravity = data.gravity;

        //Camera
        FollowPlayerScript.offset = data.cameraOffset; //Camera offset
        FollowPlayerScript.camForward = data.cameraForward; //Camera forward
        FollowPlayerScript.camRight = data.cameraRight; //Camera right
        FollowPlayerScript.targetRotation = data.cameraRotation; //Camera rotation
        FollowPlayerScript.rotate = true; //Rotate camera

        //Level edge
        for (int i = 0; i < levelEdges.Length; i++)
            levelEdges[i].SetActive(data.levelEdgeEnabled[i]);

        //Obstacles
        for (int i = 0; i < obstacles.Length; i++)
        {
            Rigidbody _rb = obstacles[i].GetComponent<Rigidbody>();
            if (_rb != null)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                _rb.velocity = data.obstacleVelocity[i]; //Load velocity
                obstacles[i].transform.position = data.obstaclePosition[i]; //Load position
                _rb.angularVelocity = data.obstacleAngularVelocity[i]; //Load angular velocity
                obstacles[i].transform.rotation = data.obstacleRotation[i]; //Load rotation
                _rb.detectCollisions = data.obstacleRbEnabled[i];
                _rb.isKinematic = !data.obstacleRbEnabled[i];
                _rb.constraints = RigidbodyConstraints.None;
            }
        }

        //Boxes
        for (int i = 0; i < boxes.Length; i++)
        {
            Rigidbody _rb = boxes[i].GetComponent<Rigidbody>();
            if (_rb != null)
            {
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                _rb.velocity = data.boxVelocity[i]; //Load velocity
                boxes[i].transform.position = data.boxPosition[i]; //Load position
                _rb.angularVelocity = data.boxAngularVelocity[i]; //Load angular velocity
                boxes[i].transform.rotation = data.boxRotation[i]; //Load rotation
                _rb.detectCollisions = data.boxRbEnabled[i];
                _rb.isKinematic = !data.boxRbEnabled[i];
                _rb.constraints = RigidbodyConstraints.None;
            }
        }

        //Unfreeze player
        rb.constraints = RigidbodyConstraints.None;

        Debug.Log("Loaded Checkpoint");
    }

    //Destroy checkpoint object
    public void DestroyCheckpoint(int index)
    {
        //If checkpoint object exists
        if (checkpointObjects[index] != null)
        {
            checkpoints.RemoveAt(index); //Checkpoint data
            Destroy(checkpointObjects[index]); //Destroy checkpoint object
            checkpointObjects.RemoveAt(index); //Remove checkpoint object from the list
        }
    }

    //Destroy all checkpoint objects
    public void DestroyAllCheckpoints()
    {
        //Loop through all checkpoints
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints.RemoveAt(i); //Checkpoint data
            Destroy(checkpointObjects[i]); //Destroy checkpoint object
            checkpointObjects.RemoveAt(i); //Remove checkpoint object from the list
        }
    }
}