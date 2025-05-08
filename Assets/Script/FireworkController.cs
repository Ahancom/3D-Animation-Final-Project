using UnityEngine;

public class FireworkController : MonoBehaviour
{
    public ParticleSystem launchParticle;  // ��������
    public GameObject firework;            // ��ը GameObject
    public float explosionHeight = 5f;     // ��ըƫ�Ƹ߶�

    private bool triggered = false;

    void Start()
    {
        triggered = false;
        launchParticle.Play();
        firework.SetActive(false); // ȷ��һ��ʼ�����ص�
    }

    void Update()
    {
        if (!triggered && !launchParticle.IsAlive())
        {
            triggered = true;

            // ���� firework ��λ��Ϊ������λ�� + ����ƫ��
            Vector3 explosionPos = launchParticle.transform.position + Vector3.up * explosionHeight;
            firework.transform.position = explosionPos;

            // ���� firework Ч��
            firework.SetActive(true);
        }
    }
}
