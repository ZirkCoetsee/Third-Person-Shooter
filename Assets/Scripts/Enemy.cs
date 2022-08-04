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
    [SerializeField] Transform soundSpawnPoint;

    NavMeshAgent pathfinder;
    Animator animator;
    Transform target;
    Transform nestPosition;
    EnemyHealth health;
    List<EggHealth> nests;


    public AudioClip chaseClip;
    public AudioClip attackClip;
    public AudioClip dieClip;
    [Range(0, 1)] public float chaseClipVolume = 1f;

    float distanceToTarget = Mathf.Infinity;
    float distanceToNest = Mathf.Infinity;
    bool isProvoked = false;
    bool playClipOnce = true;


    // Start is called before the first frame update
    void Start()
    {
        pathfinder = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInParent<Animator>();
        nests = new List<EggHealth>(FindObjectsOfType<EggHealth>());
        SelectRandomNest();
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    private void Update()
    {
        if (health.IsDead())
        {
            AudioSource.PlayClipAtPoint(dieClip, soundSpawnPoint.position, chaseClipVolume);
            pathfinder.enabled = false;
            enabled = false;
            GetComponent<MeshCollider>().enabled = false;
        }

        if (target == null)
        {
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Attack", false);
                animator.SetTrigger("Idle");
            }
            return;
        }
        else if( target != null && !isProvoked){
            //Debug.Log("Navigating to nest");
            PatroleNests();
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
        //Debug.Log($"Distance to target: {distanceToTarget}/n Stopping Distance {pathfinder.stoppingDistance}");
        if (distanceToTarget >= pathfinder.stoppingDistance)
        {
            ChaseTarget();
            while (playClipOnce == true)
            {
                //Debug.Log("Play chaseClip");
                AudioSource.PlayClipAtPoint(chaseClip, soundSpawnPoint.position, 1);
                playClipOnce = false;

            }
        }

        if (distanceToTarget <= pathfinder.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        if (animator != null)
        {
            animator.SetBool("Attack",true);
            //Debug.Log("Attacking Player");
        }
    }

    private void ChaseTarget()
    {
        if (pathfinder.enabled)
        {
            pathfinder.SetDestination(target.position);
        }
        if (animator != null)
        {
            animator.SetBool("Attack", false);
            animator.SetTrigger("Move");
        }
    }

    private void PatroleNests()
    {
        //Debug.Log($"nestPosition: {nestPosition.position}");
        //Debug.Log($"currentPosition: {transform.position}");
            distanceToNest = Vector3.Distance(nestPosition.position, transform.position);
            //Debug.Log($"Distance to nest: {distanceToNest}");
            //Debug.Log($"Stopping Distance {pathfinder.stoppingDistance}");
            //FaceNest();

            if (distanceToNest >= pathfinder.stoppingDistance)
            {
                MoveToNest();
                while (playClipOnce == true)
                {
                    //Debug.Log("Play chaseClip");
                    AudioSource.PlayClipAtPoint(chaseClip, soundSpawnPoint.position, 1);
                    playClipOnce = false;

                }
            }

            if (distanceToNest <= pathfinder.stoppingDistance)
            {
                ChangeNest();
            }
    }

    private void ChangeNest()
    {
        if (nests.Count != 0)
        {
            SelectRandomNest();

        }
    }

    private void MoveToNest()
    {
        if (pathfinder.enabled)
        {
            pathfinder.SetDestination(nestPosition.position);
        }
        if (animator != null)
        {
            animator.SetBool("Attack", false);
            animator.SetTrigger("Move");
        }
    }

    private void SelectRandomNest()
    {
        if (nests.Count == 0)
        {
            isProvoked = true;
            return;
        }
        else
        {
            //Debug.Log("Select a random nest");
            int value = UnityEngine.Random.Range(0, nests.Count);
            //Debug.Log($"Moving to nest number: {value}" );
            if (nests[value].IsDead())
            {
                nests.Remove(nests[value]);
                if (nests.Count == 0)
                {
                    isProvoked = true;
                    return;
                }
                value = UnityEngine.Random.Range(0, nests.Count);
            }

            nestPosition = nests[value].gameObject.transform;
        }

    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime * turnSpeed);
    }

    private void FaceNest()
    {
        Vector3 direction = (nestPosition.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
}
