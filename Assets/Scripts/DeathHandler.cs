using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] public Canvas gameOverCancas;

    private void Start()
    {
        gameOverCancas.enabled = false;
            
    }

    public void HandleDeath()
    {
        gameOverCancas.enabled = true;
        Time.timeScale = 0;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
