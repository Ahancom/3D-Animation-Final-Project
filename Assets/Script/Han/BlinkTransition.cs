using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class BlinkTransition : MonoBehaviour
{
    [Header("材质 & 场景")]
    [Tooltip("用于眨眼效果的材质，需包含 _Radius 属性")]
    public Material blinkMaterial;
    [Tooltip("闭眼完全后要加载的场景名，留空则不切换")]
    public string nextSceneName;

    [Header("开场睁眼")]
    [Tooltip("第一次半开阶段时长 (0→midOpenRadius)")]
    public float openHalfDuration = 0.4f;
    [Tooltip("第二次全开阶段时长 (0→1)")]
    public float openFullDuration = 0.6f;
    [Range(0, 1)]
    [Tooltip("半开时的 _Radius 大小 (0 完全闭，1 完全开)")]
    public float midOpenRadius = 0.5f;

    [Header("闭眼")]
    [Tooltip("第一次半闭阶段时长 (1→midCloseRadius)")]
    public float closeHalfDuration = 0.4f;
    [Tooltip("第二次全闭阶段时长 (1→0)")]
    public float closeFullDuration = 0.6f;
    [Range(0, 1)]
    [Tooltip("半闭时的 _Radius 大小 (0 完全闭，1 完全开)")]
    public float midCloseRadius = 0.5f;

    [Header("Timeline 触发")]
    [Tooltip("播放完毕后触发闭眼")]
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
        // 初始为闭眼状态
        blinkMaterial.SetFloat("_Radius", 0f);
        // 播放开场眨眼
        Open();
    }

    private void OnTimelineStopped(PlayableDirector pd)
    {
        // 播放结束后触发闭眼
        Close();
    }

    /// <summary>
    /// 触发“睁眼”动画（半开→回闭→全开）
    /// </summary>
    public void Open()
    {
        StopAllCoroutines();
        StartCoroutine(OpenSequence());
    }

    /// <summary>
    /// 触发“闭眼”动画（半闭→回开→全闭），结束后切场景
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

    // 如果使用内置渲染管线，添加以下方法进行 Blit
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (blinkMaterial != null)
            Graphics.Blit(src, dest, blinkMaterial);
        else
            Graphics.Blit(src, dest);
    }
}
