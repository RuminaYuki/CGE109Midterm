using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditRoll : MonoBehaviour
{
    public float speed = 50f;           // ความเร็วในการเลื่อน (หน่วย: pixel ต่อวินาที)
    public float endY = 1000f;          // ค่า Y ที่ถือว่าจบเครดิตแล้ว
    //public string nextSceneName = "MainMenu"; // Scene ที่จะไปต่อ เช่น เมนูหลัก

    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        // ขยับตำแหน่งขึ้นทีละนิดทุกเฟรม
        rt.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // ถ้าเลื่อนเกินตำแหน่งที่กำหนด (จอด้านบน) ให้เปลี่ยน Scene
        /*  if (rt.anchoredPosition.y >= endY)
          {
              // เปลี่ยนเป็นโหลด Scene ใหม่ หรือ Quit เกมก็ได้
              SceneManager.LoadScene(nextSceneName);
              // หรือถ้าอยากออกเกมเลย: Application.Quit();
          }
        */
    }
}