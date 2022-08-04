using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField] Transform eggTransform;


    // Update is called once per frame
    void Update()
    {
        FaceNest();
    }
    private void FaceNest()
    {
        Vector3 direction = (eggTransform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    }
}
