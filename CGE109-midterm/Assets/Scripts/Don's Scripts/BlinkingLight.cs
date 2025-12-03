using System.Collections;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    [SerializeField] private GameObject lightObj;
    [SerializeField] private AudioSource audioSource;

    [Header("เสียงเปิด / ปิด ไฟ (ไฟล์เดียว)")]
    [SerializeField] private AudioClip turnOnSound;
    [SerializeField] private AudioClip turnOffSound;

    [Header("Delay ระหว่างชุดกระพริบแต่ละรอบ")]
    [SerializeField] private float minGapBetweenPatterns = 0.5f;
    [SerializeField] private float maxGapBetweenPatterns = 2f;

    private void Awake()
    {
        if (lightObj == null)
        {
            lightObj = FindChildByName(transform, "Spot Light");
            if (lightObj == null)
                Debug.LogError("❌ ไม่พบ Spot Light ใต้ " + gameObject.name);
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private GameObject FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child.gameObject;

            GameObject found = FindChildByName(child, name);
            if (found != null)
                return found;
        }
        return null;
    }

    private void Start()
    {
        StartCoroutine(FlickerLoop());
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            int patternIndex = Random.Range(0, 3);

            switch (patternIndex)
            {
                case 0: yield return Pattern_ShortTriple(); break;
                case 1: yield return Pattern_LongDrop(); break;
                case 2: yield return Pattern_Nervous(); break;
            }

            float gap = Random.Range(minGapBetweenPatterns, maxGapBetweenPatterns);
            yield return new WaitForSeconds(gap);
        }
    }

    IEnumerator Pattern_ShortTriple()
    {
        yield return Flick(false, 0.05f);
        yield return Flick(true, 0.08f);
        yield return Flick(false, 0.04f);
        yield return Flick(true, 0.10f);
        yield return Flick(false, 0.06f);
        yield return Flick(true, 0.15f);
    }

    IEnumerator Pattern_LongDrop()
    {
        yield return Flick(false, 0.6f);
        yield return Flick(true, 0.15f);
        yield return Flick(false, 0.1f);
        yield return Flick(true, 0.3f);
    }

    IEnumerator Pattern_Nervous()
    {
        for (int i = 0; i < 6; i++)
        {
            yield return Flick(false, Random.Range(0.02f, 0.08f));
            yield return Flick(true, Random.Range(0.03f, 0.12f));
        }
    }

    IEnumerator Flick(bool on, float duration)
    {
        lightObj.SetActive(on);

        if (on)
            PlayClip(turnOnSound);
        else
            PlayClip(turnOffSound);   // เดิมคุณเผลอใช้ turnOnSound ซ้ำ

        yield return new WaitForSeconds(duration);
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.PlayOneShot(clip);
    }
}
