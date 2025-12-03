using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;
    private static readonly int IsHover = Animator.StringToHash("IsHover");

    void Reset()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool(IsHover, true);     // เมาส์วางบนปุ่ม
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool(IsHover, false);    // เมาส์ออกจากปุ่ม
    }
}
