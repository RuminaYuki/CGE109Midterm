using Gamekit3D;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SpawnPoint : MonoBehaviour
{

    public MonsterManager monsterManagerScript;
    public Vector3 _spawnPoint;
    private GameObject player;
    private PlayerMovement PlayerMovementScript; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null )
        {
            PlayerMovementScript = player.GetComponent<PlayerMovement>();
        }
        _spawnPoint = player.transform.position;
        DontDestroyOnLoad(gameObject);
    }

    public void GotoSpawnPoint(Transform player)
    {
        Debug.Log("GotoSpawnPoint");
        if (player == null) 
        {
            Debug.Log("player == null");
            return; 
        }
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(ResetToSpawn(player));
    }

    public void SetSpawnPoint(Transform spawnpoint)
    {
        _spawnPoint = spawnpoint.position;
    }


    //Reset
    private bool continued = false;
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
        Debug.Log("ResetToSpawn");
        yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.GameOver));
        PlayerMovementScript.canMove = false;
        player.transform.position = _spawnPoint;
        monsterManagerScript.ResetAllMonsters();

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
}
