using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class PlayWhenPressECheckByText : MonoBehaviour
{
    [SerializeField] private UnityEvent pressE;
    [SerializeField] private GameObject text;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && text.activeInHierarchy)
        {
            pressE.Invoke();
        }
    }
}
