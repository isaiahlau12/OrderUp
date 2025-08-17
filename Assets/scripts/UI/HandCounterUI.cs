using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandCounterUI : MonoBehaviour
{
    public TextMeshProUGUI handsText;

    public void SetHandsLeft(int handsLeft)
    {
        if (handsText == null)
        {
            Debug.LogWarning("[HandCounterUI] handsText is null. Assign a TextMeshProUGUI in the Inspector.");
            return;
        }

        handsText.text = $"Hands Left: {handsLeft}";
        Debug.Log($"[HandCounterUI] Updated hands left -> {handsLeft}");
    }
}
