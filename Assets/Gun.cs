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
        // ��� reload ������� return �͡仡�͹
        if (isReloading) return;

        // ��ҡ���ع��� reload �ѵ��ѵ�
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // ��ҡ����� R ��С���ع�ѧ������ ��� reload ����
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        // �ԧ�׹
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    // Coroutine ����Ѻ reload
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // ��� animation reload
        if (animator != null)
            animator.SetBool("Reloading", true); // �Դ͹������ reload

        // �����ҵ�� reloadTime
        yield return new WaitForSeconds(reloadTime);

        // ��ش animation reload
        if (animator != null)
            animator.SetBool("Reloading", false); // �Դ͹������ reload

        // �������ع����
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    // �ѧ��ѹ�ԧ
    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (animator != null)
            animator.SetTrigger("Gunrecoil"); // ���͹������ recoil

        RaycastHit hit;
        currentAmmo--; // Ŵ����ع

        // Ray �ԧ�͡�
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // ���ⴹ Zombie
            ZombieHealth zombieHealth = hit.transform.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);
            }

            // Effect ⴹ Zombie
            if (hit.transform.CompareTag("Zombie"))
            {
                if (zombieImpactEffect != null)
                    Instantiate(zombieImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                // Effect ⴹ���/��ᾧ
                if (impactEffect != null)
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
