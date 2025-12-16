using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ClimbingScript))]
public class PlaySoundWhenClimb : MonoBehaviour
{
    [SerializeField] private UnityEvent onActivated;
    [SerializeField] private float colddownRun = 0.5f;

    private ClimbingScript climbingScript;
    private bool lastActive;

    private void Start()
    {
        climbingScript = GetComponent<ClimbingScript>();
    }

    private void Update()
    {
        bool currentActive = climbingScript.Active;

        // เล่นแค่ตอน false -> true
        if (currentActive && !lastActive)
        {
            StartCoroutine(ColddownRun());
        }

        lastActive = currentActive;
    }

    IEnumerator ColddownRun()
    {
        yield return new WaitForSeconds(colddownRun);
        onActivated.Invoke();
    }
}