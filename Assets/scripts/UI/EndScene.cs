using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    public TextMeshProUGUI handsPlayedText;
    public TextMeshProUGUI customersSatisfiedText;
    public TextMeshProUGUI cashRewardText;
    public TextMeshProUGUI titleText;

    [Header("Cash Settings")]
    public int basePerCustomer = 100;
    public float handsEfficiencyMultiplier = 1.0f;

    private void Start()
    {
        if (PersistentGameStats.Instance == null)
        {
            handsPlayedText.text = "Hands Played: -";
            customersSatisfiedText.text = "Customers Satisfied: - / -";
            cashRewardText.text = "Cash Reward: $0";
            if (titleText != null) titleText.text = "No stats";
            return;
        }

        int handsPlayed = PersistentGameStats.Instance.handsPlayed;
        int customersSatisfied = PersistentGameStats.Instance.customersSatisfied;
        int totalCustomers = PersistentGameStats.Instance.totalCustomersInLevel;
        bool won = PersistentGameStats.Instance.levelWon;

        handsPlayedText.text = $"Hands Played: {handsPlayed}";
        customersSatisfiedText.text = $"Customers Satisfied: {customersSatisfied}/{totalCustomers}";
        cashRewardText.text = $"Cash Reward: ${CalculateCash(customersSatisfied, totalCustomers, handsPlayed)}";
        if (titleText != null) titleText.text = won ? "You Win!" : "Closing Time";
    }

    private int CalculateCash(int satisfied, int totalCustomers, int handsPlayed)
    {
        int baseScore = satisfied * basePerCustomer;
        float ratio = totalCustomers > 0 ? (float)totalCustomers / Mathf.Max(1, handsPlayed) : 1f;
        ratio = Mathf.Clamp(ratio, 0.5f, 2.0f);
        return Mathf.RoundToInt(baseScore * (handsEfficiencyMultiplier * ratio));
    }

    // to play again 
    public void PlayAgain(string levell)
    {
        if (!string.IsNullOrEmpty(levell))
            SceneManager.LoadScene(levell);
        else
            Debug.LogError("PlayAgain called with empty scene name!");
    }
}
