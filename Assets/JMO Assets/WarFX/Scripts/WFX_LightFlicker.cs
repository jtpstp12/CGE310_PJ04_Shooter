using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
    public float time = 0.05f;
    public ParticleSystem muzzleFlash; // เพิ่มตัวแปรสำหรับ Particle System
    private float timer;
    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>(); // ดึง Light Component
        myLight.enabled = false; // ปิดไฟตอนเริ่ม
        timer = time;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // เช็คว่า muzzleFlash กำลังเล่นอยู่หรือไม่
            if (muzzleFlash != null && muzzleFlash.isPlaying)
            {
                myLight.enabled = !myLight.enabled; // กระพริบไฟ
            }
            else
            {
                myLight.enabled = false; // ปิดไฟถ้า muzzleFlash ไม่ทำงาน
            }

            // รอเวลาก่อนเปลี่ยนสถานะไฟ
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
