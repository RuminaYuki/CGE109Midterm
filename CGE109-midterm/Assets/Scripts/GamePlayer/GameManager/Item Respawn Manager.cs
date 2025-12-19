using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemRespawnManager : MonoBehaviour
{
    public static ItemRespawnManager Instance;  // Singleton

    public List<GameObject> allItems = new List<GameObject>();
    public SpawnPoint SpawnPoint;

    void Awake()
    {
        SpawnPoint = FindObjectOfType<SpawnPoint>();
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

    public void RegisterItemRespawn(GameObject item)
    {
        if (!allItems.Contains(item))
        {
            allItems.Add(item);
            //Debug.Log("Registered monster: " + monster.name + " Total: " + allMonsters.Count);
        }
    }

    public void ResetAllItems()
    {
        foreach (GameObject item in allItems)
        {
            InteractableShowLabel ItTb = item.GetComponentInChildren<InteractableShowLabel>();
            if (item != null && SpawnPoint != null && ItTb != null)  // ป้องกัน null
            {
                foreach (GameObject Nameitem in SpawnPoint.Inventory)
                {
                    Debug.Log(Nameitem.name + " " + ItTb.Item.name);
                    if (Nameitem.name == ItTb.Item.name)
                    {
                        return;
                    }
                }
                item.SetActive(true);
            }
        }
    }
}
