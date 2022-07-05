using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    public void Shoot(RaycastHit raycastHit)
    {
        EnemyHealth enemy = raycastHit.transform.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(25f);

        }
    }

}
