using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayRandomSound))]
public class MonsterSoundManager : MonoBehaviour
{
    private PlayRandomSound playRandomSound;

    [Header("Interval Before Next Sound")]
    [SerializeField] private Vector2 intervalRange = new Vector2(3f, 8f);

    private Coroutine soundLoop;

    private void Awake()
    {
        playRandomSound = GetComponent<PlayRandomSound>();
    }

    private void OnEnable()
    {
        soundLoop = StartCoroutine(SoundLoop());
    }

    private void OnDisable()
    {
        if (soundLoop != null)
            StopCoroutine(soundLoop);
    }

    private IEnumerator SoundLoop()
    {
        while (true)
        {
            // 1. รอสุ่มเวลาก่อนเริ่มเสียง
            float waitTime = Random.Range(intervalRange.x, intervalRange.y);
            yield return new WaitForSeconds(waitTime);

            // 2. เล่นเสียง
            playRandomSound.playWithRandom();

            // 3. รอจนกว่าเสียงจะเล่นจบจริง
            AudioSource src = playRandomSound.AudioSource;
            if (src != null)
            {
                yield return new WaitWhile(() => src.isPlaying);
            }
        }
    }

    public void StopSound()
    {
        if (soundLoop != null)
        {
            StopCoroutine(soundLoop);
            soundLoop = null;
        }

        if (playRandomSound.AudioSource != null)
            playRandomSound.AudioSource.Stop();
    }

    public void ResumeSound()
    {
        if (soundLoop == null)
            soundLoop = StartCoroutine(SoundLoop());
    }
}
