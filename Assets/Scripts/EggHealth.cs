using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EggHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] UnityEvent OnDestroyedEvent;
    [SerializeField] GameObject minimapNode;
    [SerializeField] GameObject directionIndicator;

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
        Destroy(directionIndicator);
        GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0,0,0));

    }
}
