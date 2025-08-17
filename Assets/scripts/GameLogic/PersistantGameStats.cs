using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

//Persistent singleton that survives scene loads and holds the stats like hands played etc 
// Use ResetStats(totalCustomers) to reset before a level.
public class PersistentGameStats : MonoBehaviour
{
    public static PersistentGameStats Instance { get; private set; }

    public int handsPlayed;
    public int customersSatisfied;
    public int totalCustomersInLevel;
    public bool levelWon; // to say if you win 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[PersistentGameStats] Awake - instance created");
    }
    public void ResetStats(int totalCustomers)
    {
        handsPlayed = 0;
        customersSatisfied = 0;
        totalCustomersInLevel = totalCustomers;
        levelWon = false;
        Debug.Log($"[PersistentGameStats] ResetStats totalCustomers={totalCustomers}");
    }
}
