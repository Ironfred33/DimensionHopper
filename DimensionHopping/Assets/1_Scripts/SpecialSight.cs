using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Steuert die Vorschau des Perspektivwechsels
public class SpecialSight : MonoBehaviour
{
    public TransformPositionOnPerspective scriptPGO;
    public PGO scriptPGOLG;
    [HideInInspector] public PGO scriptPGOGenerator;
    public Camera camera;
    [HideInInspector] public EVSpecialSight specialSightEV;

    [HideInInspector] public Vector3 transformFirstPoint;
    [HideInInspector] public Vector3 transformSecondPoint;
    public GameObject copy;
    [HideInInspector] public bool activeCoolDown;
    [HideInInspector] public bool activeSightTime;
    public GameObject copiedCubeImmobile;
    public GameObject copiedCubeMoving;
    [HideInInspector] public GameObject instantiatedCopy;
    [HideInInspector] public GameObject instantiatedMovingCopy;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Material _transparencyMaterial;
    private Vector3 _hitSize;
    [SerializeField] private GameObject _pivot;
    private Vector3 _hitPosition;
    private GameObject _pivotCopy;

    public float platformCopyDecreaseAmount;
    private float _elapsed;

    void Start()
    {

        specialSightEV = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVSpecialSight>();

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


                if (GameObject.FindGameObjectWithTag("LevelGenerator"))
                {

                    scriptPGOLG = hit.transform.GetComponent<PGO>();
                    transformFirstPoint = scriptPGOLG.transformFirstPoint;
                    transformSecondPoint = scriptPGOLG.transformSecondPoint;

                }
                else
                {

                    scriptPGO = hit.transform.GetComponent<TransformPositionOnPerspective>();
                    transformFirstPoint = scriptPGO.transformFirstPoint;
                    transformSecondPoint = scriptPGO.transformSecondPoint;

                }


                switch (hit.transform.tag)
                {
                    case "PGOxPositive":
                        _transparencyMaterial.color = new Color(0.0f, 1.8f, 6.0f, specialSightEV.platformTransparency);
                        break;
                    case "PGOxNegative":
                        _transparencyMaterial.color = new Color(8.0f, 0.0f, 0.0f, specialSightEV.platformTransparency);
                        break;
                    case "PGOzPositive":
                        _transparencyMaterial.color = new Color(0.0f, 6.0f, 0.0f, specialSightEV.platformTransparency);
                        break;
                    case "PGOzNegative":
                        _transparencyMaterial.color = new Color(750.0f, 760.0f, 1.0f, specialSightEV.platformTransparency);
                        break;
                    default:
                        Debug.Log("No Tag");
                        break;
                }


                CreateCopy(hit);

                StartCoroutine(TrackCoolDown());
                StartCoroutine(TrackSightTime());
                StartCoroutine(TransformCopyPosition());

            }

        }

    }





    // altes Skript
    // Erstellt PGO-Kopie
    void CreateCopy(RaycastHit hit)
    {



        if (GameObject.FindGameObjectWithTag("LevelGenerator"))
        {
            copy = hit.transform.Find("Cube").gameObject;
        }
        else
        {
            copy = hit.transform.Find("CopyTarget").gameObject;
        }



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

        DeleteAllColliders(instantiatedCopy);
        DeleteAllColliders(instantiatedMovingCopy);


        DeleteAllChildren(instantiatedCopy);
        DeleteAllChildren(instantiatedMovingCopy);


        MakeSlightlySmaller(instantiatedMovingCopy);


    }


    // um die Anzeige-Bugs zu vermeiden wenn 2 Materials direkt auf der selben Ebene sind
    void MakeSlightlySmaller(GameObject obj)
    {
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * platformCopyDecreaseAmount, obj.transform.localScale.y * platformCopyDecreaseAmount, obj.transform.localScale.z * platformCopyDecreaseAmount);


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

}


