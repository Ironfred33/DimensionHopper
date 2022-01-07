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
    public EVCamera cameraEV;

    // Start is called before the first frame update
    void Start()
    {


        _player = GameObject.FindGameObjectWithTag("Player");
        _droneHeight = cameraEV.cameraHeight2DP;
        _droneDistance = cameraEV.cameraDistance2DP;
        offset = new Vector3(0, _droneHeight, -_droneDistance - droneClipping);

        transform.position = _player.transform.position + offset;


    }
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _player.transform.position + offset, cameraEV.smoothing);
    }
}
