using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent Agent;

    public Vector3 m_Position;

    private void Awake()
    {       
        Agent = GetComponent<NavMeshAgent>();
        m_Position = Agent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Agent.SetDestination(m_Position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Noise"))
        {
            m_Position = other.transform.position; // สั่งเดินไปยังตำแหน่ง Trigger
            Debug.Log("Enemy added and moving to trigger");
            
        }
    }
}
