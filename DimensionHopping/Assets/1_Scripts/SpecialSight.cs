using System.Collections;
using UnityEngine;

// Steuert die Vorschau des Perspektivwechsels
public class SpecialSight : MonoBehaviour
{

    public Camera camera;
    public TransformPositionOnPerspective scriptPGO;
    [HideInInspector] public PGO scriptPGOGenerator;
    [HideInInspector] public Vector3 transformFirstPoint;
    [HideInInspector] public Vector3 transformSecondPoint;
    [HideInInspector] public GameObject copy;
    [HideInInspector] public bool activeCoolDown;
    [HideInInspector] public bool activeSightTime;
    private float _elapsed;

    public GameObject copiedCubeImmobile;
    public GameObject copiedCubeMoving;
    [HideInInspector] public GameObject instantiatedCopy;
    [HideInInspector] public GameObject instantiatedMovingCopy;
    [HideInInspector] public EVSpecialSight specialSightEV;

    public float platformCopyDecreaseAmount;


    [SerializeField] private MeshRenderer _mesh;

    [SerializeField] private Material _transparencyMaterial;

    private Vector3 _hitSize;
    [SerializeField] private GameObject _pivot;
    private Vector3 _hitPosition;

    private GameObject _pivotCopy;

    void Start()
    {

        specialSightEV = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVSpecialSight>();

        _transparencyMaterial.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);

        _pivot = Resources.Load<GameObject>("Prefabs/Pivot");

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

                Debug.Log("hit the PGO: " + hit.transform.tag);

                _hitSize = hit.collider.gameObject.GetComponent<Renderer>().bounds.size;


                // if (GameObject.FindGameObjectWithTag("LevelGenerator") != null)
                // {

                //     scriptPGOGenerator = hit.transform.GetChild(0).GetComponent<PGO>();


                //     transformFirstPoint = scriptPGOGenerator.transformFirstPoint;
                //     transformSecondPoint = scriptPGOGenerator.transformSecondPoint;


                // }

                scriptPGO = hit.transform.GetComponent<TransformPositionOnPerspective>();


                transformFirstPoint = scriptPGO.transformFirstPoint;
                transformSecondPoint = scriptPGO.transformSecondPoint;



                // neuer Ansatz unten, könnte aber zu heeeeeeeeeeftigsten Komplikationen führen, deshalb wird jetzt doch wieder zur alten Methode geswitcht
                // CreateCopies(hit);



                CreateCopy(hit);

                StartCoroutine(TrackCoolDown());
                StartCoroutine(TrackSightTime());
                StartCoroutine(TransformCopyPosition());

            }

        }

    }




    void AlignCopy(GameObject copy, Vector3 hitPosition)
    {
        copy.transform.position = hitPosition;

        copy.transform.position = new Vector3(copy.transform.position.x + (0.5f * _hitSize.x), copy.transform.position.y + (0.5f * _hitSize.y), copy.transform.position.z - (0.5f * _hitSize.z));

        SetNewPivot(copy);


    }

    void SetNewPivot(GameObject copy)
    {
        _pivotCopy = Instantiate(_pivot, _hitPosition, Quaternion.identity);
        copy.gameObject.transform.SetParent(_pivotCopy.transform);


    }


    // altes Skript
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

    public void MakeTransparent(Material mat, GameObject obj)
    {
        var materials = obj.GetComponent<MeshRenderer>().materials;

        materials[0] = mat;

        obj.GetComponent<MeshRenderer>().materials = materials;

    }

    // Managet PGO-Kopien
    void HandleCopies()
    {

        // Macht Copies leicht transparent

        _mesh = instantiatedCopy.GetComponent<MeshRenderer>();


        MakeTransparent(_transparencyMaterial, instantiatedCopy);
        MakeTransparent(_transparencyMaterial, instantiatedMovingCopy);

        // instantiatedCopy.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);
        // instantiatedMovingCopy.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, specialSightEV.platformTransparency);

        DeleteAllColliders(instantiatedCopy);
        DeleteAllColliders(instantiatedMovingCopy);


        DeleteAllChildren(instantiatedCopy);
        DeleteAllChildren(instantiatedMovingCopy);


        // XXX - hier weitermachen
    
        MakeSlightlySmaller(instantiatedMovingCopy);


    }


    // um die Anzeige-Bugs zu vermeiden wenn 2 Materials direkt auf der selben Ebene sind
    void MakeSlightlySmaller(GameObject obj)
    {
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * platformCopyDecreaseAmount, obj.transform.localScale.y * platformCopyDecreaseAmount, obj.transform.localScale.z * platformCopyDecreaseAmount);

        //obj.transform.position = new Vector3(obj.transform.position.x + ((1.0f-platformCopyDecreaseAmount) / 2f), obj.transform.position.y + ((1.0f-platformCopyDecreaseAmount) / 2f),obj.transform.position.z (1.0f-platformCopyDecreaseAmount) / 2f );
        
  

    }

    // Löscht Collider auf PGO-Kopien, damit der Spieler nicht auf diesen laufen kann
    void DeleteAllColliders(GameObject copy)
    {

        Component[] colliders;

        colliders = copy.GetComponents(typeof(Collider));

        foreach (Collider collider in colliders)
        {
            Destroy(collider);
        }

    }


    // Löscht Child-Objekte der PGOs, sofern vorhanden
    void DeleteAllChildren(GameObject copy)
    {


        while (copy.transform.childCount > 0)
        {
            DestroyImmediate(copy.transform.GetChild(0).gameObject);
        }

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



    // neues Skript
    void CreateCopies(RaycastHit hit)
    {


        copiedCubeImmobile = GameObject.CreatePrimitive(PrimitiveType.Cube);

        _hitPosition = hit.transform.position;

        copiedCubeImmobile.transform.localScale = _hitSize;

        Debug.Log("hitPos: " + _hitPosition);

        AlignCopy(copiedCubeImmobile, _hitPosition);



        Debug.Log(hit.collider.gameObject.GetComponent<Renderer>().bounds.size);

        //copiedCubeImmobile = _proBuilderScript.o
        //copiedCubeImmobile.transform.localScale = hit.transform.localScale;


        // hier sind noch die "alten" Positionen. Muss angepasst werden an generierten Cube mit anderem Pivot 
        // oder Empty GAmeobject aus Resources generieren und als parent setzen nach AlignCopy (Z 115)

        if (hit.collider.gameObject.transform.position == transformFirstPoint)
        {
            _pivotCopy.transform.position = transformSecondPoint;
            //copiedCubeImmobile.transform.position = transformSecondPoint;

        }
        else if (hit.collider.gameObject.transform.position == transformSecondPoint)
        {
            _pivotCopy.transform.position = transformFirstPoint;
            //copiedCubeImmobile.transform.position = transformFirstPoint;

        }




    }

}


