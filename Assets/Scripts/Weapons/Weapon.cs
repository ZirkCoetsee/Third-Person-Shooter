using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] private float damage = 25f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private ParticleSystem vfxHitGreen;
    [SerializeField] private ParticleSystem vfxHitRed;
    [SerializeField] private TrailRenderer tracerEffect;
    [SerializeField] private float shootDelay = 0.1f;
    [SerializeField] private Ammo ammoSlot;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] float inaccuracyDistance;
    [SerializeField] int bulletsPershot = 1;
    [SerializeField] TextMeshProUGUI ammoText;

    public AudioClip shootClip;
    public AudioClip emptyClip;
    [Range(0, 1)] public float shootClipVolume = 0.5f;
    public float msBetweenShots = 100f;

    float nextShotTime;

    private bool canShoot = true;

    private void Update()
    {
        DisplayAmmo();
    }

    private void DisplayAmmo()
    {
        int ammoAmount = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = ammoAmount.ToString();
    }

    private void OnEnable()
    {
        canShoot = true;
    }

    public void ShootRapid(RaycastHit raycastHit)
    {
 
        //Stops user from shooting every milisecond
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000f;
            if (ammoSlot.GetCurrentAmmo(ammoType) >= 1 && canShoot)
            {
                EnemyHealth enemyHealth = raycastHit.transform.GetComponent<EnemyHealth>();
                EggHealth eggHealth = raycastHit.transform.GetComponent<EggHealth>();
                muzzleFlash.Emit(1);


                if (bulletsPershot > 1)
                {
                    for (int i = 0; i < bulletsPershot; i++)
                    {
                        Debug.Log("Shot Gun Shell Fired");
                        Vector3 shotGunProjectileDirection = new Vector3(raycastHit.point.x + Random.Range(-inaccuracyDistance, inaccuracyDistance), raycastHit.point.y + Random.Range(-inaccuracyDistance, inaccuracyDistance), raycastHit.point.z + Random.Range(-inaccuracyDistance, inaccuracyDistance));
                        var tracer = Instantiate(tracerEffect, spawnBulletPosition.position, Quaternion.identity);
                        tracer.AddPosition(spawnBulletPosition.position);
                        tracer.transform.position = shotGunProjectileDirection;
                    }
                }
                else
                {
                    var tracer = Instantiate(tracerEffect, spawnBulletPosition.position, Quaternion.identity);
                    tracer.AddPosition(spawnBulletPosition.position);
                    tracer.transform.position = raycastHit.point;
                }

                AudioSource.PlayClipAtPoint(shootClip, spawnBulletPosition.position, shootClipVolume);

                ammoSlot.ReduceCurrentAmmo(ammoType);
                if (raycastHit.transform.tag == "Enemy")
                {
                    //Hit Target
                    vfxHitGreen.transform.position = raycastHit.point;
                    vfxHitGreen.transform.forward = raycastHit.normal;
                    vfxHitGreen.Emit(1);
                }
                else
                {
                    //Hit Something else
                    vfxHitRed.transform.position = raycastHit.point;
                    vfxHitRed.transform.forward = raycastHit.normal;
                    vfxHitRed.Emit(1);
                }
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);

                }
                if (eggHealth != null)
                {
                    eggHealth.TakeDamage(damage);
                }
            }
            else if (ammoSlot.GetCurrentAmmo(ammoType) < 1)
            {
                AudioSource.PlayClipAtPoint(emptyClip, spawnBulletPosition.position, shootClipVolume);
            }
            canShoot = true;
        }


    }
    
    public IEnumerator Shoot(RaycastHit raycastHit)
    {
        if (ammoSlot.GetCurrentAmmo(ammoType) >= 1 && canShoot)
        {
            EnemyHealth enemyHealth = raycastHit.transform.GetComponent<EnemyHealth>();
            EggHealth eggHealth = raycastHit.transform.GetComponent<EggHealth>();
            muzzleFlash.Emit(1);


            if (bulletsPershot > 1)
            {
                for (int i = 0; i < bulletsPershot; i++)
                {
                    Debug.Log("Shot Gun Shell Fired");
                    Vector3 shotGunProjectileDirection = new Vector3( raycastHit.point.x + Random.Range(-inaccuracyDistance,inaccuracyDistance), raycastHit.point.y + Random.Range(-inaccuracyDistance, inaccuracyDistance), raycastHit.point.z + Random.Range(-inaccuracyDistance, inaccuracyDistance));
                    var tracer = Instantiate(tracerEffect, spawnBulletPosition.position, Quaternion.identity);
                    tracer.AddPosition(spawnBulletPosition.position);
                    tracer.transform.position = shotGunProjectileDirection;
                }
            }
            else
            {
                var tracer = Instantiate(tracerEffect, spawnBulletPosition.position, Quaternion.identity);
                tracer.AddPosition(spawnBulletPosition.position);
                tracer.transform.position = raycastHit.point;
            }

            AudioSource.PlayClipAtPoint(shootClip, spawnBulletPosition.position, shootClipVolume);

            ammoSlot.ReduceCurrentAmmo(ammoType);
            if (raycastHit.transform.tag == "Enemy")
            {
                //Hit Target
                vfxHitGreen.transform.position = raycastHit.point;
                vfxHitGreen.transform.forward = raycastHit.normal;
                vfxHitGreen.Emit(1);
            }
            else
            {
                //Hit Something else
                vfxHitRed.transform.position = raycastHit.point;
                vfxHitRed.transform.forward = raycastHit.normal;
                vfxHitRed.Emit(1);
            }
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);

            }
            if (eggHealth != null)
            {
                eggHealth.TakeDamage(damage);
            }
        }else if (ammoSlot.GetCurrentAmmo(ammoType) < 1)
        {
            AudioSource.PlayClipAtPoint(emptyClip, spawnBulletPosition.position, shootClipVolume);
        }
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

}
