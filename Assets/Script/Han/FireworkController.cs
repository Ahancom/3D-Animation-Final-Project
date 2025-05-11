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
        // �ȶ����¼�
        timeline.stopped += OnTimelineStopped;
    }

    void Start()
    {
        // �ֶ����� Timeline
        timeline.Play();
    }

    void OnTimelineStopped(PlayableDirector pd)
    {
        // Timeline ��������󣬲ſ�ʼ�̻�����
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
