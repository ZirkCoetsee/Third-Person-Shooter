using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private TrailRenderer tracerEffect;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private ParticleSystem vfxHitGreen;
    [SerializeField] private ParticleSystem vfxHitRed;
    [SerializeField] private float aimDuration = 0.3f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] Rig aimLayer;
    [SerializeField] Weapon weapon;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;


    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        //Get the mouse position (center of the screen)
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        //HitScan Method for projectiles
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        Vector3 worldAimTarget = mouseWorldPosition;
        //Only using the rotation on the y axis
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        //Switch between cameras based on aim action
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            //Set the rotation on move to false when we are aiming as to not clash with ThirdPersonController Rotation
            thirdPersonController.SetRotateOnMove(false);
            //animator.SetLayerWeight(1,Mathf.Lerp( animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            //Aim character through animation rig layer
            aimLayer.weight += Time.deltaTime / aimDuration;

            //Rotate player
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            if (starterAssetsInputs.shoot)
            {
                //Projectile HitScan method replaces hit logic on bullet object
                if (hitTransform != null)
                {
                    muzzleFlash.Emit(1);
                    var tracer = Instantiate(tracerEffect, spawnBulletPosition.position, Quaternion.identity);
                    tracer.AddPosition(spawnBulletPosition.position);
                    tracer.transform.position = raycastHit.point;
                    if (hitTransform.tag == "Enemy")
                    {
                        //Hit Target
                        //Instantiate(vfxHitGreen, mouseWorldPosition, Quaternion.identity);
                        vfxHitGreen.transform.position = raycastHit.point;
                        vfxHitGreen.transform.forward = raycastHit.normal;
                        vfxHitGreen.Emit(1);
                        weapon.Shoot(raycastHit);

                    }
                    else
                    {
                        //Hit Something else
                        //Instantiate(vfxHitRed, mouseWorldPosition, Quaternion.identity);
                        vfxHitRed.transform.position = raycastHit.point;
                        vfxHitRed.transform.forward = raycastHit.normal;
                        vfxHitRed.Emit(1);

                    }
                }

                starterAssetsInputs.shoot = false;
            }

        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            //animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            //Aim character through animation rig layer
            aimLayer.weight -= Time.deltaTime / aimDuration;
            starterAssetsInputs.shoot = false;

        }




    }
}
