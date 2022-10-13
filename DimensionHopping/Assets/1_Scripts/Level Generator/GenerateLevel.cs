using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenerateLevel : MonoBehaviour
{

    public int[] generationOrder;
    [HideInInspector] public GameObject myPrefab;
    private GameObject _shortLengthPlatform;
    private GameObject _midLengthPlatform;
    private GameObject _longLengthPlatform;
    private GameObject _spike;
    private GameObject _fourSpikes;
    private GameObject _goal;
    [HideInInspector] public List<GameObject> platforms = new List<GameObject>();
    public int maxSegmentLength;
    private float _countSegmentLength;
    private GameObject _myPrefabChild;
    public int segmentAmount;
    public int platformVerticalRange;
    public VerticalAlignment verticalAlignment;
    private int _randomNumber;
    [HideInInspector] public Vector3 platformSpawnPos;
    private int _highGapLengthAmount;
    private int _lowGapLengthAmount;
    private int _generatedSegments;
    private int _directionCounter;
    private bool _isGenerating;
    private int _remainingSegmentLength;
    [HideInInspector] public bool levelGenerated;
    [HideInInspector] public bool segmentGenerated;
    private float _spawnAxis;
    [HideInInspector] public Platform platformScript;
    private CameraTransition _cameraTransitionScript;
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
    public Difficulty difficulty;
    private CurrentAxis currentAxis;
    [SerializeField] private GameObject _deathZone;
    public float deathZoneDepth;
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    public enum CurrentAxis
    {
        XPositive,
        XNegative,
        ZPositive,
        ZNegative
    }
    public enum VerticalAlignment
    {
        Downwards,
        Even,
        Upwards
    }



    public void Setup()
    {

        Debug.Log("Setup");

        LoadPrefabs();

        GetAllComponents();

        SetDifficulty();

        generationOrder = new int[segmentAmount];


    }

    void GetAllComponents()
    {
        _cameraTransitionScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraTransition>();
    }



    public void SetDifficulty()
    {

        switch (difficulty)
        {
            case (Difficulty.Easy):


                _highGapLengthAmount = 4;
                _lowGapLengthAmount = 1;

                _spikeFrequencyHighAmount = 4;

                _spikeFrequencyLowAmount = 3;

                _spawnSecondSpikeChance = 4;

                _PGOFrequencyHighAmount = 9;

                _PGOFrequencyLowAmount = 6;




                break;

            case (Difficulty.Normal):

                _highGapLengthAmount = 5;
                _lowGapLengthAmount = 2;

                _spikeFrequencyHighAmount = 3;

                _spikeFrequencyLowAmount = 2;

                _spawnSecondSpikeChance = 3;


                _PGOFrequencyHighAmount = 7;

                _PGOFrequencyLowAmount = 5;


                break;

            case (Difficulty.Hard):

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
        _deathZone = Resources.Load("Level Generator/DeathZone") as GameObject;


        _goal = Resources.Load("Level Generator/Goal") as GameObject;
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

        GenerateDeathZone();


        _isGenerating = false;
        levelGenerated = true;
        Debug.Log("Level Generated");


    }

    void GenerateDeathZone()
    {


        switch(verticalAlignment)
        {
            case (VerticalAlignment.Upwards):


            Instantiate(_deathZone, new Vector3(platforms[0].transform.position.x, platforms[0].transform.position.y - deathZoneDepth, platforms[0].transform.position.z), Quaternion.identity);
            
            // generiere death zone an punkt von erster plattform -y wert

            break;

            case (VerticalAlignment.Even):

            Instantiate(_deathZone, new Vector3(platforms[0].transform.position.x, platforms[0].transform.position.y - deathZoneDepth, platforms[0].transform.position.z), Quaternion.identity);

            // generiere death zone an punkt von erster plattform -y wert

            break;

            case (VerticalAlignment.Downwards):
            
            Instantiate(_deathZone, new Vector3(platforms.Last().transform.position.x, platforms.Last().transform.position.y - deathZoneDepth, platforms.Last().transform.position.z), Quaternion.identity);


            // generiere death zone an punkt von letzter plattform -y wert

            break;

        }

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


                // setze Achse des PGOs 

                switch (platformScript.currentAxis)
                {
                    case Platform.CurrentAxis.XPositive:

                        PGOscript.worldAxis = PGO.WorldAxis.PGOxPositive;

                        PGOscript.worldAxisTargetPoint = platform.transform.position.z + randomRelocation;

                        PGOscript.PlatformSetup();


                        break;



                    case Platform.CurrentAxis.ZPositive:

                        PGOscript.worldAxis = PGO.WorldAxis.PGOzPositive;

                        PGOscript.worldAxisTargetPoint = platform.transform.position.x - randomRelocation;

                        PGOscript.PlatformSetup();



                        break;


                    case Platform.CurrentAxis.XNegative:


                        PGOscript.worldAxis = PGO.WorldAxis.PGOxNegative;

                        PGOscript.worldAxisTargetPoint = platform.transform.position.z - randomRelocation;

                        PGOscript.PlatformSetup();



                        break;

                    case Platform.CurrentAxis.ZNegative:

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
                        case Platform.CurrentAxis.XPositive:


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



                            else
                            {
                                instantiatedSpike = Instantiate(_fourSpikes, new Vector3(platform.transform.position.x + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);
                                SetPlatformAsParent(instantiatedSpike, platform);
                                Debug.Log("(xPos) FourSpike generated at: " + new Vector3(platform.transform.position.x + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z));

                            }

                            break;



                        case Platform.CurrentAxis.ZPositive:


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


                        case Platform.CurrentAxis.XNegative:


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

                        case Platform.CurrentAxis.ZNegative:


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

    void GenerateGoal()
    {


        GameObject lastPlatform = platforms.Last();

        platformScript = lastPlatform.GetComponent<Platform>();

        GameObject instantiatedGoal;

        lastPlatform.transform.Find("Cube").gameObject.GetComponent<MeshRenderer>().material = _goalMaterial;


        switch (platformScript.currentAxis)
                {
                    case Platform.CurrentAxis.XPositive:

                        instantiatedGoal = Instantiate(_goal, new Vector3(lastPlatform.transform.position.x + (lastPlatform.transform.Find("Cube").gameObject.transform.localScale.x / 2), lastPlatform.transform.position.y + 1, lastPlatform.transform.position.z), Quaternion.identity);

                        break;

                    case Platform.CurrentAxis.ZPositive:

                        instantiatedGoal = Instantiate(_goal, new Vector3(lastPlatform.transform.position.x, lastPlatform.transform.position.y + 1, lastPlatform.transform.position.z + (lastPlatform.transform.Find("Cube").gameObject.transform.localScale.x / 2)), Quaternion.identity);

                        //Instantiate(_spike, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z + (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)), Quaternion.identity);

                        break;


                    case Platform.CurrentAxis.XNegative:

                        instantiatedGoal = Instantiate(_goal, new Vector3(lastPlatform.transform.position.x - (lastPlatform.transform.Find("Cube").gameObject.transform.localScale.x / 2), lastPlatform.transform.position.y + 1, lastPlatform.transform.position.z), Quaternion.identity);

                        //Instantiate(_spike, new Vector3(platform.transform.position.x - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2), platform.transform.position.y + 0.5f, platform.transform.position.z), Quaternion.identity);

                        break;

                    case Platform.CurrentAxis.ZNegative:

                        instantiatedGoal = Instantiate(_goal, new Vector3(lastPlatform.transform.position.x, lastPlatform.transform.position.y + 1, lastPlatform.transform.position.z - (lastPlatform.transform.Find("Cube").gameObject.transform.localScale.x / 2)), Quaternion.identity);

                        //Instantiate(_spike, new Vector3(platform.transform.position.x, platform.transform.position.y + 0.5f, platform.transform.position.z - (platform.transform.Find("Cube").gameObject.transform.localScale.x / 2)), Quaternion.identity);

                        break;
                }


        if(lastPlatform.transform.Find("SingleSpike(Clone)") != null) Destroy(lastPlatform.transform.Find("SingleSpike(Clone)").gameObject);
        else if (lastPlatform.transform.Find("FourSpikes(Clone)") != null) Destroy(lastPlatform.transform.Find("FourSpikes(Clone)").gameObject);
        

    }

    void SetPlatformAsParent(GameObject spike, GameObject platform)
    {
        spike.transform.parent = platform.transform;
    }

    void GenerateRandomDirections()
    {
        Debug.Log("Random Generationorder gesetzt");

        //generationOrder.Length = 10;


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

            currentAxis = CurrentAxis.XPositive;

        }
        else if (generationOrder[_directionCounter] == 1)
        {
            _spawnAxis = 270;

            currentAxis = CurrentAxis.ZPositive;


        }
        else if (generationOrder[_directionCounter] == 2)
        {
            _spawnAxis = 180;

            currentAxis = CurrentAxis.XNegative;


        }

        else if (generationOrder[_directionCounter] == 3)
        {
            _spawnAxis = 90;

            currentAxis = CurrentAxis.ZNegative;


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


        if (!_firstPlatformMarked)
        {

            prefabCopy.GetComponent<Platform>().firstPlatform = true;

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

        _myPrefabChild = prefabCopy.transform.Find("Cube").gameObject;


        // prüft, ob Gap zu groß ist. Wenn ja, wird gap verringert
        // BUG: wenn gap verringert wird, wird die PF aus dem nächsten Segment manchmal vertikal versetzt an die Stelle des Anfangs des nächsten Segments gespawnt. Segmente überlappen sich teilweise (schlecht?)

        if ((_gapLength + _myPrefabChild.transform.localScale.x + _countSegmentLength) > maxSegmentLength)
        {

            _gapLength -= ((_gapLength + _myPrefabChild.transform.localScale.x + _countSegmentLength) - maxSegmentLength);


        }

        SetPlatformAxis();


        SetSpawnPointAxisDependent(_myPrefabChild, _gapLength);


        // festlegung Y-Achse: platFormVerticalRange legt fest, wie hoch die Abstände auf der Y-Achse der Plattformen zueinander sein können

        SetPlatformYPoint();

        CountSegmentLength(_gapLength);


    }

    void SetPlatformAxis()
    {

        platformScript = prefabCopy.GetComponent<Platform>();

        switch (currentAxis)
        {
            case CurrentAxis.XPositive:



                platformScript.currentAxis = Platform.CurrentAxis.XPositive;

                break;

            case CurrentAxis.ZPositive:



                platformScript.currentAxis = Platform.CurrentAxis.ZPositive;
                break;

            case CurrentAxis.XNegative:



                platformScript.currentAxis = Platform.CurrentAxis.XNegative;

                break;

            case CurrentAxis.ZNegative:



                platformScript.currentAxis = Platform.CurrentAxis.ZNegative;

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



        switch (currentAxis)
        {
            case CurrentAxis.XPositive:

                platformSpawnPos.x += platform.transform.localScale.x + gapLength;

                break;

            case CurrentAxis.ZPositive:

                platformSpawnPos.z += platform.transform.localScale.x + gapLength;

                break;

            case CurrentAxis.XNegative:

                platformSpawnPos.x -= platform.transform.localScale.x + gapLength;

                break;

            case CurrentAxis.ZNegative:

                platformSpawnPos.z -= platform.transform.localScale.x + gapLength;

                break;


        }

    }


    public int RandomNumber(int min, int max)
    {
        return Random.Range(min, max);
    }
}
