using UnityEngine;

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
                Destroy(this.gameObject, 0.5f);
            }
        }
    }
}
