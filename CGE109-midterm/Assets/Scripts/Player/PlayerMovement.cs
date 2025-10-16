using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float groundDrag = 5f;

    public Transform orientation;          // บอกทิศทางการเคลื่อนที่ (ตามกล้อง)
    public Transform groundCheck;          // จุดเช็คพื้น (วางไว้ใต้ตัวละคร)
    public float groundDistance = 0.4f;    // รัศมีเช็คพื้น
    public LayerMask environmentMask;      // เลเยอร์ที่ถือว่าเป็นพื้น

    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 velocity;              // ใช้เก็บแรงโน้มถ่วง

    private bool grounded;
    private float speedMultiplier = 1f;

    public bool IsMoveTo;
    private Vector3 MoveTo;
    private Vector3 Point;
    private Vector3 Move;

    public GameObject[] Inventory;
    private int i = 0;

    public GameObject Flashlight;
    public bool FlashlightOn;
    public bool KeyCard;
    public bool KeyCard2;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Flashlight.SetActive(false);
    }

    void Update()
    {
        // ✅ เช็คชนพื้น + เลเยอร์
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, environmentMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f; // รีเซ็ตค่าเวลาติดพื้น
        }

        // ✅ รับ input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize();

        // ✅ เคลื่อนที่
        if (!IsMoveTo)
        {
            Move = moveDirection * moveSpeed * speedMultiplier ;
            controller.Move(Move * Time.deltaTime);
        }
        else if (IsMoveTo)
        {
            MoveTo = (Point - transform.position);
            MoveTo.y = 0f;
            MoveTo.Normalize();
            controller.Move(MoveTo * moveSpeed * speedMultiplier * Time.deltaTime);
            print(Point + "" + transform.position);
            if (Point.x - transform.position.x < 0.009f && Point.z - transform.position.z < 0.009f && Point.x - transform.position.x > -0.009f && Point.z - transform.position.z > -0.009f)
            {
                print("Stop");
                IsMoveTo = false;
            }
        }

        // ✅ กดนั่ง (Crouch)
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.localScale = new Vector3(1f, 0.4f, 1f);
            speedMultiplier = 0.75f;
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            speedMultiplier = 1f;
        }

        // ✅ กดวิ่ง (Sprint)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speedMultiplier = 2f;
        }

        // ✅ กระโดด
        if (grounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // ✅ ใช้แรงโน้มถ่วง
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
        Debug.Log(MoveTo);
    }

    public Vector3 GetMoveMent() 
    {
        return Move;
    }

    public bool GetIsMoveTo()
    {
        return IsMoveTo;
    }
    
    public void AddToInventory(GameObject pickUpObj)
    {
        bool AddSup = false;
        i = 0;
        while (AddSup)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = pickUpObj;
                AddSup = true;
                Debug.Log(Inventory);
                return;
            }
            i++;
        }
    }

    public int GetNumItemInInventory() 
    {
        return i;
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
