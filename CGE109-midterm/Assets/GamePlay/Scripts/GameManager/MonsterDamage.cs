using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public SpawnPoint spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spawnPoint.GotoSpawnPoint(other.transform);
        }

    }
}
