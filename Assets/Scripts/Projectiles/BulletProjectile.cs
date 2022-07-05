using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] float speed = 50f;
    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>() != null)
        {
            //Hit Target
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
        else
        {
            //Hit Something else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);

        }
        Destroy(gameObject);

    }
}
