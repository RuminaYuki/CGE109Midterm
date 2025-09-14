using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Vector3 MoveDirection;
    public Transform orientation;
    private Rigidbody rb;

    float horizontalInput, verticalInput;

    public float groundDrag;

    public float playheight;
    public LayerMask Environment;
    bool grounded;

    public CapsuleCollider Collider;

    private float speed = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, (playheight * 0.5f) + 0.9f, Environment);
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else if (!grounded) { rb.linearDamping = 0; }
        Debug.Log(rb.linearDamping);
        Debug.Log(grounded);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        MoveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(MoveDirection.normalized * moveSpeed * 10f * speed, ForceMode.Force);
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limittedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limittedVel.x, rb.linearVelocity.y, limittedVel.z);
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //Collider.center = new Vector3(0, .5f, 0); 
            Collider.height = 1f;
            speed = 0.75f;
        }
        else
        {
            //Collider.center = new Vector3(0, 0, 0);
            Collider.height = 2f;
            speed = 1f;
        }


    }
}
