using System.Collections.Generic;
using UnityEngine;

public class OBJManager : MonoBehaviour
{
    public static OBJManager Instance;  // Singleton

    private List<OBJ> allOBJs = new List<OBJ>();

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

    public void RegisterOBJ(OBJ OBJ)
    {
        if (!allOBJs.Contains(OBJ))
        {
            allOBJs.Add(OBJ);
            //Debug.Log("Registered monster: " + monster.name + " Total: " + allMonsters.Count);
        }
    }

    public void ResetAllOBJs()
    {
        foreach (OBJ mon in allOBJs)
        {
            if (mon != null)  // ป้องกัน null
            {
                mon.gameObject.SetActive(true);
                Debug.Log("Reset OBJs: " + mon.name);
            }
        }
    }
}
