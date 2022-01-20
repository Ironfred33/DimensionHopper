using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSight : MonoBehaviour
{

    public Camera camera;

    public TransformPositionOnPerspective scriptPGO;

    public Vector3 transformFirstPoint;
    public Vector3 transformSecondPoint;

    public GameObject copy;

    public bool activeCoolDown;

    public bool activeSightTime;


    private float _elapsed;
    public GameObject instantiatedCopy;

    public GameObject instantiatedMovingCopy;

    public EVSpecialSight specialSightEV;

    // Start is called before the first frame update
    void Start()
    {

        specialSightEV = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVSpecialSight>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V) && !activeCoolDown)
        {
            ShootRay();

        }

    }



    // COLLIDER ENTFERNEN, DAMIT SPIELER NICHT WIRKLICH AUF DIESE PLATTFORMEN KANN
    // BEIDE COPYS BLINKEN LASSEN BZW OPACITY WEGNEHMEN


    void ShootRay()
    {

        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, specialSightEV.sightRange))
        {

            if (hit.transform.tag == "PGOxPositive" || hit.transform.tag == "PGOzPositive" || hit.transform.tag == "PGOxNegative" || hit.transform.tag == "PGOzNegative")
            {

                Debug.Log(hit.transform.tag);

                scriptPGO = hit.transform.GetComponent<TransformPositionOnPerspective>();

                transformFirstPoint = scriptPGO.transformFirstPoint;
                transformSecondPoint = scriptPGO.transformSecondPoint;

                CreateCopy(hit);
                StartCoroutine(TrackCoolDown());
                StartCoroutine(TrackSightTime());
                StartCoroutine(TransformCopyPosition());

            }

        }

    }

    void CreateCopy(RaycastHit hit)
    {
        //GameObject instantiatedCopy;
        //Transform instantiatedCopy;

        copy = hit.transform.gameObject;

        

        if (hit.collider.gameObject.transform.position == transformFirstPoint)
        {

            instantiatedCopy = Instantiate(copy, transformSecondPoint, Quaternion.identity);

            instantiatedMovingCopy = Instantiate(copy, transformFirstPoint, Quaternion.identity);

            instantiatedCopy.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);
            instantiatedMovingCopy.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);


        }
        else if (hit.collider.gameObject.transform.position == transformSecondPoint)
        {
            instantiatedCopy = Instantiate(copy, transformFirstPoint, Quaternion.identity);

            instantiatedMovingCopy = Instantiate(copy, transformSecondPoint, Quaternion.identity);

            
            instantiatedCopy.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);
            instantiatedMovingCopy.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);
          

        }


    }


    IEnumerator TrackCoolDown()
    {
        float _elapsed = 0f;
        float _dt;
        activeCoolDown = true;

        while (_elapsed <= specialSightEV.coolDown)
        {
            _dt = Time.deltaTime;
            _elapsed += _dt;

            yield return null;

        }

        activeCoolDown = false;


    }

    IEnumerator TrackSightTime()
    {

        float _elapsed = 0f;
        float _dt;
        activeSightTime = true;

        while (_elapsed <= specialSightEV.sightTime)
        {
            _dt = Time.deltaTime;
            _elapsed += _dt;

            yield return null;

        }

        Destroy(instantiatedCopy);
        activeSightTime = false;

    }


    IEnumerator TransformCopyPosition()
    {
        _elapsed = 0f;
        float _dt;

        if (instantiatedMovingCopy.transform.position == transformFirstPoint)
        {

            while (_elapsed <= specialSightEV.sightTime)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;

                instantiatedMovingCopy.transform.position = Vector3.Lerp(transformFirstPoint, transformSecondPoint, _elapsed / specialSightEV.sightTime);



                yield return null;
            }

        }
        else if (instantiatedMovingCopy.transform.position == transformSecondPoint)
        {

            while (_elapsed <= specialSightEV.sightTime)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;

                instantiatedMovingCopy.transform.position = Vector3.Lerp(transformSecondPoint, transformFirstPoint, _elapsed / specialSightEV.sightTime);



                yield return null;
            }

        }









        Destroy(instantiatedMovingCopy);





    }



}
