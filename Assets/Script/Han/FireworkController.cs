using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class FireworkController : MonoBehaviour
{
    public PlayableDirector timeline;
    [System.Serializable]
    public class Entry { public ParticleSystem firework; public float delay; }
    public Entry[] fireworks;

    void Awake()
    {
        // 先订阅事件
        timeline.stopped += OnTimelineStopped;
    }

    void Start()
    {
        // 手动启动 Timeline
        timeline.Play();
    }

    void OnTimelineStopped(PlayableDirector pd)
    {
        // Timeline 真正播完后，才开始烟花序列
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        foreach (var e in fireworks)
        {
            yield return new WaitForSeconds(e.delay);
            e.firework?.Play();
        }
    }

    void OnDestroy()
    {
        timeline.stopped -= OnTimelineStopped;
    }
}
