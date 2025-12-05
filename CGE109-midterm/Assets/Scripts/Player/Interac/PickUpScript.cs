using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public int WhereInInInventory;
    [SerializeField] private PlayerMovement PlayerScript;
    public GameObject Camera;

    public float throwForce = 200f;
    public GameObject heldObj;
    private Rigidbody heldObjRb;


    void Update()
    {
        if (heldObj != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ThrowObject();
            }
        }
    }

    public void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>() && PlayerScript.AddToInventory(pickUpObj))
        {
            heldObj = pickUpObj;
        }   
    }

    void ThrowObject()
    {
        if (PlayerScript.RemoveToInventory(heldObj))
        {
            GameObject Item = GameObject.Instantiate(heldObj);
            Item.transform.position = Camera.transform.position;
            heldObjRb = Item.GetComponent<Rigidbody>();
            heldObjRb.AddForce(Camera.transform.forward * throwForce);
            heldObjRb = null;
            heldObj = null;
        }
    }

}
