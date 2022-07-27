using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeaponIndex = 0;
    [SerializeField] Weapon[] weapons;

    public Weapon currentWeapon;


    private void Start()
    {
        SetWeaponActive();
    }

    private void Update()
    {
        int previousWeapon = currentWeaponIndex;

        ProcessKeyInput();
        ProcessScrollWheel();

        if (previousWeapon != currentWeaponIndex)
        {
            SetWeaponActive();
        }
    }

    private void ProcessScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeaponIndex >= weapons.Length - 1)
            {
                currentWeaponIndex = 0;
            }
            else
            {
                currentWeaponIndex++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentWeaponIndex <= 0)
            {
                currentWeaponIndex = weapons.Length - 1;
            }
            else
            {
                currentWeaponIndex--;
            }
        }
    }

    private void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponIndex = 1;
        }
    }

    private void SetWeaponActive()
    {
        int weaponIndex = 0;

        foreach (Weapon weapon in weapons)
        {
            if (weaponIndex == currentWeaponIndex)
            {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon;

            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }
}
