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

    private bool _conditionBool;
    public bool ConditionBool
    {
        get => _conditionBool;
        set
        {
            if (_conditionBool == value) return; // ถ้าไม่เปลี่ยน ไม่ทำอะไร

            _conditionBool = value;

            if (_conditionBool)
            {
                StartActivate = true;
            }
            else
            {
                // UnParent(Player.transform); // ถูกย้ายไปอยู่ใน StopPush()

                StartActivate = false;
                StopPush(); // เรียก StopPush() แทน เพื่อจัดการทั้งหมดในฟังก์ชันเดียว
            }
        }
    }
    private void Awake()
    {
        //PlayerMoveMentScript = Player.GetComponent<PlayerMovement>();
        CharacterController = Player.GetComponent<CharacterController>();
        Rigidbody = GetComponent<Rigidbody>();

        // **แก้ไข #1:** ตั้ง Rigidbody เป็น IsKinematic เพื่อให้ควบคุมด้วย Transform ได้ง่ายขึ้น
        if (Rigidbody != null)
        {
            Rigidbody.isKinematic = true;
        }
    }

    void FixedUpdate()
    {
        if (PlayerMoveMentScript.GetIsMoveTo() == false && StartActivate)
        {
            IsPush = true;
            if (PlayerMoveMentScript.canMove) { PlayerGoToChild(); }
            if (IsPush)
            {
                // **แก้ไข #2:** ใช้ transform.position แทน Rigidbody.MovePosition
                // เพื่อหลีกเลี่ยงความขัดแย้งของฟิสิกส์กับ CharacterController ของ Player
                transform.position += PlayerMoveMentScript.GetMoveMent() * Time.fixedDeltaTime;

                // transform.SetParent(Player.transform, true); // (โค้ดเดิมที่ถูกคอมเมนต์)
                // CharacterController.Move(PlayerMoveMentScript.GetMoveMent()); // (โค้ดเดิมที่ถูกคอมเมนต์)
                // Rigidbody.MovePosition(transform.position + PlayerMoveMentScript.GetMoveMent() * Time.deltaTime); // (โค้ดเดิม)
                // Rigidbody.AddForce(PlayerMoveMentScript.GetMoveMent() * 2, ForceMode.Force); // (โค้ดเดิมที่ถูกคอมเมนต์)
            }

            // **แก้ไข #3:** ปรับปรุงเงื่อนไขการหยุดให้ง่ายขึ้นและย้ายไป Update/ใช้ FixedUpdate ได้ แต่ใช้ Mathf.Abs
            // เนื่องจากเงื่อนไขเดิมซับซ้อนเกินไป และมักเกิดปัญหา Floating Point Errors
            float dx = PushPoint.transform.position.x - Player.transform.position.x;
            float dz = PushPoint.transform.position.z - Player.transform.position.z;

            if (Mathf.Abs(dx) > 0.1f || Mathf.Abs(dz) > 0.1f) // 0.1f เป็นค่า Tolerance
            {
                StopPush();
            }

            return;
            /*velocity.y += gravity * Time.deltaTime;
            CharacterController.Move(velocity * Time.deltaTime);*/
        }

    }
    public void PlayerGoToChild()
    {
        PlayerMoveMentScript.canMove = false;

        // **แก้ไข #4:** ปิด CharacterController เมื่อเป็น Child
        if (CharacterController != null)
        {
            CharacterController.enabled = false;
        }

        Player.transform.SetParent(transform, true);
        //PlayerMoveMentScript.enabled = false; // (โค้ดเดิมที่ถูกคอมเมนต์)
        //CharacterController.enabled = false; // (โค้ดเดิมที่ถูกคอมเมนต์)
    }

    public void UnParent(Transform player)
    {
        // **แก้ไข #5:** เปิด CharacterController กลับมาเมื่อยกเลิกการเป็น Child
        if (CharacterController != null)
        {
            CharacterController.enabled = true;
        }

        //CharacterController.enabled = true; // (โค้ดเดิมที่ถูกคอมเมนต์)
        player.SetParent(null);
        //PlayerMoveMentScript.enabled = true; // (โค้ดเดิมที่ถูกคอมเมนต์)
        PlayerMoveMentScript.canMove = true;
    }
    public void StartPush(Transform GamePushPoint)
    {
        PushPoint.transform.position = GamePushPoint.transform.position;
        //PushPoint.transform.position = GetPushPoint.transform.position;
        PlayerMoveMentScript.MoveToPoint(PushPoint.transform.position);
        ConditionBool = true;
    }
    public void StopPush()
    {
        ConditionBool = false;
        IsPush = false;
        PlayerMoveMentScript.IsMoveTo = false;
        UnParent(Player.transform);
    }
}