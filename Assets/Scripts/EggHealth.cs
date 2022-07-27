using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EggHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] UnityEvent OnDestroyedEvent;
    [SerializeField] GameObject minimapNode;

    bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
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
        OnDestroyedEvent.Invoke();
        Destroy(minimapNode);
        this.GetComponent<Renderer>().material.color = new Color(0,0,0);
    }
}
