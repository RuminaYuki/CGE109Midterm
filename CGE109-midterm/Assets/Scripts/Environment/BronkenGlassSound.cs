using UnityEngine;

public class BronkenGlassSound : MonoBehaviour
{
    //[SerializeField] private MeshCollider objCollider;
    //[SerializeField] private Collider noiseCollider;
    [SerializeField] private float activeTime = 0.3f;

    private void Awake()
    {
        //noiseCollider.enabled = false;
    }
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
            //noiseCollider.enabled = true;
            Debug.Log("🦶 Glass broken - Noise emitted!");
            Invoke(nameof(DisableNoise), activeTime);
        }
    }

    private void DisableNoise()
    {
        //noiseCollider.enabled = false;
        Debug.Log("🦶 Glass broken - Off");
    }
}
