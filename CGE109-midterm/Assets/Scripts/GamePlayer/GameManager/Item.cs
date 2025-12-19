using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemRespawnManager RespawnManager;

    private void Awake()
    {
        RespawnManager = FindObjectOfType<ItemRespawnManager>();
    }
    void Start()
    {
        RespawnManager.RegisterItemRespawn(this.gameObject);
    }
}
