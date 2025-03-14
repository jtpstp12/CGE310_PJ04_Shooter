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

        // สมัคร event OnGameEnd เพื่อปิดปืนเมื่อเกมจบ
        GameManager.instance.OnGameEnd += DisableGun;
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnGameEnd -= DisableGun;
    }

    void Update()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if (animator != null)
            animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime);

        if (animator != null)
            animator.SetBool("Reloading", false);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (animator != null)
            animator.SetTrigger("Gunrecoil");

        RaycastHit hit;
        currentAmmo--;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            ZombieHealth zombieHealth = hit.transform.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);
            }

            if (hit.transform.CompareTag("Zombie"))
            {
                if (zombieImpactEffect != null)
                    Instantiate(zombieImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                if (impactEffect != null)
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    void DisableGun()
    {
        gameObject.SetActive(false); // ปิดปืนเมื่อเกมจบ
    }
}
