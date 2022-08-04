using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinHandler : MonoBehaviour
{
    [SerializeField] public Canvas gameWinCancas;

    private void Start()
    {
        gameWinCancas.enabled = false;

    }

    public void HandleWin()
    {
        gameWinCancas.enabled = true;
        Time.timeScale = 0;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
