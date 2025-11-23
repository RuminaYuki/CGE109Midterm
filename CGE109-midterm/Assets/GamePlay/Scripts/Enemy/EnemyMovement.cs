using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent Agent;
    public float rotationSpeed = 2f;

    private Vector3 m_Position;
    private Vector3 m_Direction;
    private Quaternion targetRotation;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Noise"))
        {
            Agent.SetDestination(transform.position);
            m_Position = other.transform.position;
            m_Direction = m_Position - transform.position;
            m_Direction.y = 0f;

            targetRotation = Quaternion.LookRotation(m_Direction);

            StartCoroutine(LookTarget());
        }
    }

    private IEnumerator LookTarget()
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * 100f * Time.deltaTime
            );

            yield return null;
        }

        Agent.SetDestination(m_Position);
    }
}
