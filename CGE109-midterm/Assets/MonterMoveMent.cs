using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MonterMoveMent : MonoBehaviour
{
    public float gravity = -9.81f;

    public Animator Animator;

    public Camera Camera;
    public FirstPersonController PlayerCam;

    public CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 velocity;
    public bool IsMoveTo;
    private Vector3 MoveTo;
    public Transform Point;
    public Transform Point2;
    public Transform PointTarget;
    private Vector3 Move;
    public float speed = 11f;

    public Vector3 StartPoint;
    private void Awake()
    {
        StartPoint = transform.position;
    }
    private void Update()
    {
        //Camera.transform.LookAt(transform.position);
        if (IsMoveTo && controller != null)
        {
            MoveTo = (PointTarget.transform.position - transform.position);
            MoveTo.y = 0f;
            MoveTo.Normalize();
            controller.Move(MoveTo * speed * Time.deltaTime);
            //print(Point + "" + transform.position);
            if (PointTarget.position.x - transform.position.x < 0.02f && PointTarget.position.z - transform.position.z < 0.02f && PointTarget.position.x - transform.position.x > -0.02f && PointTarget.position.z - transform.position.z > -0.02f)
            {
                print("Stop");
                IsMoveTo = false;
            }
        }

    }

    public void Active()
    {
        PointTarget = Point;
        IsMoveTo = true;
        //Animator.enabled = true;
        StartCoroutine(LookAtAfterDelay());

    }

    private IEnumerator LookAtAfterDelay()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        //PlayerCam = Camera.GetComponent<PlayerCam>();
        PlayerCam.enabled = false;
        Camera.transform.LookAt(transform.position);
        yield return new WaitForSeconds(0.5f);
        Animator.SetBool("Growl", true);
        Animator.enabled = true;
        playerMovement.canMove = false;

        yield return new WaitForSeconds(0.6f);
        PlayerCam.enabled = true;
        playerMovement.canMove = true;

        yield return new WaitForSeconds(1f);
        Animator.SetBool("Growl", false);
        PointTarget = Point2;
        speed = 3.8f;
        IsMoveTo = true;
    }
}
