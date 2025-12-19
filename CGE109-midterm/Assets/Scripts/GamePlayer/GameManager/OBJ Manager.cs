using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class OBJManager : MonoBehaviour
{
    public static OBJManager Instance;  // Singleton

    private List<GameObject> allOBJs = new List<GameObject>();

    public InteractableShowLabel interactableShowLabel;
    public SpawnPoint spawnPoint;

    void Awake()
    {
        spawnPoint = FindObjectOfType<SpawnPoint>();
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

    public void RegisterOBJ(GameObject OBJ)
    {
        if (!allOBJs.Contains(OBJ))
        {
            allOBJs.Add(OBJ);
            //Debug.Log("Registered monster: " + monster.name + " Total: " + allMonsters.Count);
        }
    }

    public void ResetAllOBJs()
    {
        foreach (GameObject mon in allOBJs)
        {
            if (mon != null)  // ป้องกัน null
            {
                interactableShowLabel = mon.GetComponentInChildren<InteractableShowLabel>();

                Debug.Log("here");
                foreach (GameObject item in spawnPoint.Inventory)
                {
                    Debug.Log(interactableShowLabel.Item.name + item.name);
                    if (interactableShowLabel.Item.name == item.name)
                    {
                        Debug.Log("here");
                        mon.gameObject.SetActive(false);
                        return;
                    }
                    mon.gameObject.SetActive(true);
                }
            }
        }
    }
}
