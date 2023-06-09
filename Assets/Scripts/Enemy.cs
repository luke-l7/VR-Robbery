using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using FMOD.Studio;
using FMODUnity;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent enemyAgent;
    private Animator anim;
    public float radius;
    public string yawn;
    bool playSound = true;
    bool playSeeSound = true;
    bool playHearSound = true;
    //waypoints
    public Transform[] waypoints;
    int wayPointIndex;
    Vector3 nextWaypoint;
    public GameObject monetorWaypoint;
    bool shouldRest = true;

    //raycasting
    [Range(0, 360)]
    public float angle;
    public GameObject player;
    public bool canSeePlayer;
    public float PlayerEnemyDistance;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public GameObject pickup;
    // Start is called before the first frame update
    void Start()
    {
        PlayerEnemyDistance = Vector3.Distance(player.transform.position, transform.position);
        anim = GetComponent<Animator>();
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAgent.speed = 0.05f;
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(transform.position, nextWaypoint));
        if (canSeePlayer)
        {
            anim.SetBool("persuit", true);
            enemyAgent.SetDestination(player.transform.position);
        }
        else if(Vector3.Distance(transform.position, nextWaypoint) < 2)
        {
            //rest
            if (waypoints[wayPointIndex] == monetorWaypoint.transform && shouldRest)
            {

                TriggerSound();
                
                anim.SetBool("patrol", false);
                anim.SetBool("rest", true);
                //stop for 3 seconds, cant see
                canSeePlayer = false;
                StartCoroutine(monitorRest());
            }
            else
            {
                
                anim.SetBool("persuit", false);
                anim.SetBool("patrol", true);
                anim.SetBool("rest", false);

                advanceToNextWaypoint();
                updateDestination();
            }
        }
        else
        {
            anim.SetBool("persuit", false);
            anim.SetBool("patrol", true);
            updateDestination();
        }
        
        //Debug.Log(canSeePlayer);
    }
    void updateDestination()
    {
        nextWaypoint = waypoints[wayPointIndex].position;
        enemyAgent.SetDestination(nextWaypoint);
    }
    void advanceToNextWaypoint()
    { 
        wayPointIndex++;
        if(wayPointIndex == waypoints.Length)
        {
            wayPointIndex = 0;
            shouldRest= true;
            playSound = true;
        }
    }
    private IEnumerator monitorRest()
    {
        yield return new WaitForSeconds(5);

        shouldRest = false;

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
        
        PlayerEnemyDistance = Vector3.Distance(player.transform.position, transform.position);
            //Debug.Log(PlayerEnemyDistance);
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        //can hear player
        if ((!canSeePlayer & Vector3.Distance(player.transform.position, transform.position) < radius - 2)  || pickup.GetComponent<pickup>().alarmOff)
        {
            //Debug.Log("i see you");
            canSeePlayer = true;
            this.transform.LookAt(player.transform.position);
            TriggerHearSound();


        }
        else if (rangeChecks.Length != 0 ) {
            //only the player is in the array
            Transform target = rangeChecks[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
            
            if (!pickup.GetComponent<pickup>().alarmOff && Vector3.Angle(transform.forward, dirToTarget) < angle / 2 )
            {
                //can see player 
                if(!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstructionMask))
                {
                    Debug.Log(canSeePlayer);
                    canSeePlayer = true;
                    TriggerSeeSound();

                }
                else
                {
                    canSeePlayer = false;
                    playSeeSound = true;
                    playHearSound= false;
                }
            }
            
            else if(!pickup.GetComponent<pickup>().alarmOff)
                canSeePlayer = false;
        }
        
        else if(canSeePlayer) 
        {
            canSeePlayer= false;
            playHearSound = true;
            playSeeSound = true;


        }
    }

    public void TriggerSound()
    {
        if (playSound)
        {
            RuntimeManager.PlayOneShot("event:/Guard/Yawn", transform.position);
        }
        playSound = false;
    }

    public void TriggerSeeSound()
    {
        if (playSeeSound)
        {
            RuntimeManager.PlayOneShot("event:/Guard/Im gonna get you", transform.position);

        }
        playSeeSound = false;
        playHearSound= false;
    }

    public void TriggerHearSound()
    {
        if (playHearSound)
        {
            RuntimeManager.PlayOneShot("event:/Guard/Whos there", transform.position);

        }
        playHearSound = false;
    }

}
