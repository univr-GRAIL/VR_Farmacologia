using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportwanim : MonoBehaviour
{
    public Transform CameraAnchor;
    public Material LineMat;

    private LineRenderer LR;
    private bool validDestination = false;
    private bool hold = false;
    private bool trigger = false;
    private Vector3 destination;
    private GameObject destPointer;
    public CapsuleCollider CapC;

    public UnityEngine.InputSystem.InputActionProperty keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<LineRenderer>();
        LR = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(destPointer);
        RaycastHit hit;
        Physics.Raycast(CameraAnchor.position, CameraAnchor.forward, out hit, 100f);//, LayerMask.GetMask("Ignore Raycast"));
        GetHold();
        if (hold)
        {
            LR.positionCount = 2;
            LR.SetWidth(0.01f, 0.01f);
            LR.SetPosition(0, this.transform.position);
            LR.SetPosition(1, hit.point);
            LR.material = LineMat;
            LR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            destPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            destPointer.layer = 2;
            destPointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            destPointer.GetComponent<Renderer>().material = LineMat;
            destPointer.transform.position = hit.point;
            if (hit.transform != null && hit.transform.gameObject.tag == "floor")
            {
                LR.startColor = Color.green;
                LR.endColor = Color.green;
                validDestination = true;
            }
            else
            {
                LR.startColor = Color.red;
                LR.endColor = Color.red;
                validDestination = false;
            }
        }
        else
        {
            LR.positionCount = 0;
        }

        //Debug.Log(hit.transform.gameObject.tag);

        GetTrigger();
        if (trigger && validDestination)
        {
            
            destination = hit.point;
            destination = new Vector3(destination.x, CameraAnchor.position.y, destination.z);
            CameraAnchor.position = destination;
        }
    }

    private void GetTrigger() //da usare quando si rilascia per confermare il teletraporto
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) == true) //CAMBIARE QUI CON IL TASTO DEL VIVE DA ASSEGNARE
        {
            trigger = true;
        }
        else
        {
            trigger = false;
        }
    }
    private void GetHold() //da usare mentre si punta il raggio
    {
        //if (Input.GetKey(KeyCode.Mouse0) == true) //CAMBIARE QUI CON IL TASTO DEL VIVE DA ASSEGNARE
        if (keyToPress.action.triggered == true || Input.GetKey(KeyCode.Mouse0) == true)
        {
            Debug.Log("Presseddddddd"); //todo vedi meglio sta roba
            hold = true;
        }
        else
        {
            hold = false;
        }
    }

}
