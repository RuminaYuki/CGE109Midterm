using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpawnPoint SpawnPointScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnPointScript.SetSpawnPoint(transform);
        }
    }
}
