using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
    [Tooltip("哪个 Timeline 播放完毕后触发闭眼动画")]
    public PlayableDirector triggerTimeline;

    private void Start()
    {
        // 开场——先黑屏（完全闭眼）
        blinkMaterial.SetFloat("_Radius", 0f);
        // 启动开场序列
        StartCoroutine(OpenSequence());
        // 监听 Timeline 结束
        if (triggerTimeline != null)
            triggerTimeline.stopped += _ => StartCoroutine(CloseSequence());
    }

    private void OnDestroy()
    {
        if (triggerTimeline != null)
            triggerTimeline.stopped -= _ => StartCoroutine(CloseSequence());
    }

    /// <summary>
    /// 开场睁眼：0→midOpen→0→1
    /// </summary>
    private IEnumerator OpenSequence()
    {
        // 第一次半开：0 → midOpenRadius
        yield return Animate(0f, midOpenRadius, openHalfDuration);
        // 回黑：midOpenRadius → 0
        yield return Animate(midOpenRadius, 0f, openHalfDuration);
        // 第二次全开：0 → 1
        yield return Animate(0f, 1f, openFullDuration);
    }

    /// <summary>
    /// 结尾闭眼：1→midClose→1→0，然后切场景
    /// </summary>
    private IEnumerator CloseSequence()
    {
        // 第一次半闭：1 → midCloseRadius
        yield return Animate(1f, midCloseRadius, closeHalfDuration);
        // 回开：midCloseRadius → 1
        yield return Animate(midCloseRadius, 1f, closeHalfDuration);
        // 第二次全闭：1 → 0
        yield return Animate(1f, 0f, closeFullDuration);

        // 切场景
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// 通用插值，将 _Radius 从 from→to 用 duration 秒
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
