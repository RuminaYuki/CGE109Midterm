using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTrigger;
    [SerializeField] private bool playOnce;
    private bool isplayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playOnce)
            {
                if (!isplayed)
                {
                    OnTrigger.Invoke();
                    isplayed = true;
                }
            }
            else
            {
                OnTrigger.Invoke();
            }
        }
    }
}
