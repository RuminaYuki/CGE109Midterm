using Gamekit3D.GameCommands;
using TMPro;
using UnityEngine;
//using UnityEngine.Windows;

public class InteractableShowLabel : MonoBehaviour
{
    [SerializeField] private bool _onFogus = false;

    [SerializeField] private GameObject text;
    public float UpperY = 0.5f;

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

    public bool NeedKeyCard1;
    public bool NeedKeyCard2;


    void Update()
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
        _onFogus = false;
    }

    public void OnFogusByPlayer()
    {
        _onFogus = true;
    }

    private void Event()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            //print("HiF");
            if (_keyCode == KeyCode.E)
            {
                if (Item != null && ItemForThrow && _pickUpScript != null)
                {
                    _pickUpScript.PickUpObject(Item);
                    //print("Hi");
                    return;
                }
                if (IsItem) 
                {
                    if (bgsScript != null)
                    {
                        bgsScript.OnTriggerEnter(ColliderObj);
                    }
                    if (!PlayerMovementScript.FlashlightOn)
                    {
                        PlayerMovementScript.SetFlashlight();
                        DestroyAfterActivate();
                        return;
                    }
                    if (!PlayerMovementScript.KeyCard)
                    {
                        //Debug.Log("Pick up item!!");
                        PlayerMovementScript.SetKeyCard();
                        DestroyAfterActivate();
                        return;
                    }
                    if (!PlayerMovementScript.KeyCard2)
                    {
                        //Debug.Log("Pick up item!!");
                        PlayerMovementScript.SetKeyCard2();
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
                    if (PushableObjectScript.StartActivate == true)
                    {
                        PushableObjectScript.StopPush();
                        return;
                    }
                    if (PushableObjectScript.IsPush == false)
                    {
                        PushableObjectScript.PushPoint = this.gameObject;
                        PushableObjectScript.StartPush(PushPoint);
                        
                        return;
                    }
                }
                
                Debug.Log("Nothing");
                return;
            }
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
}
