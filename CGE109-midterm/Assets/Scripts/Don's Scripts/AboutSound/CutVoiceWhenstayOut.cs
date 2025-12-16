using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutVoiceWhenstayOut : MonoBehaviour
{
    [SerializeField] private List<AudioLowPassFilter> lowPassFliter;
    [SerializeField] private float cutoffDefault = 5000f;
    [SerializeField] private float fade = 0.5f;

    private Coroutine fadeRoutine;

    private void Start()
    {
        // เริ่มต้นให้ "อู้" (ตัดแหลมหนัก)
        foreach (var l in lowPassFliter)
        {
            if (l != null) l.cutoffFrequency = 10f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // เข้า Area -> กลับเป็นปกติ
        StartFadeAll(10f, cutoffDefault);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // ออก Area -> อู้
        StartFadeAll(cutoffDefault, 10f);
    }

    private void StartFadeAll(float from, float to)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeCutoffAll(from, to));
    }

    private IEnumerator FadeCutoffAll(float from, float to)
    {
        float elapsed = 0f;

        // กันหารศูนย์
        float duration = Mathf.Max(0.0001f, fade);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float current = Mathf.Lerp(from, to, t);

            foreach (var l in lowPassFliter)
            {
                if (l != null) l.cutoffFrequency = current;
            }

            elapsed += Time.deltaTime;
            yield return null; // สำคัญมาก ไม่งั้นค้าง
        }

        // จบให้เป๊ะ
        foreach (var l in lowPassFliter)
        {
            if (l != null) l.cutoffFrequency = to;
        }
    }
}
