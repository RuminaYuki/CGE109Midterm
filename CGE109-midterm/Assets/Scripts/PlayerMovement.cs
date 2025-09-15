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

    public float maxSlopeAngle;
    private RaycastHit slopeHit;

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
        grounded = Physics.Raycast(transform.position, Vector3.down, (playheight * 0.5f) + 1.25f, Environment);
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else if (!grounded) { rb.linearDamping = 0; }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Moveplayer();
        MoveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        
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

    private void Moveplayer()
    {
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 5f * speed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(MoveDirection.normalized * moveSpeed * 10f * speed, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playheight*0.5f + 1.25f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(MoveDirection, slopeHit.normal).normalized;
    }
}
