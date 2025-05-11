using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class BlinkTransition : MonoBehaviour
{
    [Header("���� & ����")]
    [Tooltip("����գ��Ч���Ĳ��ʣ������ _Radius ����")]
    public Material blinkMaterial;
    [Tooltip("������ȫ��Ҫ���صĳ��������������л�")]
    public string nextSceneName;

    [Header("��������")]
    [Tooltip("��һ�ΰ뿪�׶�ʱ�� (0��midOpenRadius)")]
    public float openHalfDuration = 0.4f;
    [Tooltip("�ڶ���ȫ���׶�ʱ�� (0��1)")]
    public float openFullDuration = 0.6f;
    [Range(0, 1)]
    [Tooltip("�뿪ʱ�� _Radius ��С (0 ��ȫ�գ�1 ��ȫ��)")]
    public float midOpenRadius = 0.5f;

    [Header("����")]
    [Tooltip("��һ�ΰ�ս׶�ʱ�� (1��midCloseRadius)")]
    public float closeHalfDuration = 0.4f;
    [Tooltip("�ڶ���ȫ�ս׶�ʱ�� (1��0)")]
    public float closeFullDuration = 0.6f;
    [Range(0, 1)]
    [Tooltip("���ʱ�� _Radius ��С (0 ��ȫ�գ�1 ��ȫ��)")]
    public float midCloseRadius = 0.5f;

    [Header("Timeline ����")]
    [Tooltip("������Ϻ󴥷�����")]
    public PlayableDirector triggerTimeline;

    private void OnEnable()
    {
        if (triggerTimeline != null)
            triggerTimeline.stopped += OnTimelineStopped;
    }

    private void OnDisable()
    {
        if (triggerTimeline != null)
            triggerTimeline.stopped -= OnTimelineStopped;
    }

    private void Start()
    {
        // ��ʼΪ����״̬
        blinkMaterial.SetFloat("_Radius", 0f);
        // ���ſ���գ��
        Open();
    }

    private void OnTimelineStopped(PlayableDirector pd)
    {
        // ���Ž����󴥷�����
        Close();
    }

    /// <summary>
    /// ���������ۡ��������뿪���رա�ȫ����
    /// </summary>
    public void Open()
    {
        StopAllCoroutines();
        StartCoroutine(OpenSequence());
    }

    /// <summary>
    /// ���������ۡ���������ա��ؿ���ȫ�գ����������г���
    /// </summary>
    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(CloseSequence());
    }

    private IEnumerator OpenSequence()
    {
        yield return Animate(0f, midOpenRadius, openHalfDuration);
        yield return Animate(midOpenRadius, 0f, openHalfDuration);
        yield return Animate(0f, 1f, openFullDuration);
    }

    private IEnumerator CloseSequence()
    {
        yield return Animate(1f, midCloseRadius, closeHalfDuration);
        yield return Animate(midCloseRadius, 1f, closeHalfDuration);
        yield return Animate(1f, 0f, closeFullDuration);

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Animate(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            blinkMaterial.SetFloat("_Radius", to);
            yield break;
        }

        float elapsed = 0f;
        blinkMaterial.SetFloat("_Radius", from);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            blinkMaterial.SetFloat("_Radius", Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        blinkMaterial.SetFloat("_Radius", to);
    }

    // ���ʹ��������Ⱦ���ߣ�������·������� Blit
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (blinkMaterial != null)
            Graphics.Blit(src, dest, blinkMaterial);
        else
            Graphics.Blit(src, dest);
    }
}
