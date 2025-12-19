using Gamekit3D;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{

    public MonsterManager monsterManagerScript;
    public OBJManager OBJManagerScript;
    public Transform _spawnPoint;
    public GameObject player;
    public PlayerMovement PlayerMovementScript;

    public List<GameObject> Inventory = new List<GameObject>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null )
        {
            PlayerMovementScript = player.GetComponent<PlayerMovement>();
        }
        //_spawnPoint = player.transform.position;
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            PlayerMovementScript = player.GetComponent<PlayerMovement>();
        }   
    }

    public void GotoSpawnPoint(Transform player)
    {
        if (player == null) {return; }
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(ResetToSpawn(player));
    }

    public void SetSpawnPoint(Transform spawnpoint)
    {
        _spawnPoint = spawnpoint;
        Inventory.Clear();
        Inventory.AddRange(PlayerMovementScript.Inventory);
    }


    //Reset
    public bool continued = false;
    private string continuedType;
    public void SetType_continued(string type)
    {
        continuedType = type;
    }


    public void SetBool_continued()
    {
        continued = true;
    }

    

    public IEnumerator ResetToSpawn(Transform player)
    {
        yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.GameOver));
        PlayerMovementScript.canMove = false;
        player.transform.position = _spawnPoint.position;
        player.transform.forward = _spawnPoint.forward;
        monsterManagerScript.ResetAllMonsters();
        ItemRespawnManager.Instance.ResetAllItems();
        OBJManagerScript.ResetAllOBJs();
        PlayerMovementScript.Inventory.Clear();
        PlayerMovementScript.Inventory.AddRange(Inventory);
        PlayerMovementScript.CheckItemInventory();

        yield return new WaitUntil(() => continued);
        if (continuedType == "Reset")
        {
            yield return StartCoroutine(ScreenFader.FadeSceneIn());
            Cursor.lockState = CursorLockMode.Locked;
            PlayerMovementScript.canMove = true;
        } else if (continuedType == "Return")
        {
            yield return StartCoroutine(ScreenFader.FadeSceneIn());
        }
        continued = false;
        yield break;
    }

    public void ReInventory()
    {
        if (PlayerMovementScript != null)
        {
            PlayerMovementScript.Inventory.Clear();
            PlayerMovementScript.Inventory.AddRange(Inventory);
            PlayerMovementScript.CheckItemInventory();
        }
    }
}
