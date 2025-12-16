using UnityEngine;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;  // Singleton

    private List<Monster> allMonsters = new List<Monster>();

    void Awake()
    {
        // Singleton pattern: ตัวเดียวใน scene
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterMonster(Monster monster)
    {
        if (!allMonsters.Contains(monster))
        {
            allMonsters.Add(monster);
            //Debug.Log("Registered monster: " + monster.name + " Total: " + allMonsters.Count);
        }
    }

    public void ResetAllMonsters()
    {
        foreach (Monster mon in allMonsters)
        {
            if (mon != null)  // ป้องกัน null
            {
                mon.transform.position = mon.initialPosition;
                mon.transform.rotation = mon.initialRotation;
                EnemyMovement EnemyMM = mon.GetComponent<EnemyMovement>();
                if (EnemyMM != null)
                    EnemyMM.Agent.SetDestination(mon.initialPosition);
                // Reset state อื่นๆ ได้ เช่น mon.GetComponent<EnemyAI>().ResetState();
                //Debug.Log("Reset monster: " + mon.name);
            }
        }
    }
}