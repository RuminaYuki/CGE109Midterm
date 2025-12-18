using UnityEngine;
using UnityEngine.Events;

public class PlayWhenPressECheckByActive : MonoBehaviour
{
    [SerializeField] UnityEvent press;
    [SerializeField] GameObject reference;
    bool played = false;

    private void Update()
    {
        if (played) return;
        if(reference.activeSelf == false)
        {
            press.Invoke();
            played = true;
        }
    }
}
