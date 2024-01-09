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
    float currentTime = 0f;  //Timer per tenere conto del tempo passato da un salvataggio ad un altro
    string path;  //Path del file di log

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

    //Funzione per disegnare la sfera nella scena e capire quanto grande effettivamente �
    void OnDrawGizmos()  //Usata solo per debug, si pu� benissimo commentare
    {
        Gizmos.color = Color.red;  //Per settare il colore dei disegni di debug
        Gizmos.DrawSphere(playerRotation.position, radiusVisible);  //Per capire la grandezza del campo visivo
        Gizmos.DrawLine(playerRotation.position, playerRotation.TransformDirection(Vector3.forward));  //Per capire la direzione di dove si guarda
    }

    //Funzione per aggiungere informazioni al file di log
    void AddLog()
    {
        //Log per tempo di gioco
        File.AppendAllText(path, "[LOG " + System.DateTime.Now.ToString("HH:mm:ss.fff") + "]\n");  //Salviamo l'orario di salvtaggio del log

        //Log per movimento (la posizione y non serve in quanto non si muove verso l'altro o il basso l'utente
        float xPos = playerPosition.position.x - startPos.x;  //Calcoliamo la posizione x rispetto a quella iniziale
        float zPos = playerPosition.position.z - startPos.z;  //Calcoliamo la posizione z rispetto a quella iniziale
        File.AppendAllText(path, "- Posizione Utente (X, Z): (" + xPos + ", " + zPos + ")\n");  //Salviamo la posizione in coordinate cartesiane

        //Log per dove la testa è rivolta
        Vector3 getPlayerRotation = playerRotation.rotation.eulerAngles;  //Prendiamo gli angoli di eulero della rotazione del giocatore
        float regularAngle = getPlayerRotation.y % 360f;  //Ci assicuriamo che siano compresi tra 0 e 360 (quindi non negativi o maggiori di 360)
        File.AppendAllText(path, "- Direzione Testa in Gradi: " + regularAngle + "°\n");  //Salviamo la rotazione, quindi la direzione di dove è rivolta la testa l'utente

        //Log per sapere verso che oggetto è rivolta la testa
        RaycastHit hit;  //Variabile per salvare le informazioni verso che oggetto è rivolta la testa
        if (Physics.SphereCast(playerRotation.position, radiusVisible, playerRotation.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))  //Controlliamo che la testa sia effettivamente rivolta verso qualcosa
            File.AppendAllText(path, "- Verso che oggetto è rivolta la testa: " + hit.transform.gameObject.name + "\n");  //Salviamo l'oggetto
        else  //Altrimenti salviamo che la testa non è rivolta verso nessun oggetto in particolare
            File.AppendAllText(path, "- Testa non rivolta verso nessun oggetto particolare\n");

        // Log oggetti guardati con sguardo
        if (currentGazedObject != null)
            File.AppendAllText(path, "- Oggetto guardato: " + currentGazedObject.name + "\n");  //Salviamo l'oggetto guardato
        else
            File.AppendAllText(path, "- Nessun oggetto particolare guardato\n"); 

        //Log per oggetti in mano
        if (handObjects.Count != 0)  //Se la struttura ha almeno un oggetto allora salviamo gli oggetti che ha in mano
        {
            string objects = "";  //Prepariamo la stringa per salvare gli oggetti
            for (int i = 0; i < handObjects.Count; i++)  //Cicliamo attraverso tutti gli oggetti salvati
            {
                if (i == handObjects.Count - 1)  //Se l'oggetto è l'ultimo allora non aggiungiamo una virgola
                    objects += handObjects[i].name;
                else  //Altrimenti aggiungiamo una virgola per separare gli oggetti
                    objects += handObjects[i].name + ", ";
            }
            File.AppendAllText(path, "- Oggetti in Mano: " + objects + "\n\n-------------------------------------------------------------------\n\n");
        }
        else  //Altrimenti salviamo il fatto che non ha nessun oggetto in mano
        {
            File.AppendAllText(path, "- Nessun Oggetto in Mano\n\n-------------------------------------------------------------------\n\n");
        }
    }

    //Funzione che imposta un flag per dire alla funzione AddLog che l'utente ha preso in mano un oggetto
    //e poi chiama tale funzione per salvare l'oggetto per evitare che venga perso il salvataggio dell'oggetto
    //gameObject � il riferimento all'oggetto che viene preso
    public void GrabOn(GameObject gameObject)  //Funzione da chiamare quando viene preso in mano un oggetto
    {
        handObjects.Add(gameObject);  //Aggiungiamo l'oggetto preso alla struttura utilizzata per memorizzare gli oggetti
        AddLog();  //Chiamiamo la funzione per aggiornare il log
    }

    //Funzione che imposta un flag per dire alla funzione AddLog che l'utente ha lasciato dalla mano un oggetto
    //e poi chiama tale funzione per salvare l'oggetto per evitare che venga perso il salvataggio dell'oggetto
    //gameObject � il riferimento all'oggetto che viene lasciato
    public void GrabOff(GameObject gameObject)  //Funzione da chiamare quando viene lasciato dalla mano un oggetto
    {
        handObjects.Remove(gameObject);  //Aggiungiamo l'oggetto preso alla struttura utilizzata per memorizzare gli oggetti
        AddLog();  //Chiamiamo la funzione per aggiornare il log
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
}
