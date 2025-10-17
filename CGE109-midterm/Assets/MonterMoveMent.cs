using System.Collections;
using System.Drawing;
using UnityEngine;

public class MonterMoveMent : MonoBehaviour
{
    public float gravity = -9.81f;

    public Animator Animator;

    public Camera Camera;
    public PlayerCam PlayerCam;

    private CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 velocity;

    private bool IsMoveTo;

    private void Update()
    {
        //Camera.transform.LookAt(transform.position);

        velocity.y += gravity * Time.deltaTime;
        if (controller != null)
        {
            controller.Move(velocity * Time.deltaTime);
        }
    }

    public void Active()
    {
        Animator.enabled = true;
        StartCoroutine(LookAtAfterDelay());

    }

    private IEnumerator LookAtAfterDelay()
    {

        PlayerCam = Camera.GetComponent<PlayerCam>();
        PlayerCam.enabled = false;



        Camera.transform.LookAt(transform.position);

        yield return new WaitForSeconds(1.2f);

        PlayerCam.enabled = true;
    }
}
