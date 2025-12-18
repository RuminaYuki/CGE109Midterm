using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpawnPoint SpawnPointScript;
    public bool DefaultPoint;

    private void Awake()
    {
        SpawnPointScript = FindObjectOfType<SpawnPoint>();
        if (SpawnPointScript != null && DefaultPoint)
        {
            SpawnPointScript._spawnPoint = transform.position;
            SpawnPoint spawnPoint = FindAnyObjectByType<SpawnPoint>();
            spawnPoint.ReInventory();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && SpawnPointScript != null && DefaultPoint)
        {
            SpawnPoint spawnPoint = FindAnyObjectByType<SpawnPoint>();
            spawnPoint.ReInventory();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && SpawnPointScript != null && !DefaultPoint)
        {
            SpawnPointScript.SetSpawnPoint(transform);
        }
    }
}
