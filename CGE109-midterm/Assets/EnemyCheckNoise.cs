using UnityEngine;
using System.Collections.Generic; // ต้องใช้สำหรับ List

public class EnemyCheckNoise : MonoBehaviour
{
    // List เก็บ Enemy ทั้งหมดที่อยู่ใน Trigger
    private List<EnemyMovement> enemiesInTrigger = new List<EnemyMovement>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMovement em = other.GetComponent<EnemyMovement>();
            if (em != null && !enemiesInTrigger.Contains(em))
            {
                enemiesInTrigger.Add(em); // เพิ่ม Enemy เข้ามาใน List
                em.m_Position = transform.position; // สั่งเดินไปยังตำแหน่ง Trigger
                Debug.Log("Enemy added and moving to trigger");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMovement em = other.GetComponent<EnemyMovement>();
            if (em != null && enemiesInTrigger.Contains(em))
            {
                enemiesInTrigger.Remove(em); // เอาออกเมื่อออก Trigger
                Debug.Log("Enemy left trigger");
            }
        }
    }

    // ถ้าต้องการสั่งทุก Enemy ใน Trigger ทำบางอย่างทุก frame
    void Update()
    {
        foreach (var em in enemiesInTrigger)
        {
            if (em != null)
            {
                em.m_Position = transform.position;
            }
        }
    }
}
