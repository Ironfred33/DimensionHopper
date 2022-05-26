using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Steuert die Gegner-KI
public class EnemyAI : MonoBehaviour
{
    public Vector3[] waypoints;
    public int destPoint = 0;
    public float stoppingDistance = 0.5f;

    private NavMeshAgent _enemyAgent;

    private GameObject _fireParticles;

    void Start()
    {
        _fireParticles = transform.Find("FireParticles").gameObject;
        
        _enemyAgent = GetComponent<NavMeshAgent>();

        
        // AutoBraking macht den Gegner langsamer, wenn er kurz vor seinem Zielpunkt ist

        // enemyAgent.autoBraking = false;

        waypoints[0] = transform.position;

        GoToNextDestination();

    }

    void Update()
    {
        if(!_enemyAgent.pathPending && _enemyAgent.remainingDistance < stoppingDistance)
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

        _enemyAgent.destination = waypoints[destPoint];

        destPoint = (destPoint + 1) % waypoints.Length;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("PGOzNegative") || collision.gameObject.CompareTag("PGOzPositive") || collision.gameObject.CompareTag("PGOxPositive") || collision.gameObject.CompareTag("PGOxNegative"))
        {
            Debug.Log("Enemy knocked");
            _enemyAgent.enabled = false;
            _fireParticles.SetActive(false);
        }
    }


}

    
