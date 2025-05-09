using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlinkTransition : MonoBehaviour
{
    [Header("Material")]
    [Tooltip("Material")]
    public Material blinkMaterial;
    [Tooltip("Next Scene")]
    public string nextSceneName = "";

    [Header("First Blink")]
    public float firstBlinkDuration = 0.6f;
    [Tooltip("Radius")]
    [Range(0, 1)]
    public float midRadius = 0.5f;

    [Header("Second Blink")]
    public float secondBlinkDuration = 0.6f;

    public void StartBlink()
    {
        StopAllCoroutines();
        StartCoroutine(BlinkSequence());
    }

    private IEnumerator BlinkSequence()
    {
        yield return SimpleBlink(firstBlinkDuration, midRadius);

        yield return FullBlinkAndLoad(secondBlinkDuration, nextSceneName);
    }

    private IEnumerator SimpleBlink(float totalDuration, float mid)
    {
        float half = totalDuration / 2f;
        yield return Animate(1f, mid, half);
        yield return Animate(mid, 1f, half);
    }

    private IEnumerator FullBlinkAndLoad(float totalDuration, string scene)
    {
        float half = totalDuration / 2f;
        yield return Animate(1f, 0f, half);

        if (!string.IsNullOrEmpty(scene))
            SceneManager.LoadScene(scene);

        yield return Animate(0f, 1f, half);
    }

    private IEnumerator Animate(float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float r = Mathf.Lerp(from, to, t / time);
            blinkMaterial.SetFloat("_Radius", r);
            yield return null;
        }
        blinkMaterial.SetFloat("_Radius", to);
    }

    private void Start()
    {
        StartBlink();
    }
}
