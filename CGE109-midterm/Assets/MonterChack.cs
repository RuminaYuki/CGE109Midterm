using System.Collections;
using UnityEngine;

public class MonterChack : MonoBehaviour
{
    public MonterMoveMent MonterMoveMentScript;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MonterMoveMentScript.Active();
        }
    }
    
    

}
