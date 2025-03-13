using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
    public float time = 0.05f;
    public ParticleSystem muzzleFlash; // �������������Ѻ Particle System
    private float timer;
    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>(); // �֧ Light Component
        myLight.enabled = false; // �Դ俵͹�����
        timer = time;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // ����� muzzleFlash ���ѧ��������������
            if (muzzleFlash != null && muzzleFlash.isPlaying)
            {
                myLight.enabled = !myLight.enabled; // ��о�Ժ�
            }
            else
            {
                myLight.enabled = false; // �Դ俶�� muzzleFlash ���ӧҹ
            }

            // �����ҡ�͹����¹ʶҹ��
            do
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            while (timer > 0);

            timer = time;
        }
    }
}
