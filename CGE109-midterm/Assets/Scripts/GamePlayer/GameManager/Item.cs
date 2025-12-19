using UnityEngine;

public class Item : MonoBehaviour
{
    void Start()
    {
        ItemRespawnManager.Instance.RegisterItemRespawn(this.gameObject);

        //Debug.Log("Monster registered: " + gameObject.name + " at " + initialPosition);
    }
}
