using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Manages UI slider and the text 
public class SatisfactionMeter : MonoBehaviour
{
    public TMP_Text scoreText;  // cureentScore
    public Slider meterSlider;  // bar assign both inspector
    //Updates slider and text
    public void UpdateMeter(int currentScore)
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();

        if (meterSlider != null)
            meterSlider.value = currentScore; // Assuming maxValue is already set
    }
}
