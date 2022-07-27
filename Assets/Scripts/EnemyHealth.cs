using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] GameObject enemyContainer;
    [SerializeField] UnityEvent OnDamageTakenEvent;
    [SerializeField] UnityEvent OnEnemyDiedEvent;

    bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        OnDamageTakenEvent.Invoke();
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        GetComponent<Animator>().SetTrigger("Die");
        OnEnemyDiedEvent.Invoke();
    }
}
