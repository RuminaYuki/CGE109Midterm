using UnityEngine;
using UnityEngine.Events;

public class PlayWhenDestroy : MonoBehaviour
{
    [SerializeField] private UnityEvent destoryed;

    private void OnDestroy()
    {
        destoryed.Invoke();
    }
}
