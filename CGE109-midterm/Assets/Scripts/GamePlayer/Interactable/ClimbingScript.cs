using UnityEngine;
using System.Collections;

public class ClimbingScript : MonoBehaviour
{
    public Transform pointStart;
    public float startTime = 0f;
    public Transform pointUp;
    public float uptime = 0.5f;
    public Transform pointEnd;
    public float endtime = 0.25f;
    public GameObject Player;
    public PlayerMovement PlayerMovementScript;
    public CharacterController PlayerCharacterController;

    public bool Active = false;

    private void Update()
    {
        if (PlayerMovementScript != null)
        {
            if (!PlayerMovementScript.IsMoveTo && Active)
            {
                Active = false;
                Debug.Log("Here Climb");
                PlayerMovementScript.useMoveCharacter = false;
                StartCoroutine(DelayedAction());
            }
        }
    }

    public void Climbing(GameObject PlayerObj)
    {
        Player = PlayerObj;
        PlayerMovementScript = PlayerObj.GetComponent<PlayerMovement>();
        PlayerCharacterController = PlayerObj.GetComponent<CharacterController>();

        if (pointStart != null && pointUp != null && pointEnd != null)
        {
            PlayerMovementScript.MoveToPoint(pointStart.position);
            Active = true;
            return;
        }

        Debug.Log("PointStart, pointUp or pointEnd is null");
    }

    IEnumerator DelayedAction()
    {
        
        // 1. ย่อตัวลง
        yield return StartCoroutine(ChangeHeight(PlayerCharacterController, 1.5f, 1.0f, startTime));
        print("Down");

        // 2. เคลื่อนไป pointUp พร้อมค่อยๆ ยืดตัวขึ้น
        yield return StartCoroutine(MoveAndGrow(Player.transform, pointUp.position, PlayerCharacterController, 1.0f, 1.5f, uptime));
        print("Move to Up");

        // 3. เคลื่อนไป pointEnd (บนสุด)
        yield return StartCoroutine(MovePlayer(Player.transform, pointEnd.position, endtime));
        print("Move to End");

        PlayerMovementScript.useMoveCharacter = true;
        
        print("Done!");
    }

    IEnumerator ChangeHeight(CharacterController controller, float start, float end, float duration)
    {
        
        float time = 0f;
        while (time < duration)
        {
            PlayerMovementScript.useGravity();
            controller.height = Mathf.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        controller.height = end;
    }

    IEnumerator MoveAndGrow(Transform player, Vector3 target, CharacterController controller, float startHeight, float endHeight, float duration)
    {
        Vector3 startPos = player.position;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            player.position = Vector3.Lerp(startPos, target, t);      // เคลื่อนไป pointUp
            controller.height = Mathf.Lerp(startHeight, endHeight, t); // ค่อยๆ ยืดตัว
            time += Time.deltaTime;
            yield return null;
        }

        player.position = target;
        controller.height = endHeight;
    }

    IEnumerator MovePlayer(Transform player, Vector3 target, float duration)
    {
        Vector3 startPos = player.position;
        float time = 0f;

        while (time < duration)
        {
            player.position = Vector3.Lerp(startPos, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        player.position = target;
    }
}
