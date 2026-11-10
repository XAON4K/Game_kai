using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float bulletForce = 70f;
    [SerializeField] private bool showAmmo = true;

    [Space(5)]

    [SerializeField] private WeaponScriptableObject _settings;

    [Space(5)]

    [SerializeField] private Transform _muzzle;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Animator _gunAnimator;

    [Space(5)]

    [Header("Sounds")]
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadStartSound;
    [SerializeField] private AudioClip reloadStopSound;
    private AudioSource audioSource;

    private int ammoInMagazine;
    private int ammoLeft;

    private bool canShoot = true;
    private bool canStopAnimation = true;
    private bool alreadyReloading;

    private Light _light;
    private Transform _camera;

    private float lastTimeShot;
    private float reloadStartTime;



    private void Awake()
    {
        _light = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();
        _camera = Camera.main.transform;

        ammoInMagazine = _settings.maxAmmoInMagazine;
        ammoLeft = _settings.maxAmmo;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastTimeShot + _settings.shootCooldown)
            Shoot();

        if (!Input.GetMouseButton(0) || !canShoot || alreadyReloading)
        {
            if (_gunAnimator != null && canStopAnimation) _gunAnimator.SetBool("Fire", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!alreadyReloading && ammoInMagazine < _settings.maxAmmoInMagazine)
                Reload();
        }

        if(Time.time >= reloadStartTime + _settings.reloadTime && alreadyReloading)
            FinishedReload();
    }

    private void OnEnable()
    {
        if(showAmmo)
            AmmoVisualizer.UpdateText(ammoInMagazine, ammoLeft);
        else
            AmmoVisualizer.UpdateText();
    }

    private void Shoot()
    {
        if (ammoInMagazine <= 0 && !alreadyReloading)
        {
            Reload();
            return;
        }

        if (!canShoot) return;

        if (alreadyReloading) return;

        if(fireSound != null) audioSource.PlayOneShot(fireSound);

        lastTimeShot = Time.time;

        StartCoroutine(ParticlesCoroutine());

        List<Vector3> spreads = new List<Vector3>();

        for(int i = 0; i < _settings.bulletsInSpread; i++)
            spreads.Add(new Vector3(Random.Range(-_settings.spreadKof, _settings.spreadKof), Random.Range(-_settings.spreadKof, _settings.spreadKof), Random.Range(-_settings.spreadKof, _settings.spreadKof)));

        for(int i = 0; i < _settings.bulletsInSpread; i++)
        {
            if(bulletPrefab == null) break;

            GameObject bullet = Instantiate(bulletPrefab, _muzzle.position, Quaternion.identity);

            bullet.transform.right = -_muzzle.forward + spreads[i];
            bullet.GetComponent<Rigidbody>().AddForce((_muzzle.forward + spreads[i]) * bulletForce);
        }

        if (_gunAnimator != null)
        {
            _gunAnimator.SetBool("Fire", true);
            StartCoroutine(StopAnimationCoroutine());
        }

        ammoInMagazine--;
        if (ammoInMagazine <= 0)
            canShoot = false;

        if (showAmmo)
            AmmoVisualizer.UpdateText(ammoInMagazine, ammoLeft);
        else
            AmmoVisualizer.UpdateText();

        if (_settings.castRay)
            CastRay(spreads);
    }

    private void CastRay(List<Vector3> spread)
    {
        for(int i = 0; i < spread.Count;i++)
        {
            Ray ray = new Ray(_camera.position, _camera.forward + spread[i]);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _settings.bulletDistance))
            {
                if (hit.collider.GetComponent<EnemyController>() != null)
                {
                    hit.collider.GetComponent<EnemyController>().GiveDamage(_settings.damage);
                }
                else if (hit.collider.GetComponent<BossController>() != null)
                {
                    hit.collider.GetComponent<BossController>().GiveDamage(_settings.damage);
                }
                else if (hit.collider.GetComponent<OpenDoorButton>() != null)
                {
                    hit.collider.GetComponent<OpenDoorButton>().OpenDoor();
                }
                else if (hit.collider.GetComponent<BigBeanController>() != null)
                {
                    hit.collider.GetComponent<BigBeanController>().CollectBean();
                }
            }
        }
    }

    IEnumerator ParticlesCoroutine()
    {
        if (_light != null)
        {
            _light.enabled = true;

            yield return new WaitForSeconds(.05f);

            _light.enabled = false;
        }
    }

    private void Reload()
    {
        if (ammoLeft <= 0) return; 

        if (_gunAnimator != null) _gunAnimator.SetTrigger("Reload");

        if (reloadStartSound != null) audioSource.PlayOneShot(reloadStartSound);

        reloadStartTime = Time.time;
        alreadyReloading = true;
    }

    private void FinishedReload()
    {
        if (reloadStopSound != null) audioSource.PlayOneShot(reloadStopSound);

        canShoot = true;

        ammoLeft -= _settings.maxAmmoInMagazine - ammoInMagazine;

        if (ammoLeft < 0)
        {
            ammoInMagazine = _settings.maxAmmoInMagazine + ammoLeft;
            ammoLeft = 0;
        }
        else
        {
            ammoInMagazine = _settings.maxAmmoInMagazine;
        }

        AmmoVisualizer.UpdateText(ammoInMagazine, ammoLeft);

        alreadyReloading = false;
    }

    IEnumerator StopAnimationCoroutine()
    {
        canStopAnimation = false;

        yield return new WaitForSeconds(.04f);

        canStopAnimation = true;
    }

    public void AddAmmo(int ammount)
    {
        ammoLeft += ammount;

        if (!gameObject.activeSelf) return;

        if(showAmmo)
            AmmoVisualizer.UpdateText(ammoInMagazine, ammoLeft);
        else
            AmmoVisualizer.UpdateText();
    }

    public (int, int) GetAmmo => (ammoInMagazine, ammoLeft);
}
