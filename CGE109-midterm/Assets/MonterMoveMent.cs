using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class MonterMoveMent : MonoBehaviour
{
    public float gravity = -9.81f;

    public Animator Animator;

    public Camera Camera;
    public FirstPersonController PlayerCam;

    public CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private bool IsMoveTo;
    private Vector3 MoveTo;
    public Transform Point;
    public Transform Point2;
    private Vector3 Move;
    public float speed = 11f;

    private void Update()
    {
        //Camera.transform.LookAt(transform.position);
        if (IsMoveTo && controller != null)
        {
            MoveTo = (Point.transform.position - transform.position);
            MoveTo.y = 0f;
            MoveTo.Normalize();
            controller.Move(MoveTo * speed * Time.deltaTime);
            //print(Point + "" + transform.position);
            if (Point.position.x - transform.position.x < 0.02f && Point.position.z - transform.position.z < 0.02f && Point.position.x - transform.position.x > -0.02f && Point.position.z - transform.position.z > -0.02f)
            {
                print("Stop");
                IsMoveTo = false;
            }
        }

    }

    public void Active()
    {
        IsMoveTo = true;
        //Animator.enabled = true;
        StartCoroutine(LookAtAfterDelay());

    }

    private IEnumerator LookAtAfterDelay()
    {

        //PlayerCam = Camera.GetComponent<PlayerCam>();
        PlayerCam.enabled = false;
        Camera.transform.LookAt(transform.position);
        yield return new WaitForSeconds(0.5f);
        Animator.SetBool("Growl", true);
        Animator.enabled = true;

        yield return new WaitForSeconds(0.6f);

        PlayerCam.enabled = true;

        yield return new WaitForSeconds(0.3f);
        Animator.SetBool("Growl", false);
        Point = Point2;
        speed = 3.5f;
        IsMoveTo = true;
    }
}
