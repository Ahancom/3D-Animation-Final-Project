using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
    [Tooltip("�ĸ� Timeline ������Ϻ󴥷����۶���")]
    public PlayableDirector triggerTimeline;

    private void Start()
    {
        // ���������Ⱥ�������ȫ���ۣ�
        blinkMaterial.SetFloat("_Radius", 0f);
        // ������������
        StartCoroutine(OpenSequence());
        // ���� Timeline ����
        if (triggerTimeline != null)
            triggerTimeline.stopped += _ => StartCoroutine(CloseSequence());
    }

    private void OnDestroy()
    {
        if (triggerTimeline != null)
            triggerTimeline.stopped -= _ => StartCoroutine(CloseSequence());
    }

    /// <summary>
    /// �������ۣ�0��midOpen��0��1
    /// </summary>
    private IEnumerator OpenSequence()
    {
        // ��һ�ΰ뿪��0 �� midOpenRadius
        yield return Animate(0f, midOpenRadius, openHalfDuration);
        // �غڣ�midOpenRadius �� 0
        yield return Animate(midOpenRadius, 0f, openHalfDuration);
        // �ڶ���ȫ����0 �� 1
        yield return Animate(0f, 1f, openFullDuration);
    }

    /// <summary>
    /// ��β���ۣ�1��midClose��1��0��Ȼ���г���
    /// </summary>
    private IEnumerator CloseSequence()
    {
        // ��һ�ΰ�գ�1 �� midCloseRadius
        yield return Animate(1f, midCloseRadius, closeHalfDuration);
        // �ؿ���midCloseRadius �� 1
        yield return Animate(midCloseRadius, 1f, closeHalfDuration);
        // �ڶ���ȫ�գ�1 �� 0
        yield return Animate(1f, 0f, closeFullDuration);

        // �г���
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// ͨ�ò�ֵ���� _Radius �� from��to �� duration ��
    /// </summary>
    private IEnumerator Animate(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            blinkMaterial.SetFloat("_Radius", to);
            yield break;
        }

        float t = 0f;
        blinkMaterial.SetFloat("_Radius", from);
        while (t < duration)
        {
            t += Time.deltaTime;
            blinkMaterial.SetFloat("_Radius", Mathf.Lerp(from, to, t / duration));
            yield return null;
        }
        blinkMaterial.SetFloat("_Radius", to);
    }
}
