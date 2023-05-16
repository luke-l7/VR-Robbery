using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private NavMeshAgent enemyAgent;
    private Animator anim;
    public float radius;

    [Range(0, 360)]
    public float angle;
    public GameObject player;
    public bool canSeePlayer;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (canSeePlayer)
        {
            anim.SetBool("persuit", true);
            enemyAgent.SetDestination(player.transform.position);
        }
        else
        {
            anim.SetBool("persuit", false);

        }
        //Debug.Log(canSeePlayer);
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if(rangeChecks.Length != 0 ) {
            //only the player is in the array
            Transform target = rangeChecks[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
                //can see player
                if(!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;

                }
            }
            else
                canSeePlayer = false;
        }
        else if(canSeePlayer) 
        {
            canSeePlayer= false;
        }
    }
}
