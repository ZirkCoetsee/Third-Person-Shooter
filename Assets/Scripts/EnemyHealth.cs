using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] GameObject enemyContainer;
    [SerializeField] UnityEvent OnDamageTakenEvent;

    public void TakeDamage(float damage)
    {
        OnDamageTakenEvent.Invoke();
        hitPoints -= damage;
        if (hitPoints <= 0)
        {   
            GameObject.Destroy(enemyContainer);
        }
    }
}
