using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert die Vorschau des Perspektivwechsels
public class SpecialSight : MonoBehaviour
{

    public Camera camera;
    [HideInInspector] public TransformPositionOnPerspective scriptPGO;
    [HideInInspector] public PGO scriptPGOGenerator;
    [HideInInspector] public Vector3 transformFirstPoint;
    [HideInInspector] public Vector3 transformSecondPoint;
    [HideInInspector] public GameObject copy;
    [HideInInspector] public bool activeCoolDown;
    [HideInInspector] public bool activeSightTime;
    private float _elapsed;
    [HideInInspector] public GameObject instantiatedCopy;
    [HideInInspector] public GameObject instantiatedMovingCopy;
    [HideInInspector] public EVSpecialSight specialSightEV;

    void Start()
    {

        specialSightEV = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVSpecialSight>();

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V) && !activeCoolDown)
        {
            ShootRay();

        }

    }

    // Führt Perspektivenvorschau durch, sofern PGO in Range ist 
    void ShootRay()
    {

        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, specialSightEV.sightRange))
        {

            if (hit.transform.tag == "PGOxPositive" || hit.transform.tag == "PGOzPositive" || hit.transform.tag == "PGOxNegative" || hit.transform.tag == "PGOzNegative")
            {

                Debug.Log(hit.transform.tag);


                // if (GameObject.FindGameObjectWithTag("LevelGenerator") != null)
                // {

                //     scriptPGOGenerator = hit.transform.GetChild(0).GetComponent<PGO>();
                

                //     transformFirstPoint = scriptPGOGenerator.transformFirstPoint;
                //     transformSecondPoint = scriptPGOGenerator.transformSecondPoint;


                // }
                
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

    // Erstellt PGO-Kopie
    void CreateCopy(RaycastHit hit)
    {


        copy = hit.transform.gameObject;



        if (hit.collider.gameObject.transform.position == transformFirstPoint)
        {

            instantiatedCopy = Instantiate(copy, transformSecondPoint, Quaternion.identity);

            instantiatedMovingCopy = Instantiate(copy, transformFirstPoint, Quaternion.identity);

            HandleCopies();


        }
        else if (hit.collider.gameObject.transform.position == transformSecondPoint)
        {
            instantiatedCopy = Instantiate(copy, transformFirstPoint, Quaternion.identity);

            instantiatedMovingCopy = Instantiate(copy, transformSecondPoint, Quaternion.identity);

            HandleCopies();


        }


    }

    // Managet PGO-Kopien
    void HandleCopies()
    {


        instantiatedCopy.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);
        instantiatedMovingCopy.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);

        DeleteColliders();
        CheckForChild(instantiatedCopy);
        CheckForChild(instantiatedMovingCopy);

    }

    // Löscht Collider auf PGO-Kopien, damit der Spieler nicht auf diesen laufen kann
    void DeleteColliders()
    {
        Collider col;

        col = instantiatedCopy.GetComponent<Collider>();

        Destroy(col);

        col = instantiatedMovingCopy.GetComponent<Collider>();

        Destroy(col);

    }


    // Löscht Child-Objekte der PGOs, sofern vorhanden
    void CheckForChild(GameObject copy)
    {
        if (copy.transform.childCount == 1)
        {
            DeleteChild(copy);
        }
        else Debug.Log("No Child");

    }

    // Löscht Child-Objekte der PGOs
    void DeleteChild(GameObject copy)
    {
        Destroy(copy.gameObject.transform.GetChild(0).gameObject);
    }




    // Überwacht Cooldown
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

    // Überwacht die Dauer der Perspektivenvorschau
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


    // Verschiebt PGO-Kopie in Vorschau und löscht sie anschließend am Zielort
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
