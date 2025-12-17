using System.Collections.Generic;
using System.Linq;
using Gamekit3D;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    //public static PlayerMovement instance;

    public float moveSpeed = 5f;
    public float groundDrag = 5f;

    public Transform orientation;          // บอกทิศทางการเคลื่อนที่ (ตามกล้อง)
    public Transform groundCheck;          // จุดเช็คพื้น (วางไว้ใต้ตัวละคร)
    public float groundDistance = 0.4f;    // รัศมีเช็คพื้น
    public LayerMask environmentMask;      // เลเยอร์ที่ถือว่าเป็นพื้น

    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    public CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 velocity;              // ใช้เก็บแรงโน้มถ่วง

    private bool grounded;
    private float speedMultiplier = 1f;

    public bool IsMoveTo;
    private Vector3 MoveTo;
    private Vector3 Point;
    private Vector3 Move;
    public bool canMove = true;
    public bool canRun = true;

    public List<GameObject> Inventory = new List<GameObject>();
    [SerializeField] private Transform HeldPosition;
    public GameObject HeldObj;
    public GameObject CameraHolder;

    public GameObject Flashlight;
    public bool FlashlightOn;
    public bool KeyCard;
    public bool KeyCard2;

    public bool useMoveCharacter = true;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        controller = GetComponent<CharacterController>();
        if (!FlashlightOn)
        {
            Flashlight.SetActive(false);
        }
    }

    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, environmentMask);
        if (grounded && velocity.y < 0)
        {
            velocity.y = -3f;
        }

        if (controller != null && useMoveCharacter)
        {
            MoveCharacter();
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.localScale = new Vector3(1f, 0.4f, 1f);
            speedMultiplier = 0.75f;
            canRun = false;
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            speedMultiplier = 1f;
            canRun = true;
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && canRun)
        {
            speedMultiplier = 2f;
        }

        /*if (grounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }*/

        
    }

    public void MoveCharacter()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize();

        if (!IsMoveTo)
        {
            Move = moveDirection * moveSpeed * speedMultiplier;
            if (canMove) { controller.Move(Move * Time.deltaTime); }
        }
        else if (IsMoveTo && canMove)
        {
            MoveTo = (Point - transform.position);
            MoveTo.y = 0f;
            MoveTo.Normalize();
            controller.Move(MoveTo * moveSpeed * speedMultiplier * Time.deltaTime);
            //print(Point + "" + transform.position);
            if (Point.x - transform.position.x < 0.02f && Point.z - transform.position.z < 0.02f && Point.x - transform.position.x > -0.02f && Point.z - transform.position.z > -0.02f)
            {
                print("Stop");
                IsMoveTo = false;
            }
        }
        if (canMove)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    public void useGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void MoveToPoint(Vector3 PointToGo)
    {
        IsMoveTo = true;
        Point = PointToGo;
        MoveTo = (PointToGo - transform.position);
        MoveTo.y = 0f;
        MoveTo.Normalize();
        //Debug.Log(MoveTo);
    }

    public Vector3 GetMoveMent()
    {
        return Move;
    }

    public bool GetIsMoveTo()
    {
        return IsMoveTo;
    }

    public bool AddToInventory(GameObject pickUpObj)
    {
        ItemPickUp IPU = pickUpObj.GetComponent<ItemPickUp>();
        if (IPU.ItemData.itemType == ItemType.Normal)
        {
            if (!Inventory.Contains(pickUpObj))
            {
                Inventory.Add(pickUpObj);
                if (pickUpObj.name == "FlashLight") SetFlashlight();
                if (pickUpObj.name == "KeyCard 1") SetKeyCard();
                if (pickUpObj.name == "KeyCard2") SetKeyCard2();
                return false;
            }
        }


        if (!Inventory.Contains(pickUpObj))
        {
            Inventory.Add(pickUpObj);
            HeldObj = GameObject.Instantiate(IPU.ItemData.gameObj, HeldPosition.transform.position, CameraHolder.transform.rotation);
            /*Rigidbody rb = HeldObj.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ |
                             RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationY |
                             RigidbodyConstraints.FreezePositionX |
                             RigidbodyConstraints.FreezePositionY |
                             RigidbodyConstraints.FreezePosition;*/
            HeldObj.transform.SetParent(HeldPosition);
            return true;
        }
        return false;
    }

    public bool RemoveToInventory(GameObject pickUpObj)
    {

        if (Inventory.Contains(pickUpObj))
        {
            Inventory.Remove(pickUpObj);
            if (HeldObj != null)
            {
                Destroy(HeldObj);
            }
            return true;
        }
        return false;
    }

    public void SetFlashlight()
    {
        Flashlight.SetActive(true);
        FlashlightOn = true;
    }

    public void SetKeyCard()
    {
        KeyCard = true;
    }

    public void SetKeyCard2()
    {
        KeyCard2 = true;
    }
}
