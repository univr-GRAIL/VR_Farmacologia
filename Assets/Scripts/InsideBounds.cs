using UnityEngine;

public class InsideBounds : MonoBehaviour
{
    // Detects manually if obj is being seen by the main camera

    GameObject obj;
    Collider objCollider;

    Camera cam;
    Plane[] planes;

    void Start()
    {
        obj = this.gameObject;
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        objCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            Debug.Log(obj.name + " has been detected!");
        }
        else
        {
            Debug.Log("Nothing has been detected");
        }
    }
}