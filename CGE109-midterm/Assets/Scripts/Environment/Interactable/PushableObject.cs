using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class PushableObject : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerMovement PlayerMoveMentScript;
    [SerializeField] public GameObject PushPoint;

    private CharacterController CharacterController;
    private Rigidbody Rigidbody;

    public bool StartActivate = false;
    public bool IsPush = false;

    public float gravity = -9.81f;
    private Vector3 velocity;
    private void Awake()
    {
        //PlayerMoveMentScript = Player.GetComponent<PlayerMovement>();
        CharacterController = GetComponent<CharacterController>();
        Rigidbody = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        if (PlayerMoveMentScript.GetIsMoveTo() == false && StartActivate)
        {
            IsPush = true;
            if (IsPush)
            {
                //transform.SetParent(Player.transform, true);
                //CharacterController.Move(PlayerMoveMentScript.GetMoveMent());
                Rigidbody.MovePosition(transform.position + PlayerMoveMentScript.GetMoveMent()*8.5f * Time.deltaTime);
                //Rigidbody.AddForce(PlayerMoveMentScript.GetMoveMent() * 2, ForceMode.Force);

            }
            if (PushPoint.transform.position.x - Player.transform.position.x > 0.02f && PushPoint.transform.position.z - Player.transform.position.z > 0.02f 
                || PushPoint.transform.position.x - Player.transform.position.x < -0.02f && PushPoint.transform.position.z - Player.transform.position.z < -0.02f
                || PushPoint.transform.position.x - Player.transform.position.x > 0.02f && PushPoint.transform.position.z - Player.transform.position.z < -0.02f
                || PushPoint.transform.position.x - Player.transform.position.x < -0.02f && PushPoint.transform.position.z - Player.transform.position.z > 0.02f)
            {
                StopPush();
            }


            /*velocity.y += gravity * Time.deltaTime;
            CharacterController.Move(velocity * Time.deltaTime);*/
        }
        
    }

    public void StartPush(Transform GamePushPoint)
    {
        PushPoint.transform.position = GamePushPoint.transform.position;
        //PushPoint.transform.position = GetPushPoint.transform.position;
        PlayerMoveMentScript.MoveToPoint(PushPoint.transform.position);
        StartActivate = true;
    }
    public void StopPush()
    {
        StartActivate = false;
        IsPush = false;
        PlayerMoveMentScript.IsMoveTo = false;
    }
}
