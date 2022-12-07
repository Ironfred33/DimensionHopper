using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GeneratorOptions : MonoBehaviour
{

    public GenerateLevel levelGeneration;

    public TMP_Dropdown difficulty;

    public TMP_Dropdown verticalAlignment;

    public TMP_InputField segmentAmount;
    public TMP_InputField segmentLength;

  


    public void OverWriteGenerationOptions()
    {
        SetDifficulty();
        SetVerticalAlignment();
        SetSegmentAmount();
        SetSegmentLength();
    }



    void SetSegmentAmount()
    {
        levelGeneration.segmentAmount =  StringToInt(segmentAmount.text);

    }

    void SetSegmentLength()
    {
        levelGeneration.maxSegmentLength =  StringToInt(segmentLength.text);

    }


    int StringToInt(string text)
    {
        int amount = 0;

        if(int.TryParse(text, out amount)) amount = Convert.ToInt32(text);
        else amount = 10;

        return amount;

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
