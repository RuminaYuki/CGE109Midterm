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
        if (other.gameObject.CompareTag("Player"))
        {
            spawnPoint.GotoSpawnPoint(other.transform);
        }

    }
}
