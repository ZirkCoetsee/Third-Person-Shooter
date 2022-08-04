using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtPickup : MonoBehaviour
{
    [SerializeField] int healAmount = 5;
    [SerializeField] int RotationSpeed = 10;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerHealth>().Heal(healAmount);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));
    }
}
