using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("NavMeshAgent")]
    public NavMeshAgent Agent;
    public float rotationSpeed = 2f;
    public float moveSpeed = 2f;

    [Header("Detect Player")]
    public bool seePlayer = false;
    public Transform player;
    public Transform raycastTransform;
    public LayerMask obstructionMask;
    public float detectRadius = 10f;
    public float detectAngle = 60f;

    private Vector3 m_Position;
    private Vector3 m_Direction;
    private Quaternion targetRotation;

    public bool isLocking;

    [Header("Animation")]
    public Animator animator;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = moveSpeed;
    }

    private void Update()
    {
        DetectPlayer();
        animator.SetFloat("Velocity", Agent.velocity.sqrMagnitude);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Noise") && !seePlayer)
        {
            StartCoroutine(LookTarget(other.transform));
            animator.SetTrigger("Is Turn around");
            return;
        }
    }



    private IEnumerator LookTarget(Transform player)
    {
        isLocking = true;
        Agent.SetDestination(transform.position);
        m_Position = player.transform.position;
        m_Direction = m_Position - transform.position;
        m_Direction.y = 0f;

        targetRotation = Quaternion.LookRotation(m_Direction);


        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * 100f * Time.deltaTime
            );

            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        Agent.SetDestination(m_Position);
        yield return new WaitForSeconds(0.1f);
        isLocking = false;
    }

    private void DetectPlayer()
    {
        seePlayer = false;
        if (!player)
        {
            Debug.Log("!player");
            return;
        }


        Vector3 directionToPlayer = (player.position - raycastTransform.position).normalized;
        float distanceToPlayer = Vector3.Distance(raycastTransform.position, player.position);

        if (distanceToPlayer <= detectRadius)
        {
            Debug.Log("distanceToPlayer <= detectRadius");
            float angle = Vector3.Angle(raycastTransform.forward, directionToPlayer);

            if (angle <= detectAngle / 2)
            {
                Debug.Log("angle <= detectAngle / 2");
                if (!Physics.Raycast(raycastTransform.position, directionToPlayer, distanceToPlayer, obstructionMask))
                {
                    animator.SetTrigger("Is Turn around");
                    seePlayer = true;
                }
            }
        }
        if (seePlayer && !isLocking)
        {
            if (Agent.velocity.sqrMagnitude == 0f)
            {
                StartCoroutine(LookTarget(player));
                return;
            }
            Agent.SetDestination(player.position);
            
        }

    }
    private void OnDrawGizmosSelected()
    {
        if (!raycastTransform) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(raycastTransform.position, detectRadius);
        Vector3 right = Quaternion.Euler(0, detectAngle / 2, 0) * raycastTransform.forward * detectRadius;
        Vector3 left = Quaternion.Euler(0, -detectAngle / 2, 0) * raycastTransform.forward * detectRadius;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastTransform.position, raycastTransform.position + right);
        Gizmos.DrawLine(raycastTransform.position, raycastTransform.position + left);
    }
}
