using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;

    NavMeshAgent pathfinder;
    Animator animator;
    Transform target;
    
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    private void Update()
    {
        if (target == null)
        {
            animator = GetComponentInParent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Attack", false);
                animator.SetTrigger("Idle");
            }
            return;
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            EngageTarget();

        }else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }

     }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void EngageTarget()
    {
        FaceTarget();
        if (distanceToTarget >= pathfinder.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= pathfinder.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        animator = GetComponentInParent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Attack",true);
        }
    }

    private void ChaseTarget()
    {
        pathfinder.SetDestination(target.position);
        animator = GetComponentInParent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Attack", false);
            animator.SetTrigger("Move");
        }

        /*if (distanceToTarget <= chaseRange)
        {
            StartCoroutine(UpdatePath());
        }*/
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime * turnSpeed);
    }

    /*
    IEnumerator UpdatePath()
    {

        //Improved Update to update only 4x a second and not a shit tun
        float refreshRate = 0.25f;

        while (target != null)
        {
            pathfinder.SetDestination(target.position);

            yield return new WaitForSeconds(refreshRate);
        }
    }*/
}
