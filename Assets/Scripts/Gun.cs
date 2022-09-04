using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;

    float timeSinceLastShot;

    public GameObject shotPrefab;
    public TextMeshProUGUI ammoUI;
    GameObject laser;
    RaycastHit hit;

    public Animator anim;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        AmmoText(gunData.currentAmmo);
    }

    public void StartReload()
    {
        if (!gunData.reloading)
            anim.SetBool("Reload", true);
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
        AmmoText(gunData.currentAmmo);
        anim.SetBool("Reload", false);
    }


    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if(gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageable damagable = hitInfo.transform.GetComponent<IDamageable>();
                    damagable?.TakeDamage(gunData.damage);

                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
                AmmoText(gunData.currentAmmo);
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(muzzle.position, muzzle.forward);

    }

    private void OnGunShot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, gunData.maxDistance))
        {
            GameObject laser = GameObject.Instantiate(shotPrefab, transform.position, transform.rotation) as GameObject;
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);

        }
    }

    private void AmmoText(int current)
    {
        ammoUI.text = ""+current;
    }
}

