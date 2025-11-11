using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
    public GameObject Particle;
    public GameObject MeshObj;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null) 
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                MeshObj.SetActive(false);
                Particle.transform.parent = null;
                Particle.transform.localRotation = Quaternion.Euler(-90, 0, 0); ;
                Particle.SetActive(true);
                StartCoroutine(DelayedAction(0.7f));
                
            }
        }
    }

    IEnumerator DelayedAction(float time)
    {
        yield return new WaitForSeconds(time); // Wait for 2 seconds (scaled)
        Destroy(Particle);
        Destroy(gameObject);
    }
}
