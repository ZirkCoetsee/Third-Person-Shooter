using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinicamController : MonoBehaviour
{
    Transform target;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float zOffset;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3( target.position.x + xOffset,target.position.y + yOffset ,target.position.z + zOffset);
    }
}
