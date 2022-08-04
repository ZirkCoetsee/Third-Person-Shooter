using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPickupSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] pickupArray;

    public void SpawnRandomPickup(Transform spawnPoint)
    {
        int value = Random.Range(0, pickupArray.Length);
        GameObject pickUp = Instantiate(pickupArray[value], new Vector3(spawnPoint.position.x, 1f, spawnPoint.position.z), Quaternion.identity) as GameObject;

    }
}
