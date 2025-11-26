using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public MonsterManager monsterManagerScript;
    public Vector3 _spawnPoint;
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _spawnPoint = player.position;
    }

    public void GotoSpawnPoint(Transform player)
    {
        player.position = _spawnPoint;
        monsterManagerScript.ResetAllMonsters();

    }

    public void SetSpawnPoint(Transform spawnpoint)
    {
        _spawnPoint = spawnpoint.position;
    }
}
