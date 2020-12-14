using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagement : MonoBehaviour
{
    
    
    public GameObject playerPrefab;


    public Vector3 SpawnCoords;

    void Start()
    {
        Instantiate(playerPrefab, SpawnCoords, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
