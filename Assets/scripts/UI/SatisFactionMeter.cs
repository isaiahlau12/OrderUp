using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SatisfactionMeter : MonoBehaviour
{
    [Header("UI References")]
    public Slider meterSlider;                       // Drag your UI Slider here
    public TextMeshProUGUI scoreText;                // Drag your TMP text here directly

    public void UpdateMeter(int currentScore, int scoreToBeat)
    {
        // Update score text (e.g., "10 / 20")
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
