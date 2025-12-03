using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TS_Mainmenu : MonoBehaviour
{
    [Header("Scene เป้าหมาย")]
    [SerializeField] private string nextSceneName;

    [Header("Fade Settings")]
    [SerializeField] private float lightFadeDuration = 0.7f;
    [SerializeField] private float imageFadeDuration = 1.5f;

    [SerializeField] private Image fadeImage;  // Image สีดำเต็มจอ (alpha 0 ตอนเริ่ม)

    private List<Light> lights = new List<Light>();
    private float[] originalIntensity;

    void Awake()
    {
        // หา Fade Image อัตโนมัติ ถ้าไม่เซ็ตใน Inspector
        if (fadeImage == null)
        {
            var obj = GameObject.Find("FadeImage");
            if (obj != null) fadeImage = obj.GetComponent<Image>();
        }

        // รวบรวมไฟทุกดวงในฉาก
        lights.AddRange(FindObjectsOfType<Light>());

        originalIntensity = new float[lights.Count];
        for (int i = 0; i < lights.Count; i++)
        {
            originalIntensity[i] = lights[i].intensity;
        }
    }

    // เรียกฟังก์ชันนี้ตอนจะเปลี่ยนฉาก (เช่น จากปุ่ม Start)
    public void StartSceneFade()
    {
        StartCoroutine(FadeOutAndLoad());
    }

    private IEnumerator FadeOutAndLoad()
    {
        float time = 0f;

        float lightTime = lightFadeDuration;
        float imageTime = imageFadeDuration;

        // เวลา Fade ที่ยาวที่สุด → ใช้กำกับลูป
        float maxDuration = Mathf.Max(lightTime, imageTime);

        Color c = fadeImage.color;
        float startAlpha = c.a;  // ปกติคือ 0

        while (time < maxDuration)   // วนตามเวลาของจอดำ (ยาวสุด)
        {
            time += Time.deltaTime;

            // --- t สำหรับไฟ (ใช้เวลาสั้นกว่า → จบก่อน) ---
            float tLight = Mathf.Clamp01(time / lightFadeDuration);

            // --- t สำหรับจอดำ (ใช้เวลานานกว่า → จบทีหลัง) ---
            float tImage = Mathf.Clamp01(time / imageFadeDuration);

            // ===== Fade ไฟ (จบก่อน) =====
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i] != null)
                {
                    lights[i].intensity = Mathf.Lerp(originalIntensity[i], 0f, tLight);
                }
            }

            // ===== Fade ภาพดำ (จบทีหลัง) =====
            c.a = Mathf.Lerp(startAlpha, 1f, tImage);
            fadeImage.color = c;

            yield return null;
        }

        // --- Force ให้ทุกอย่างจบเป๊ะ ---
        for (int i = 0; i < lights.Count; i++)
            if (lights[i] != null) lights[i].intensity = 0f;

        c.a = 1f;
        fadeImage.color = c;

        // โหลด scene
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

}

