using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossfight_Laser : MonoBehaviour
{
    [SerializeField] private GameObject[] laserBalls;
    [SerializeField] private bool _firing;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private List<Transform> _wayPointList;

    [SerializeField] private Transform[] _wayPoints;
    private AudioSource laserAudio;
    LineRenderer laserLine;

    void Awake() 
    {
        laserLine = GetComponent<LineRenderer>();
        laserAudio = GetComponent<AudioSource>();
    
    }

    void Start()
    {
        laserLine.enabled = false;
        _wayPointList = new List<Transform>();
        for(int i = 0; i < _wayPoints.Length; i++)
        {
            _wayPointList.Add(_wayPoints[i]);
        }
        Debug.Log(_wayPointList);
    }

    // Update is called once per frame
    void Update()
    {
        float step = _walkSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _wayPointList[0].position, step);
        if(transform.position == _wayPointList[0].position)
        {
            _wayPointList.RemoveAt(0);
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            Attack();
        }

    }

    private void Attack()
    {
        StartCoroutine(LaserCooldown());
    }

    private IEnumerator LaserCooldown()
    {
        laserLine.enabled = true;
        laserAudio.Play();
        GameObject laserDestination = laserBalls[Random.Range(0, laserBalls.Length)];
        laserDestination.SetActive(true);
        laserLine.SetPosition(0,laserDestination.transform.position);
        Debug.Log("Something happened");
        RaycastHit hit;

        if(Physics.Raycast(laserDestination.transform.position, (transform.forward + transform.right).normalized, out hit, Mathf.Infinity))
        {
            laserLine.SetPosition(1, hit.transform.position);
            
        }
        yield return new WaitForSeconds(_coolDown);
    }
}
