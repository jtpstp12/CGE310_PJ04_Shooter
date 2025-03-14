using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.2f; // ค่าหน่วงระหว่างการยิงแต่ละครั้ง (ต่ำกว่านี้ยิงเร็วขึ้น)

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Animator animator;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f; // ตัวแปรเก็บเวลาถัดไปที่สามารถยิงได้
    

    void Start ()
    {
        if (currentAmmo == -1)
            currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        // กดค้างเพื่อยิง และต้องรอให้ถึงเวลา fireRate ก่อนถึงจะยิงได้
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate; // กำหนดเวลาถัดไปที่ยิงได้
            Shoot();
        }
    }

    IEnumerator Reload ()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime);

        animator.SetBool("Reloading", false);

        currentAmmo = maxAmmo;
        isReloading = false;
    }


    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (animator != null)
            animator.SetTrigger("Gunrecoil"); // เล่น Animation แรงดีด

        RaycastHit hit;
        currentAmmo--;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (impactEffect != null)
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
