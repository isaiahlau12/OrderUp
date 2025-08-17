using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SatisfactionMeter : MonoBehaviour
{
    [Header("UI References")]
    public Slider meterSlider;                       
    public TextMeshProUGUI scoreText;                
    public void UpdateMeter(int currentScore, int scoreToBeat)
    {
        // Update score text 
        if (scoreText != null)
            scoreText.SetText($"{currentScore} / {scoreToBeat}");
        else
            Debug.LogWarning("SatisfactionMeter: scoreText not assigned!");

        // Update slider values
        if (meterSlider != null)
        {
            meterSlider.maxValue = scoreToBeat;
            meterSlider.value = currentScore;
        }
        else
        {
            Debug.LogWarning("SatisfactionMeter: meterSlider not assigned!");
        }
    }
}
