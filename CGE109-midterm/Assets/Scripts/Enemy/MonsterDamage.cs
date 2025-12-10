using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public SpawnPoint spawnPoint;
    private void Start()
    {
        spawnPoint = FindObjectOfType<SpawnPoint>();
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("if (other.gameObject.CompareTag(\"Player\"))");
            spawnPoint.GotoSpawnPoint(other.transform);
        }

    }
}
