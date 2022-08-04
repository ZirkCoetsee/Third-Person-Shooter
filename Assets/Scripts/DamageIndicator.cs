using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageIndicator : MonoBehaviour
{
    [SerializeField] Canvas impactCanvas;
    [SerializeField] private float impactTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        impactCanvas.enabled = false;
    }

    public void ShowDamageImpact()
    {
        StartCoroutine(ShowSplatter());
    }

    IEnumerator ShowSplatter()
    {
        impactCanvas.enabled = true;
        yield return new WaitForSeconds(impactTime);
        impactCanvas.enabled = false;
    }
}
