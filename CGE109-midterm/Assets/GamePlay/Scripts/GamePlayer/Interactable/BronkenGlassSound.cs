using UnityEngine;

public class BronkenGlassSound : MonoBehaviour
{
    //[SerializeField] private MeshCollider objCollider;
    [SerializeField] private Collider noiseCollider;
    [SerializeField] private float activeTime = 0.3f;

    private void Awake()
    {
        noiseCollider.enabled = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🦶 Glass broken - Noise emitted!");
            noiseCollider.enabled = true;
            Invoke(nameof(DisableNoise), activeTime);
        }
    }

    private void DisableNoise()
    {
        noiseCollider.enabled = false;
        Debug.Log("🦶 Glass broken - Off");
    }
}
