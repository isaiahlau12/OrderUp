using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SatisfactionMeter : MonoBehaviour
{
    public Slider meterSlider;
    private TextMeshProUGUI scoreText;
    private void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("Currentscore").GetComponent<TextMeshProUGUI>();
        Debug.Log(scoreText)
                ; 
    }

    public void UpdateMeter(int currentScore, int scoreToBeat)
    {
        if (scoreText != null)
            scoreText.SetText( $"{currentScore} / {scoreToBeat}");

        if (meterSlider != null)
        {
            meterSlider.maxValue = scoreToBeat;
            meterSlider.value = currentScore;
        }
    }
}
