using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemRespawnManager : MonoBehaviour
{
    public List<GameObject> allItems = new List<GameObject>();
    public SpawnPoint SpawnPoint;

    private string CurrentScene;

    void Awake()
    {
        CurrentScene = SceneManager.GetActiveScene().name;
        SpawnPoint = FindObjectOfType<SpawnPoint>();
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
