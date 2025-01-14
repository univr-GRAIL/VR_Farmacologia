using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Autohand.Demo{
    public class OpenXRTeleporterLink : MonoBehaviour {
        public Teleporter hand;
        public InputActionProperty startTeleportAction;
        public InputActionProperty finishTeleportAction;
        public InputActionProperty cancelTeleportAction;

        bool teleporting = false;

        void OnEnable() {
            if (startTeleportAction.action != null) startTeleportAction.action.Enable();
            if (startTeleportAction.action != null) startTeleportAction.action.performed += StartTeleportAction;
            if (finishTeleportAction.action != null) finishTeleportAction.action.Enable();
            if (finishTeleportAction.action != null) finishTeleportAction.action.performed += FinishTeleportAction;
            if (cancelTeleportAction.action != null) cancelTeleportAction.action.Enable();
            if (cancelTeleportAction.action != null) cancelTeleportAction.action.performed += CancelTeleportAction;
        }

        void OnDisable() {
            if (startTeleportAction.action != null) startTeleportAction.action.performed -= StartTeleportAction;
            if (finishTeleportAction.action != null) finishTeleportAction.action.performed -= FinishTeleportAction;
            if (cancelTeleportAction.action != null) cancelTeleportAction.action.performed -= CancelTeleportAction;
        }


        void StartTeleportAction(InputAction.CallbackContext a) {
            if (!teleporting) {
                hand.StartTeleport();
                teleporting = true;
            }
        }

        void FinishTeleportAction(InputAction.CallbackContext a) {
            if (teleporting) {
                hand.Teleport();
                teleporting = false;
            }
        }

        // Custom code for Farmacologia App
        // Cancel teleport when corresponding button is pressed
        void CancelTeleportAction(InputAction.CallbackContext a) {
            if(teleporting){
                hand.CancelTeleport();
                teleporting = false;
            }
        }
    }
}
