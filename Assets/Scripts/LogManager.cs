using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LogManager : MonoBehaviour
{
    [Header("Save Log Options")]
    [SerializeField] float saveTime = 0.2f;  //Rappresenta ogni quanto viene aggiornato il file di log
    [SerializeField] Transform playerPosition;  //Rappresenta la posizione dell'utente (Assegnare la componente che effettivamente si muove)
    [SerializeField] Transform playerRotation;  //Rappresenta la direzione dello sguardo dell'utente (Assegnare la camera principale)
    [SerializeField] LayerMask layerMask;  //Layer degli oggetti che si vuole sapere vengano guardati [NotifyVisible -> Assegnare questi oggetti a questo layer]
    [SerializeField] float radiusVisible = 0.05f;  //Raggio della sfera per vedere gli oggetti "particolari" guardati dall'utente

    List<GameObject> handObjects;  //Struttura per tenere traccia degli oggetti che l'utente ha in mano
    GameObject currentGazedObject = null;  //Oggetto attuale guardato dall'utente
    bool justTeleported = false;
    float currentTime = 0f;  //Timer per tenere conto del tempo passato da un salvataggio ad un altro
    string path;  //Path del file di log
    // User position RELATIVE to initial position (X, Z)
    float xPos = 0.0f;
    float zPos = 0.0f;

    Vector3 startPos;  //Vettore per salvare la posizione iniziale del giocatore

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

    //All'inizio dell'esecuzione verr� creato il file di log corrispondente a questa esecuzione
    void Start()
    {
        //Inizializziamo la lista degli oggetti in mano e la posizione iniziale del giocatore
        handObjects = new List<GameObject>();
        startPos = playerPosition.position;

        //Path dove verr� salvato il file (per ora sul Desktop)
        path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt"; ;

        //Controlliamo se il file esiste gi� (non dovrebbe gi� esistere perch� � impossibile eseguire due volte nello stesso tempo)
        //e lo creiamo scrivendo il nome della scena attuale all'inizio
        if (!File.Exists(path))
            File.WriteAllText(path, "LOG " + SceneManager.GetActiveScene().name + "\n\n-------------------------------------------------------------------\n\n");
    }

    //Ogni saveTime secondi verr� aggiunte informazioni al file di log
    void Update()
    {
        currentTime += Time.deltaTime;  //Aggiorniamo il timer
        if (currentTime >= saveTime)  //Se il timer � maggiore del tempo di salvataggio allora effettuiamo il salvataggio e azzeriamo il timer
        {
            AddLog();  //Chiamiamo la funzione per aggiornare il log
            currentTime = 0f;  //Reimpostiamo il timer a 0
        }
    }

    //Funzione per disegnare la sfera nella scena e capire quanto grande effettivamente è
    void OnDrawGizmos()  //Usata solo per debug, si pu� benissimo commentare
    {
        Gizmos.color = Color.red;  //Per settare il colore dei disegni di debug
        Gizmos.DrawSphere(playerRotation.position, radiusVisible);  //Per capire la grandezza del campo visivo
        Gizmos.DrawLine(playerRotation.position, playerRotation.TransformDirection(Vector3.forward));  //Per capire la direzione di dove si guarda
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

        // todo: change con cono e mesh. se oggetto sta dentro, stampa

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

    //Funzione che imposta un flag per dire alla funzione AddLog che l'utente ha preso in mano un oggetto
    //e poi chiama tale funzione per salvare l'oggetto per evitare che venga perso il salvataggio dell'oggetto
    //gameObject � il riferimento all'oggetto che viene preso
    public void GrabOn(GameObject gameObject)  //Funzione da chiamare quando viene preso in mano un oggetto
    {
        handObjects.Add(gameObject);  //Aggiungiamo l'oggetto preso alla struttura utilizzata per memorizzare gli oggetti
        AddLog(); // update log
    }

    //Funzione che imposta un flag per dire alla funzione AddLog che l'utente ha lasciato dalla mano un oggetto
    //e poi chiama tale funzione per salvare l'oggetto per evitare che venga perso il salvataggio dell'oggetto
    //gameObject � il riferimento all'oggetto che viene lasciato
    public void GrabOff(GameObject gameObject)  //Funzione da chiamare quando viene lasciato dalla mano un oggetto
    {
        handObjects.Remove(gameObject);  //Aggiungiamo l'oggetto preso alla struttura utilizzata per memorizzare gli oggetti
        AddLog(); // update log
    }

    public void GazeOn(GameObject gameObject)
    {
        if(currentGazedObject != gameObject)
            currentGazedObject = gameObject;
        //AddLog();
    }

    public void GazeOff(GameObject gameObject)
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
