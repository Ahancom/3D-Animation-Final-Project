using UnityEngine;

public class FireworkController : MonoBehaviour
{
    public ParticleSystem launchParticle;  // 发射粒子
    public GameObject firework;            // 爆炸 GameObject
    public float explosionHeight = 5f;     // 爆炸偏移高度

    private bool triggered = false;

    void Start()
    {
        triggered = false;
        launchParticle.Play();
        firework.SetActive(false); // 确保一开始是隐藏的
    }

    void Update()
    {
        if (!triggered && !launchParticle.IsAlive())
        {
            triggered = true;

            // 设置 firework 的位置为发射器位置 + 向上偏移
            Vector3 explosionPos = launchParticle.transform.position + Vector3.up * explosionHeight;
            firework.transform.position = explosionPos;

            // 激活 firework 效果
            firework.SetActive(true);
        }
    }
}
