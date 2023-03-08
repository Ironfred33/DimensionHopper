using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossfight_Laser : MonoBehaviour
{
    [SerializeField] private GameObject[] laserBalls;
    [SerializeField] private List<Transform> _wayPointList;
    [SerializeField] private Transform[] _wayPoints;
    private AudioSource laserAudio;
    private LineRenderer laserLine;
    private GameObject _laserOrigin;
    
    [SerializeField] private float _coolDown;
    [SerializeField] private float _walkSpeed;
    private bool _firing;


    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserAudio = GetComponent<AudioSource>();

    }

    void Start()
    {
        laserLine.enabled = false;
        _wayPointList = new List<Transform>();
        for (int i = 0; i < _wayPoints.Length; i++)
        {
            _wayPointList.Add(_wayPoints[i]);
        }
    }

    void Update()
    {
        float step = _walkSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _wayPointList[0].position, step);
        if (transform.position == _wayPointList[0].position)
        {
            _wayPointList.RemoveAt(0);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Attack();
        }

        if (_firing)
        {
            laserLine.SetPosition(0, _laserOrigin.transform.position);
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
        _laserOrigin = laserBalls[Random.Range(0, laserBalls.Length)];
        _laserOrigin.SetActive(true);
        RaycastHit hit;

        if (Physics.Raycast(_laserOrigin.transform.position, (Vector3.down + transform.forward * 10), out hit, Mathf.Infinity))
        {
            _firing = true;
            laserLine.SetPosition(1, hit.point);
        }
        yield return new WaitForSeconds(_coolDown);
    }
}
