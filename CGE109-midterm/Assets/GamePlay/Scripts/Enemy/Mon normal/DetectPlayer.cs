using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private EnemyMovement EnemyMovementScript;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (EnemyMovementScript != null)
            {
                EnemyMovementScript.player = other.gameObject.transform;
                EnemyMovementScript.seePlayer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (EnemyMovementScript != null)
            {
                EnemyMovementScript.seePlayer = false;
            }
        }
    }
}
