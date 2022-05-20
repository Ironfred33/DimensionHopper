using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenerateLevel : MonoBehaviour
{

    public int[] generationOrder; // =  new int[] { 0, 1, 2, 3, 3, 0 };

    public GameObject myPrefab;

    [SerializeField]private GameObject _shortLengthPlatform;
    [SerializeField]private GameObject _midLengthPlatform;
    [SerializeField]private GameObject _longLengthPlatform;

    [SerializeField] private GameObject _spike;
    [SerializeField] private GameObject _fourSpikes;

    public List<GameObject> platforms = new List<GameObject>();

    public int maxSegmentLength;


    [SerializeField] private float _countSegmentLength;
    private GameObject _myPrefabChild;


    public int segmentAmount;

    public int platformVerticalRange;


    public enum VerticalAlignment
    {
        Downwards,

        Even,

        Upwards
    }

    public VerticalAlignment verticalAlignment;
    private int _randomNumber;

    public Vector3 platformSpawnPos;


    private int _highGapLengthAmount;

    private int _lowGapLengthAmount;

    [SerializeField] private int _generatedSegments;

    private int _directionCounter;

    private bool _isGenerating;

    private int _remainingSegmentLength;

    public bool levelGenerated;

    public bool segmentGenerated;

    [SerializeField] private float _spawnAxis;

    public Platform platformScript;

    [SerializeField] private CameraTransition _cameraTransitionScript;

    public bool generateRandomDirections;

    public bool generateControlledRandomDirections;

    public GameObject prefabCopy;


    private int _spikeFrequencyHighAmount;

    private int _spikeFrequencyLowAmount;

    private int _spikeFrequency;

    private int _PGOFrequencyHighAmount;
    private int _PGOFrequencyLowAmount;

    private int _PGOFrequency;

    private bool _firstPlatformIgnored;

    private bool _firstPlatformMarked;



    private int _spawnSecondSpikeChance;

    private Material _goalMaterial;
    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD
    }


    public enum CurrentAxis
    {
        xPositive,
        xNegative,
        zPositive,
        zNegative
    }


    public Difficulty difficulty;

    private CurrentAxis currentAxis;


    void Start()
    {
        Setup();

    }

    private void Update()
    {

        SetDifficulty();



    }

    void Setup()
    {

        //StartCoroutine(CountGenerationTime());

        LoadPrefabs();

        GetAllComponents();

        SetDifficulty();

        //PGO PGOscript = 


    }

    void GetAllComponents()
    {
        _cameraTransitionScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraTransition>();
    }



    public void SetDifficulty()
    {

        switch (difficulty)
        {
            case (Difficulty.EASY):

                _highGapLengthAmount = 4;
                _lowGapLengthAmount = 1;

                _spikeFrequencyHighAmount = 4;

                _spikeFrequencyLowAmount = 3;

                _spawnSecondSpikeChance = 4;

                _PGOFrequencyHighAmount = 9;

                _PGOFrequencyLowAmount = 6;



                //_spikeFrequency = 4;




                break;

            case (Difficulty.NORMAL):

                _highGapLengthAmount = 5;
                _lowGapLengthAmount = 2;

                _spikeFrequencyHighAmount = 3;

                _spikeFrequencyLowAmount = 2;

                _spawnSecondSpikeChance = 3;


                _PGOFrequencyHighAmount = 7;

                _PGOFrequencyLowAmount = 5;


                break;

            case (Difficulty.HARD):

                _highGapLengthAmount = 6;
                _lowGapLengthAmount = 3;

                _spikeFrequencyHighAmount = 3;

                _spikeFrequencyLowAmount = 1;


                _spawnSecondSpikeChance = 2;

                _PGOFrequencyHighAmount = 4;

                _PGOFrequencyLowAmount = 2;



                break;

        }

    }

    void LoadPrefabs()
    {
        _shortLengthPlatform = Resources.Load("Level Generator/ShortLengthPlatform") as GameObject;
        _midLengthPlatform = Resources.Load("Level Generator/MidLengthPlatform") as GameObject;
        _longLengthPlatform = Resources.Load("Level Generator/LongLengthPlatform") as GameObject;

        _spike = Resources.Load("Level Generator/SingleSpike") as GameObject;
        _fourSpikes = Resources.Load("Level Generator/FourSpikes") as GameObject;
        _goalMaterial = Resources.Load("Level Generator/GoalPlatform") as Material;
    }


    // ON BUTTON CLICK
    public void Generate()
    {



        Debug.Log("GENERATE TRIGGERD");
        platformSpawnPos = new Vector3(0, 0, 0);
        _isGenerating = true;



        // generiere ein Segment so oft wie angegeben


        // Wenn gewünscht, generiere komplett random Directions, es kann sein dass unplayable Levels entstehen weil sie zuerst in die eine Richtung und dann in die 
        // komplett entgegengesetzte generiert werden. Dafür entstehen aber teilsweise sehr interessante Level

        if (generateRandomDirections) GenerateRandomDirections();
        // Kontrollierte Generierung der Directions, hier wird auch random generiert aber verhindert, dass nach der Generierung in eine Richtung direkt in die entgegengesetzte generiert wird
        else if (generateControlledRandomDirections) GenerateControlledRandomDirections();

        while (segmentAmount > _generatedSegments) GenerateSegment();

        Debug.Log("Segments Generated");
        Debug.Log("Start Spike Generation");

        GenerateSpikes();

        GeneratePGOs();

        _cameraTransitionScript.PGOSetup();

        GenerateGoal();

        Debug.Log(platforms.Last().name);


        _isGenerating = false;
        levelGenerated = true;
        Debug.Log("Level Generated");


    }

    void GeneratePGOs()
    {


        int platformCounter = 0;

        foreach (GameObject platform in platforms)
        {

            platformCounter++;

            _PGOFrequency = RandomNumber(_PGOFrequencyLowAmount, _PGOFrequencyHighAmount);

            platformScript = platform.GetComponent<Platform>();


            if (platformCounter % _PGOFrequency == 0 && platformScript.firstPlatform == false)
            {

                // Weise pgo skript zu und enable es

                PGO PGOscript = platform.AddComponent<PGO>();
                PGOscript.enabled = true;

                float randomRelocation = RandomNumber(3, 6);

                Debug.Log(platform.transform.position);

                // PGOscript.worldAxisTargetPoint = platform.transform.position.z + randomRelocation;

                // setze Achse des PGOs 

                switch (platformScript.currentAxis)
                {
                    case Platform.CurrentAxis.xPositive:

                        PGOscript.worldAxis = PGO.WorldAxis.PGOxPositive;

                        PGOscript.worldAxisTargetPoint = platform.transform.position.z + randomRelocation;

                        PGOscript.PlatformSetup();


                        break;



                    case Platform.CurrentAxis.zPositive:

                        PGOscript.worldAxis = PGO.WorldAxis.PGOzPositive;

                        PGOscript.worldAxisTargetPoint = platform.transform.position.x - randomRelocation;

                        PGOscript.PlatformSetup();



                        break;


                    case Platform.CurrentAxis.xNegative:


                        PGOscript.worldAxis = PGO.WorldAxis.PGOxNegative;

                        PGOscript.worldAxisTargetPoint = platform.transform.position.z - randomRelocation;

                        PGOscript.PlatformSetup();



                        break;

                    case Platform.CurrentAxis.zNegative:

                        PGOscript.worldAxis = PGO.WorldAxis.PGOzNegative;

                        PGOscript.worldAxisTargetPoint = platform.transform.position.x + randomRelocation;

                        PGOscript.PlatformSetup();



                        break;
                }

            }

        }


        // weise bestimmte PLattformen PGO-Skripte zu

        // setze Worldaxis in diesem Skript auf momentane Achse -> zwischenspeicherung des PGOs

        //  

        // verschiebe das PGO auf der jeweiligen Achse um -Wert- Meter (das ist dann worlaxistargetpoint)

        // weise worldaxistargetpoint im pgoscript zu 
    }

    void GenerateGoal()
    {
        GameObject lastPlatform = platforms.Last();

        lastPlatform.transform.Find("Cube").gameObject.GetComponent<MeshRenderer>().material = _goalMaterial;

        // Hier noch Collider in "Ziel" ändern, damit Szene neu geladen wird

        //Object.GetComponent<MeshRenderer> ().material = Material1;Object.GetComponent<MeshRenderer> ().material = Material1;

    }

    void GenerateSpikes()
    {

        int platformCounter = 0;

        foreach (GameObject platform in platforms)
        {
            if (!_firstPlatformIgnored)
            {
                platformCounter++;
                _firstPlatformIgnored = true;

            }

            else
            {

                platformCounter++;



                _spikeFrequency = RandomNumber(_spikeFrequencyLowAmount, _spikeFrequencyHighAmount);

                platformScript = platform.GetComponent<Platform>();

                GameObject instantiatedSpike = null;



                if (platformCounter % _spikeFrequency == 0)
                {

                    switch (platformScript.currentAxis)
                    {
                        case Platform.CurrentAxis.xPositive:


                            if (platform.name == "ShortLengthPlatform(Clone)")
                            {
                                instantiatedSpike = Instantiate(_spike, new Vector3(platform.transform.position.x + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);

                                SetPlatformAsParent(instantiatedSpike, platform);

                                Debug.Log("(xPos) SingleSpike generated at: " + new Vector3(platform.transform.position.x + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z));
                            }

                            else if (platform.name == "LongLengthPlatform(Clone)")
                            {
                                float platformFifth = platform.transform.Find("Cube").gameObject.transform.localScale.x / 5;
                                float randomNum = RandomNumber(0, _spawnSecondSpikeChance);


                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x + (platformFifth + 1f), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                // Chance, einen zweiten Spike zu generieren ( AUF HARD 50% )
                                if (randomNum == 0)
                                {
                                    instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x + (3 * platformFifth + 1f), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);
                                    SetPlatformAsParent(instantiatedSpike, platform);
                                }
                            }


                            //else if(platform.name == "LongLengthPlatform(Clone)") return; // Plattform in 5 Teile teilen, dann bei 2/5 spikes generieren und bei 4/5

                            else
                            {
                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                Debug.Log("(xPos) FourSpike generated at: " + new Vector3(platform.transform.position.x + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z));

                            }

                            break;



                        case Platform.CurrentAxis.zPositive:


                            if (platform.name == "ShortLengthPlatform(Clone)")
                            {

                                instantiatedSpike = Instantiate(_spike, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                Debug.Log("(zPos) SingleSpike generated at: " + new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)));
                            }

                            else if (platform.name == "LongLengthPlatform(Clone)")
                            {
                                float platformFifth = platform.transform.Find("Cube").gameObject.transform.localScale.x / 5;
                                float randomNum = RandomNumber(0, _spawnSecondSpikeChance);


                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z + (platformFifth + 1f)), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                // Chance, einen zweiten Spike zu generieren ( AUF HARD 50% )
                                if (randomNum == 0)
                                {
                                    instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z + (3 * platformFifth + 1f)), Quaternion.identity);
                                    SetPlatformAsParent(instantiatedSpike, platform);
                                }
                            }

                            else
                            {
                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                Debug.Log("(zPos) FourSpike generated at: " + new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)));

                            }






                            break;


                        case Platform.CurrentAxis.xNegative:


                            if (platform.name == "ShortLengthPlatform(Clone)")
                            {

                                instantiatedSpike = Instantiate(_spike, new Vector3(platform.transform.position.x - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                Debug.Log("(xNeg) SingleSpike generated at: " + new Vector3(platform.transform.position.x - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z));
                            }

                            else if (platform.name == "LongLengthPlatform(Clone)")
                            {
                                float platformFifth = platform.transform.Find("Cube").gameObject.transform.localScale.x / 5;
                                float randomNum = RandomNumber(0, _spawnSecondSpikeChance);


                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x - (platformFifth + 1f), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                // Chance, einen zweiten Spike zu generieren ( AUF Schwierigkeitsgrad HARD 50% )
                                if (randomNum == 0)
                                {
                                    instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x - (3 * platformFifth + 1f), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);
                                    SetPlatformAsParent(instantiatedSpike, platform);
                                }
                            }

                            else
                            {
                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                Debug.Log("(xNeg) FourSpike generated at: " + new Vector3(platform.transform.position.x - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z));

                            }


                            break;

                        case Platform.CurrentAxis.zNegative:


                            if (platform.name == "ShortLengthPlatform(Clone)")
                            {

                                instantiatedSpike = Instantiate(_spike, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                Debug.Log("(zNeg) SingleSpike generated at: " + new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)));
                            }

                            else if (platform.name == "LongLengthPlatform(Clone)")
                            {
                                float platformFifth = platform.transform.Find("Cube").gameObject.transform.localScale.x / 5;
                                float randomNum = RandomNumber(0, _spawnSecondSpikeChance);


                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z - (platformFifth + 1f)), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                // Chance, einen zweiten Spike zu generieren ( AUF HARD 50% )
                                if (randomNum == 0)
                                {
                                    instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z - (3 * platformFifth + 1f)), Quaternion.identity);
                                    SetPlatformAsParent(instantiatedSpike, platform);
                                }
                            }

                            else
                            {
                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                Debug.Log("(zNeg) FourSpike generated at: " + new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)));

                            }






                            break;
                    }


                }
            }


        }
    }

    void SetPlatformAsParent(GameObject spike, GameObject platform)
    {
        spike.transform.parent = platform.transform;
    }

    void GenerateRandomDirections()
    {
        Debug.Log("Random Generationorder gesetzt");

        generationOrder[0] = 0;

        for (int i = 1; i < segmentAmount; i++)
        {
            generationOrder[i] = RandomNumber(0, 4);

        }

    }

    void GenerateControlledRandomDirections()
    {
        generationOrder[0] = 0;

        int lastDirection = 0;

        for (int i = 1; i < segmentAmount; i++)
        {
            int[] possibleWays = { 0, 0, 0 };

            switch (lastDirection)
            {
                case 0:
                    possibleWays = new int[] { 0, 1, 3 };
                    break;

                case 1:
                    possibleWays = new int[] { 0, 1, 2 };
                    break;

                case 2:
                    possibleWays = new int[] { 1, 2, 3 };
                    break;

                case 3:
                    possibleWays = new int[] { 0, 2, 3 };
                    break;
            }

            int targetIndex = RandomNumber(0, possibleWays.Length);

            int newDirection = possibleWays[targetIndex];

            generationOrder[i] = newDirection;

            lastDirection = newDirection;


        }





    }

    void GenerateSegment()
    {

        SetSpawnAxis();
        
        segmentGenerated = false;

        _countSegmentLength = 0;

        while (_countSegmentLength < maxSegmentLength)
        {

            GetRandomPlatform();

            InstantiatePlatform();

            SetNewPlatformSpawnPos();

        }

        _generatedSegments += 1;

        _firstPlatformMarked = false;

        segmentGenerated = true;
        _remainingSegmentLength = 0;

    }

    void SetSpawnAxis()
    {

        // bestimmt die neue Richtung, in die nach einem Segment generiert wird
        // Gleicht die Rotation der zu spawnenden Plattformen an die entsprechende Achse an

        if (generationOrder[_directionCounter] == 0)
        {
            _spawnAxis = 0;

            currentAxis = CurrentAxis.xPositive;

        }
        else if (generationOrder[_directionCounter] == 1)
        {
            _spawnAxis = 270;

            currentAxis = CurrentAxis.zPositive;


        }
        else if (generationOrder[_directionCounter] == 2)
        {
            _spawnAxis = 180;

            currentAxis = CurrentAxis.xNegative;


        }

        else if (generationOrder[_directionCounter] == 3)
        {
            _spawnAxis = 90;

            currentAxis = CurrentAxis.zNegative;


        }

        Debug.Log("Setting current Axis to: " + currentAxis);

        _directionCounter += 1;


    }

    public void ResetAllRelevantVariables()
    {


        _generatedSegments = 0;
        _directionCounter = 0;
        levelGenerated = false;
        _firstPlatformIgnored = false;
        platforms = new List<GameObject>();

        //ResetCameraTransitionScript();

    }

    void ResetCameraTransitionScript()
    {
        _cameraTransitionScript.arrayPGOxPositive = null;
        _cameraTransitionScript.arrayPGOxNegative = null;
        _cameraTransitionScript.arrayPGOzPositive = null;
        _cameraTransitionScript.arrayPGOzNegative = null;

        _cameraTransitionScript.transformScriptsPGOxPositive = null;
        _cameraTransitionScript.transformScriptsPGOxNegative = null;
        _cameraTransitionScript.transformScriptsPGOzPositive = null;
        _cameraTransitionScript.transformScriptsPGOzNegative = null;


    }




    void GetRandomPlatform()
    {


        _randomNumber = RandomNumber(0, 3);

        switch (_randomNumber)
        {
            case 0:
                myPrefab = _shortLengthPlatform;
                break;

            case 1:
                myPrefab = _midLengthPlatform;
                break;

            case 2:
                myPrefab = _longLengthPlatform;
                break;
        }

    }



    void InstantiatePlatform()
    {
        //Plattform wird zur Liste hinzugefügt und direkt generiert

        prefabCopy = Instantiate(myPrefab, platformSpawnPos, Quaternion.Euler(0, _spawnAxis, 0));

        AddPlatformToList(prefabCopy);


        if(!_firstPlatformMarked) {

            prefabCopy.GetComponent<Platform>().firstPlatform = true;

            //MarkFirstPlatformOfSegment();
            _firstPlatformMarked = true;

        }

    }


    void AddPlatformToList(GameObject platform)
    {
        platforms.Add(platform);

    }


    void SetNewPlatformSpawnPos()
    {

        Debug.Log("Current Axis: " + currentAxis);

        float _gapLength = RandomNumber(_lowGapLengthAmount, _highGapLengthAmount);

        //prefabCoppy
        //_myPrefabChild

        _myPrefabChild = prefabCopy.transform.Find("Cube").gameObject;


        // prüft, ob Gap zu groß ist. Wenn ja, wird gap verringert
        // BUG: wenn gap verringert wird, wird die PF aus dem nächsten Segment manchmal vertikal versetzt an die Stelle des Anfangs des nächsten Segments gespawnt. Segmente überlappen sich teilweise (schlecht?)

        if ((_gapLength + _myPrefabChild.transform.localScale.x + _countSegmentLength) > maxSegmentLength)
        {

            //MarkLastPlatformOfSegment();


            _gapLength -= ((_gapLength + _myPrefabChild.transform.localScale.x + _countSegmentLength) - maxSegmentLength);


        }

        SetPlatformAxis();


        SetSpawnPointAxisDependent(_myPrefabChild, _gapLength);


        // AUCH AN Z ANPASSEN DAMIT BERECHNUNG KORREKT IST

        //Debug.Log(platformSpawnPos.x + _myPrefabChild.transform.localScale.x + _gapLength + " = " + platformSpawnPos.x + " + " + _myPrefabChild.transform.localScale.x + " + " + _gapLength);


        // festlegung Y-Achse: platFormVerticalRange legt fest, wie hoch die Abstände auf der Y-Achse der Plattformen zueinander sein können

        SetPlatformYPoint();

        CountSegmentLength(_gapLength);


    }

    void SetPlatformAxis()
    {

        platformScript = prefabCopy.GetComponent<Platform>();

        switch (currentAxis)
        {
            case CurrentAxis.xPositive:

                //prefabCopy.GetComponent<Platform>().currentAxis = Platform.CurrentAxis.xPositive;

                platformScript.currentAxis = Platform.CurrentAxis.xPositive;

                break;

            case CurrentAxis.zPositive:

                //prefabCopy.GetComponent<Platform>().currentAxis = Platform.CurrentAxis.zPositive;

                platformScript.currentAxis = Platform.CurrentAxis.zPositive;
                break;

            case CurrentAxis.xNegative:

                //prefabCopy.GetComponent<Platform>().currentAxis = Platform.CurrentAxis.xNegative;

                platformScript.currentAxis = Platform.CurrentAxis.xNegative;

                break;

            case CurrentAxis.zNegative:

                //prefabCopy.GetComponent<Platform>().currentAxis = Platform.CurrentAxis.zNegative;

                platformScript.currentAxis = Platform.CurrentAxis.zNegative;

                break;
        }






    }

    void SetPlatformYPoint()
    {

        switch (verticalAlignment)
        {
            case (VerticalAlignment.Even):
                platformSpawnPos.y += RandomNumber(-platformVerticalRange + 1, platformVerticalRange);
                break;

            case (VerticalAlignment.Upwards):
                platformSpawnPos.y += RandomNumber(0, platformVerticalRange);
                break;

            case (VerticalAlignment.Downwards):
                platformSpawnPos.y += RandomNumber(-platformVerticalRange, 0);
                break;

        }



    }

    void CountSegmentLength(float gapLength)
    {
        _countSegmentLength += (int)_myPrefabChild.transform.localScale.x + gapLength;
    }


    void SetSpawnPointAxisDependent(GameObject platform, float gapLength)
    {

        //platformSpawnPos.x += platform.transform.localScale.x + gapLength;



        switch (currentAxis)
        {
            case CurrentAxis.xPositive:

                platformSpawnPos.x += platform.transform.localScale.x + gapLength;

                break;

            case CurrentAxis.zPositive:

                platformSpawnPos.z += platform.transform.localScale.x + gapLength;

                break;

            case CurrentAxis.xNegative:

                platformSpawnPos.x -= platform.transform.localScale.x + gapLength;

                break;

            case CurrentAxis.zNegative:

                platformSpawnPos.z -= platform.transform.localScale.x + gapLength;

                break;


        }

    }

    public IEnumerator CountGenerationTime()
    {
        float generationTime = 0;

        while (!levelGenerated)
        {
            generationTime += Time.deltaTime;

        }

        //if(levelGenerated) 
        Debug.Log("Generation Time: " + generationTime);

        yield return null;
    }




    public int RandomNumber(int min, int max)
    {
        return Random.Range(min, max);
    }
}
