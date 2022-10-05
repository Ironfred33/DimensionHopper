using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorOptions : MonoBehaviour
{

    public GenerateLevel levelGeneration;
    public Dropdown difficulty;
    public Dropdown verticalAlignment;


    // Start is called before the first frame update
  


    public void OverWriteGenerationOptions()
    {
        SetDifficulty();
        SetVerticalAlignment();
    }

    void SetDifficulty()
    {

        switch(difficulty.value)
        {
            case(0):

            levelGeneration.difficulty = GenerateLevel.Difficulty.Easy;

            break;

            case(1):

            levelGeneration.difficulty = GenerateLevel.Difficulty.Normal;

            break;

            case(2):

            levelGeneration.difficulty = GenerateLevel.Difficulty.Hard;

            break;
        }

    }

    void SetVerticalAlignment()
    {
        switch(verticalAlignment.value)
        {
            case(0):

            levelGeneration.verticalAlignment = GenerateLevel.VerticalAlignment.Upwards;

            break;

            case(1):

            levelGeneration.verticalAlignment = GenerateLevel.VerticalAlignment.Even;

            break;

            case(2):

            levelGeneration.verticalAlignment = GenerateLevel.VerticalAlignment.Downwards;

            break;
        }
    }

}
