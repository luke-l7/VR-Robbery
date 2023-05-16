using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private NavMeshAgent enemyAgent;

    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        enemyAgent= GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyAgent.SetDestination(playerTransform.position);
    }
}
