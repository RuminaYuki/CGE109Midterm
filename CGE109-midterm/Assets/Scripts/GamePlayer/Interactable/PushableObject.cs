using UnityEngine;

public class PushableObject : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerMovement PlayerMoveMentScript;
    [SerializeField] public GameObject PushPoint;
    [SerializeField] private LayerMask wallMask; // เพิ่มสำหรับตรวจว่าข้างหน้ามีกำแพงไหม

    private CharacterController CharacterController;
    private Rigidbody Rigidbody;

    public bool StartActivate = false;
    public bool IsPush = false;

    public float pushSpeed = 2.5f;
    public float checkDistance = 0.6f;

    private bool _conditionBool;
    public bool ConditionBool
    {
        get => _conditionBool;
        set
        {
            if (_conditionBool == value) return;

            _conditionBool = value;

            if (_conditionBool)
            {
                StartActivate = true;
            }
            else
            {
                StartActivate = false;
                StopPush();
            }
        }
    }

    private void Awake()
    {
        CharacterController = Player.GetComponent<CharacterController>();
        Rigidbody = GetComponent<Rigidbody>();

        // ✅ ให้ Rigidbody ไม่เป็น Kinematic เพื่อให้ชนกำแพงได้
        if (Rigidbody != null)
        {
            Rigidbody.isKinematic = false;
            //Rigidbody.constraints = RigidbodyConstraints.FreezeRotation; // กันล้ม
        }
    }

    private void FixedUpdate()
    {
        if (!StartActivate) return;

        // ถ้า Player ไปถึงจุด Push แล้ว
        if (!PlayerMoveMentScript.GetIsMoveTo())
        {
            IsPush = true;
            if (PlayerMoveMentScript.canMove)
                PlayerGoToChild();

            if (IsPush)
            {
                Vector3 moveDir = PlayerMoveMentScript.GetMoveMent();

                // ✅ ตรวจว่าด้านหน้ามีกำแพงไหมก่อนเข็น
                if (!IsWallAhead(moveDir))
                {
                    Vector3 newPos = Rigidbody.position + moveDir * pushSpeed * Time.fixedDeltaTime;
                    Rigidbody.MovePosition(newPos);
                }
                else
                {
                    // ถ้ามีกำแพง ให้หยุดขยับ
                    StopPush();
                }
            }

            // ถ้า Player หลุดจาก PushPoint มากเกินไป ให้หยุดเข็น
            float dx = PushPoint.transform.position.x - Player.transform.position.x;
            float dz = PushPoint.transform.position.z - Player.transform.position.z;
            if (Mathf.Abs(dx) > 0.2f || Mathf.Abs(dz) > 0.2f)
            {
                StopPush();
            }
        }
    }

    private bool IsWallAhead(Vector3 dir)
    {
        // ✅ ยิง Raycast จากตัว object ไปข้างหน้า เพื่อตรวจว่าชนกำแพงหรือยัง
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, dir.normalized, checkDistance, wallMask);
    }

    public void PlayerGoToChild()
    {
        PlayerMoveMentScript.canMove = false;

        if (CharacterController != null)
            CharacterController.enabled = false;

        Player.transform.SetParent(transform, true);
    }

    public void UnParent(Transform player)
    {
        if (CharacterController != null)
            CharacterController.enabled = true;

        player.SetParent(null);
        PlayerMoveMentScript.canMove = true;
    }

    public void StartPush(Transform GamePushPoint)
    {
        PushPoint.transform.position = GamePushPoint.transform.position;
        PlayerMoveMentScript.MoveToPoint(PushPoint.transform.position);
        ConditionBool = true;
        PlayerMoveMentScript.canRun = false;
        Rigidbody.constraints &= ~RigidbodyConstraints.FreezePosition;
    }

    public void StopPush()
    {
        ConditionBool = false;
        IsPush = false;
        PlayerMoveMentScript.IsMoveTo = false;
        UnParent(Player.transform);
        PlayerMoveMentScript.canRun = true;
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
