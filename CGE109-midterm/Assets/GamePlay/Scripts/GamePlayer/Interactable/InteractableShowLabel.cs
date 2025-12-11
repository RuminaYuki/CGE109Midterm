using Gamekit3D.GameCommands;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Windows;

public class InteractableShowLabel : MonoBehaviour
{
    [Header("Mode")]
    [SerializeField] private bool _onFogus = false;
    [SerializeField] private GameObject text;
    public float UpperY = 0.5f;
    public bool onTriggerMode = false;

    public KeyCode _keyCode;
    public bool OneTimeActivateOnly;
    public Outline OutlineScript;
    public GameObject GameObj;
    public PlayerMovement PlayerMovementScript;

    public bool IsItem = false;
    public bool ItemForThrow = false;
    [SerializeField] private GameObject Item;
    [SerializeField] private PickUpScript _pickUpScript;

    [SerializeField] private SimpleTranslatorController SimpleTranslatorControllerScript;
    [SerializeField] private Transform PushPoint;
    [SerializeField] private PushableObject PushableObjectScript;
    [SerializeField] private Collider ColliderObj;
    [SerializeField] private BronkenGlassSound bgsScript;

    [SerializeField] private ClimbingScript climbingScript;

    public bool NeedKeyCard1;
    public bool NeedKeyCard2;

    private bool _onTriggerStay = false;

    void Update()
    {
        if (text != null)
        {
            if (_onFogus)
            {
                if (PushableObjectScript != null) 
                {
                    if (PushableObjectScript.IsPush == false)
                    {
                        text.SetActive(_onFogus);
                    }
                    else { text.SetActive(false); }
                    Event();
                } else
                {
                    Event();
                    text.SetActive(_onFogus);

                }
            }
            else if (!_onFogus)
            {
                text.SetActive(_onFogus);
            }
        }
        if (_onTriggerStay && onTriggerMode)
        {
            Event();
        }

        _onFogus = false;
    }

    public void OnFogusByPlayer()
    {
        if (text != null) 
        {
            _onFogus = true;
        }
    }

    private void Event()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            //print("HiF");
            if (_keyCode == KeyCode.E)
            {
            }
            if (Item != null && ItemForThrow && _pickUpScript != null)
            {
                _pickUpScript.PickUpObject(Item);
                return;
            }
            if (IsItem) 
            {
                if (bgsScript != null)
                {
                    bgsScript.OnTriggerEnter(ColliderObj);
                }
                if (PlayerMovementScript != null)
                {
                    PlayerMovementScript.AddToInventory(Item);
                    DestroyAfterActivate();
                    return;
                }
            } 
            if (SimpleTranslatorControllerScript != null) 
            {
                if (!NeedKeyCard1 && !NeedKeyCard2) 
                {
                    SimpleTranslatorControllerScript.activate();
                    if (OneTimeActivateOnly)
                    {
                        DestroyAfterActivate();
                    }
                    return;
                } else if (PlayerMovementScript != null)
                {
                    if (NeedKeyCard1)
                    {
                        if (PlayerMovementScript.KeyCard)
                        {
                            SimpleTranslatorControllerScript.activate();
                            if (OneTimeActivateOnly)
                            {
                                DestroyAfterActivate();
                            }
                            return;
                        }
                    }
                    if (NeedKeyCard2)
                    {
                        if (PlayerMovementScript.KeyCard2)
                        {
                            SimpleTranslatorControllerScript.activate();
                            if (OneTimeActivateOnly)
                            {
                                DestroyAfterActivate();
                            }
                            return;
                        }
                    }
                }
            }
            if (PushableObjectScript != null)
            {
                
                if (PushableObjectScript.IsPush == false)
                {
                    PushableObjectScript.PushPoint = this.gameObject;
                    //print("here " + PushableObjectScript.StartActivate);
                    PushableObjectScript.StartPush(PushPoint);
                        
                    return;
                }
                if (PushableObjectScript.StartActivate == true)
                {
                    //print("Stop");
                    PushableObjectScript.StopPush();
                    return;
                }
            }
            if (climbingScript != null && GameObj != null)
            {
                climbingScript.Climbing(GameObj);
            }
            Debug.Log("Nothing" + IsItem);
            return;
        }
        return;
    }

    private void DestroyAfterActivate()
    {
        if (OutlineScript != null)
        {
            OutlineScript.enabled = false;
            this.gameObject.SetActive(false);
        }
        if (GameObj != null)
        {
            Destroy(this.gameObject);
            Destroy(GameObj);
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _onTriggerStay = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _onTriggerStay = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
