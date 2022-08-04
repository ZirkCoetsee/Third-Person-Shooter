using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float hitPoints = 50f;
    [SerializeField] private Image healthBar;
    private float maxHitPoints;

    private void Start()
    {
        maxHitPoints = hitPoints;
    }

    public void TakeDamage( float damage)
    {
        hitPoints -= damage;
        healthBar.fillAmount = hitPoints / maxHitPoints;
        if (hitPoints <= 0)
        {
            GetComponent<DeathHandler>().HandleDeath();
        }
    }

    public void Heal(float heal)
    {
        hitPoints += heal;
        healthBar.fillAmount = hitPoints / maxHitPoints;
    }

}
