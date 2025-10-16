using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamekit3D.GameCommands
{

    public class SendOnTriggerEnter : TriggerCommand
    {
        public LayerMask layers;
        public PlayerMovement playerMovementScript;
        public SimpleTranslatorController SimpleTranslatorControllerScript;
        public GameObject KeyCardScanner1;
        public GameObject KeyCardScanner2;
        private bool active;
        public bool IsCleanning;

        void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.CompareTag("Player") && !active)
            {
                //Send();
                print(other.name);
                if (KeyCardScanner1 != null)
                {
                    KeyCardScanner1.SetActive(false);
                }
                if (KeyCardScanner2 != null) 
                {
                    KeyCardScanner2.SetActive(false);
                }
                
                playerMovementScript.MoveToPoint(transform.position);
                
                IsCleanning = true;



            }
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (playerMovementScript.IsMoveTo == false && !active)
            {
                SimpleTranslatorControllerScript.activate();
                active = true;
            }
            if (!SimpleTranslatorControllerScript.active) 
            {
                IsCleanning = false ;
            }
            if (SimpleTranslatorControllerScript.active)
            {
                if (KeyCardScanner1 != null)
                {
                    KeyCardScanner1.SetActive(false);
                }
                if (KeyCardScanner2 != null)
                {
                    KeyCardScanner2.SetActive(false);
                }
                IsCleanning = true;
            }
            if (!IsCleanning)
            {
                if (KeyCardScanner1 != null)
                {
                    KeyCardScanner1.SetActive(true);
                }
                if (KeyCardScanner2 != null)
                {
                    KeyCardScanner2.SetActive(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            active = false;
        }
    }
}
