using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpawnPoint SpawnPointScript;
    public bool DefaultPoint;

    private void Awake()
    {
        SpawnPointScript = FindObjectOfType<SpawnPoint>();
        if (SpawnPointScript != null && DefaultPoint == true)
        {
            SpawnPointScript._spawnPoint = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && SpawnPointScript != null)
        {
            SpawnPointScript.SetSpawnPoint(transform);
        }
    }
}
