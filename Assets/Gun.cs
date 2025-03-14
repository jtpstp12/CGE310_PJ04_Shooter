using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.2f;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Animator animator;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject zombieImpactEffect;

    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // ถ้า reload อยู่ให้ return ออกไปก่อน
        if (isReloading) return;

        // ถ้ากระสุนหมด reload อัตโนมัติ
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // ถ้ากดปุ่ม R และกระสุนยังไม่เต็ม ให้ reload ด้วย
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        // ยิงปืน
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    // Coroutine สำหรับ reload
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // เล่น animation reload
        if (animator != null)
            animator.SetBool("Reloading", true); // เปิดอนิเมชั่น reload

        // รอเวลาตาม reloadTime
        yield return new WaitForSeconds(reloadTime);

        // หยุด animation reload
        if (animator != null)
            animator.SetBool("Reloading", false); // ปิดอนิเมชั่น reload

        // เติมกระสุนใหม่
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    // ฟังก์ชันยิง
    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (animator != null)
            animator.SetTrigger("Gunrecoil"); // เล่นอนิเมชั่น recoil

        RaycastHit hit;
        currentAmmo--; // ลดกระสุน

        // Ray ยิงออกไป
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // ถ้าโดน Zombie
            ZombieHealth zombieHealth = hit.transform.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);
            }

            // Effect โดน Zombie
            if (hit.transform.CompareTag("Zombie"))
            {
                if (zombieImpactEffect != null)
                    Instantiate(zombieImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                // Effect โดนพื้น/กำแพง
                if (impactEffect != null)
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
