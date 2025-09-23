using UnityEngine;
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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
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
        controller.Move(moveDirection * moveSpeed * speedMultiplier * Time.deltaTime);

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
}
