using UnityEngine;
using System.Collections;

public class FireworkController : MonoBehaviour
{
    [Tooltip("start delay")]
    public float startAfterSeconds = 10f;

    [System.Serializable]
    public class Entry
    {
        public ParticleSystem firework;
        [Tooltip("delay£©")]
        public float delay;
    }

    public Entry[] fireworks;

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(startAfterSeconds);

        foreach (var e in fireworks)
        {
            yield return new WaitForSeconds(e.delay);
            e.firework?.Play();
        }
    }
}
