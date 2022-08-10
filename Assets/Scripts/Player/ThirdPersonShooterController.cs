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
    [SerializeField] private float aimDuration = 0.3f;
    [SerializeField] Rig aimLayer;
    [SerializeField] TMPro.TextMeshProUGUI eggText;
    [SerializeField] TMPro.TextMeshProUGUI enemyText;
    [SerializeField] Canvas PauseCanvas;
    private EggHealth[] eggList;


    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private WeaponSwitcher weaponSwitcher;
    private int maxEggs;
    private int eggsleft;



    private void Awake()
    {
        PauseCanvas.enabled = false;
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        weaponSwitcher = FindObjectOfType<WeaponSwitcher>();
        eggList = FindObjectsOfType<EggHealth>();
        maxEggs = eggList.Length;
        eggsleft = maxEggs;
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            PauseGame();
        }

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
                    //StartCoroutine(weaponSwitcher.currentWeapon.Shoot(raycastHit));
                    weaponSwitcher.currentWeapon.ShootRapid(raycastHit);
                }

                //starterAssetsInputs.shoot = false;
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

    public void PauseGame()
    {
        PauseCanvas.enabled = true;
        Time.timeScale = 0;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        PauseCanvas.enabled = false;
        Time.timeScale = 1;
        FindObjectOfType<WeaponSwitcher>().enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void UpdateNumberOfEggs()
    {
        eggsleft = eggsleft - 1;
        Debug.Log("Update Number of eggs called");
        eggText.text = $"{eggsleft}/{maxEggs}";

        if (eggsleft == 0)
        {
            GetComponent<WinHandler>().HandleWin();
        }
    }
}
