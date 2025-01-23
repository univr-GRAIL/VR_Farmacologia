using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LogManager : MonoBehaviour
{
    [Header("Save Log Options")]
    [Tooltip("How often the log file is updated and saves the info")]
    [SerializeField] 
    float saveTime = 0.2f;

    [Tooltip("The player position (assign the GameObject that moves, like AutoHandPlayer)")]
    [SerializeField] 
    Transform playerPosition;

    [Tooltip("Rotation of the player's head (assign the main camera)")]
    [SerializeField] 
    Transform playerRotation;

    [Tooltip("Layer of the special items. Used to log if the user is looking at them or if the head is directed towards them. Deafult: NotifyVisible. PLEASE: Assign these special items to this layer!")]
    [SerializeField] 
    LayerMask layerMask;

    [Tooltip("Radius of the sphere used to determine if the head is pointing towards a special item (using a sphereCast)")]
    [SerializeField] 
    float radiusVisible = 0.1f;

    // What items the user is holding in their hand
    List<GameObject> handObjects;

    // Current gazed item
    GameObject currentGazedObject = null;

    // Tracks if the user just teleported (flag)
    bool justTeleported = false;

    // Timer to measure time between file saving
    float currentTime = 0f;

    // Where to save the log file
    string path; 

    // User position RELATIVE to initial position 'startPos' (X, Z)
    float xPos = 0.0f;
    float zPos = 0.0f;

    // Starting position of player
    Vector3 startPos;  

    public static LogManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // at each execution, create a new log file
    void Start()
    {
        // initialize hand items' list 
        handObjects = new List<GameObject>();
        // set initial player position
        startPos = playerPosition.position;

        // save path for log file (right now, on desktop)
        path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt"; ;

        // check if the file doesn't already exist. il false, create it
        if (!File.Exists(path))
            File.WriteAllText(path, "LOG " + SceneManager.GetActiveScene().name + "\n\n-------------------------------------------------------------------\n\n");
    }

    // save info in log file every 'saveTime'
    void Update()
    {
        // update timer
        currentTime += Time.deltaTime;

        // if the timer is greater than the save time, then save and reset the timer
        if (currentTime >= saveTime) 
        {
            AddLog(); 
            currentTime = 0f;
        }
    }

    // function to draw the gizmos in the scene (head direction ray and head spherecast) 
    void OnDrawGizmos() 
    {
        // set color of debug objects
        Gizmos.color = Color.red; 
        // draw sphere on user's head to view the 'field of view' (kinda) used to cast a sphere cast
        // (to detect if the user's head is toward a special item) 
        Gizmos.DrawSphere(playerRotation.position, radiusVisible);
        // draw head direction ray
        Gizmos.DrawLine(playerRotation.position, playerRotation.TransformDirection(Vector3.forward));
    }

    /*
     To make parsing the log easier, add an identifier to each line

    - Time: 
        begins with "[TIME "
        ends with "]"
        example: [TIME 17:27:53.793]

    - Teleport:
        if the user teleported:
            begins with "TELEPORT: YES"
            next line contains "LAST POS - (xPos, zPos)" 
                example: LAST POS - (0,0956018, 0,621542)
        if user did not teleport:
            begins with "TELEPORT: NO"
            no next lines for teleport

    - User position
        line contains "USER POS - (xPos, zPos)" 
            example: USER POS - (0,0956018, 0,621542)
        
    - Head orientation
        line contains "HEAD ORIENT - deg°"
            example: "HEAD ORIENT - 34,65643°"

    - Head pointing towards a special item:
        if the head is pointing at an item:
            begins with "HEAD POINTING: YES" 
            then line contains "HEAD POINTING AT - item"
                example: "HEAD POINTING AT - Pacchetto di Sigarette"
        if it's not pointing at anything:
            begins with "HEAD POINTING: NO"

    - User is gazing a special item:
        if user is looking at an item:
            begins with "GAZING: YES" 
            then line contains "GAZING AT - item"
                example: "GAZING AT - Pacchetto di Sigarette"
        if user is not looking at an item:
            begins with "GAZING: NO"

    - User is holding a special item in their hand:
        if user is holding an item:
            begins with "ITEMS: YES" 
            then line contains "ITEMS HAND - list of items"
                example: "ITEMS HAND - Pacchetto di Sigarette"
        if user is not holding an item:
            begins with "ITEMS: NO"
     */

    // Print log info in a file
    void AddLog()
    {
        //Time log
        File.AppendAllText(path, "[TIME " + System.DateTime.Now.ToString("HH:mm:ss.fff") + "]\n");

        // Log teleport
        if (justTeleported == false)
        {
            File.AppendAllText(path, "TELEPORT: NO - User did NOT teleport\n"); 
        }
        else if (justTeleported == true)
        {
            justTeleported = false; // re-set the var
            // print it
            File.AppendAllText(path, "TELEPORT: YES - User teleported \n \t Previous position (X, Z): LAST POS - (" + xPos + ", " + zPos + ")\n");  //Write that user just teleported + prev pos
        }

        // Moving log (y is not logged because user doesn't move up and down)
        xPos = playerPosition.position.x - startPos.x;  //x position relative to the starting point
        zPos = playerPosition.position.z - startPos.z;  //z position relative to the starting point
        // Save user position in cartesian coordinates
        File.AppendAllText(path, "User position (X, Z): USER POS - (" + xPos + ", " + zPos + ")\n"); 

        // Head orientation log
        // Take euler angles from player rotation
        Vector3 getPlayerRotation = playerRotation.rotation.eulerAngles; 
        // Make sure they are from 0 to 360 (not negatives or greater than 360)
        float regularAngle = getPlayerRotation.y % 360f;  
        // Save rotation, so the user head direction
        File.AppendAllText(path, "Head orientation (degrees): HEAD ORIENT - " + regularAngle + "°\n");

        // Log what item the head is pointing toward
        // Note: only log interesting items, such the interactables 
        // Variable to save info about what item the head is pointing toward
        RaycastHit hit;

        // Check if the head is pointing towards something
        if (Physics.SphereCast(playerRotation.position, radiusVisible, playerRotation.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            // Log item
            File.AppendAllText(path, "HEAD POINTING: YES - Head is pointing towards item: HEAD POINTING AT - " + hit.transform.gameObject.name + "\n");  
        else
            File.AppendAllText(path, "HEAD POINTING: NO - Head is NOT pointing towards a special item\n");

        // Log gazed item
        if (currentGazedObject != null)
            File.AppendAllText(path, "GAZING: YES - Gazed item: GAZING AT - " + currentGazedObject.name + "\n");  
        else
            File.AppendAllText(path, "GAZING: NO - Not gazing any special item \n"); 

        //Log items in hand
        // if the structure contains at least an item, save them as items in hand
        if (handObjects.Count != 0) 
        {
            string objects = ""; 
            // loop on saved objects
            for (int i = 0; i < handObjects.Count; i++)
            {
                // do not add a comma if the item is the last one
                if (i == handObjects.Count - 1)
                    objects += handObjects[i].name;
                // add a comma to separate items
                else
                    objects += handObjects[i].name + ", ";
            }
            File.AppendAllText(path, "ITEMS: YES - Items in hand: ITEMS HAND - " + objects + "\n\n-------------------------------------------------------------------\n\n");
        }
        // else, log that there are no items in hand
        else  
        {
            File.AppendAllText(path, "ITEMS: NO - No items in hand\n\n-------------------------------------------------------------------\n\n");
        }
    }

    // Function called whenever a special item is grabbed in hand
    // grabbedItem: the grabbed item
    public void GrabOn(GameObject grabbedItem) 
    {
        // add the object to the grabbed item list
        handObjects.Add(grabbedItem); 
        AddLog(); // update log
    }

    // Function called whenever a special item is released from an hand
    // releasedItem: the released item
    public void GrabOff(GameObject releasedItem)
    {
        // remove the object from the grabbed item list
        handObjects.Remove(releasedItem); 
        AddLog(); // update log
    }

    // Function called whenever a special item is gazed
    // gazedItem: gazed item
    public void GazeOn(GameObject gazedItem)
    {
        if(currentGazedObject != gazedItem) // change the current gazed item if the previous is different than the current one
            currentGazedObject = gazedItem;
        //AddLog();
    }

    // Function called whenever a special item is not gazed anymore
    // ungazedItem: un-gazed item
    public void GazeOff(GameObject ungazedItem)
    {
        currentGazedObject = null;
    }

    // Function called when the user teleports
    // Logs the teleport action
    public void OnTeleport()
    {
        justTeleported = true;
        AddLog();  // update log
    }
}
