using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Vector3[] waypoints;
    public int destPoint = 0;
    public float stoppingDistance = 0.5f;

    private NavMeshAgent enemyAgent;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();

        
        // AutoBraking macht den Gegner langsamer, wenn er kurz vor seinem Zielpunkt ist

        // enemyAgent.autoBraking = false;

        waypoints[0] = transform.position;

        GoToNextDestination();

    }

    // Update is called once per frame
    void Update()
    {
        if(!enemyAgent.pathPending && enemyAgent.remainingDistance < stoppingDistance)
        {
            GoToNextDestination();
        }
        
    }

    
    // Die Funktion legt den nächsten Waypoint als Zielpunkt fest
    void GoToNextDestination()
    {
        // Falls keine Waypoints eingestellt sind 
        
        if(waypoints.Length == 0)
        {
            return;
        }

        enemyAgent.destination = waypoints[destPoint];

        destPoint = (destPoint + 1) % waypoints.Length;
    }


}

    
