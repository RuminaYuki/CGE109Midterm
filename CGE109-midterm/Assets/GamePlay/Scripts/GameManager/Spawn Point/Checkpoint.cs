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
            SpawnPointScript.SetSpawnPoint(transform);
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
