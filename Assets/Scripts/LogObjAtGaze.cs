using Tobii.G2OM;
using UnityEngine;

//Monobehaviour which implements the "IGazeFocusable" interface, meaning it will be called on when the object receives focus
public class LogObjAtGaze : MonoBehaviour, IGazeFocusable
{
    //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus
    public void GazeFocusChanged(bool hasFocus)
    {
        //If this object received focus, call the log to write the current gazed obj
        if (hasFocus)
        {
            LogManager.Instance.GazeOn(this.gameObject);
        }
        //If this object lost focus, remove from the current gazed obj
        else
        {
            LogManager.Instance.GazeOff(this.gameObject);
        }
    }
}
