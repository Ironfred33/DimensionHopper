using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePosition : MonoBehaviour
{
    private GameObject _player;
    private Vector3 offset;
    private int _droneHeight;
    private int _droneDistance;
    public int droneClipping;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _droneHeight = GetComponent<EVCamera>().cameraHeight2DP;
        _droneDistance = GetComponent<EVCamera>().cameraDistance2DP;


    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector3(0, _droneHeight, -_droneDistance - droneClipping);
        transform.position = _player.transform.position + offset;
        
    }
}
