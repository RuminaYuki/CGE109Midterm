using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public SpawnPoint spawnPoint;
    public MonterMoveMent MonterMoveMent;
    private void Start()
    {
        spawnPoint = FindObjectOfType<SpawnPoint>();
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            spawnPoint.GotoSpawnPoint(other.transform);
            if (MonterMoveMent != null)
            {
                MonterMoveMent.Animator.enabled = false;
                MonterMoveMent.IsMoveTo = false;
                MonterMoveMent.StopAllCoroutines();
                MonterMoveMent.speed = 11f;
                //MonterMoveMent.PointTarget.position = MonterMoveMent.StartPoint;
                MonterMoveMent.gameObject.transform.position = MonterMoveMent.StartPoint;
            }
        }

    }
}
