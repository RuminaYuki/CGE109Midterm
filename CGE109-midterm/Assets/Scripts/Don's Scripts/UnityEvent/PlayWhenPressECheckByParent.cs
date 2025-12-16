using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(AudioSource))]
public class PlaySoundWhenPressECheckByParent : MonoBehaviour
{
    [SerializeField] private UnityEvent pressE;
    [SerializeField] private Transform parent;
    [SerializeField] private int calParent; //เช็คว่า parent มีกี่ตัวแล้วค่อยเล่น
    bool isplayed;

    void Update()
    {
        if (parent == null) return;

        if (parent.transform.childCount <= calParent && !isplayed) //เช็คว่า parent มีกี่ตัวแล้วค่อยเล่น
        {
            pressE.Invoke();
            isplayed = true;
        }
    }
}
