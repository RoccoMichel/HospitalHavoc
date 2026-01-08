using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new LevelData", menuName = "Scriptable Objects/LevelDataObject")]
public class LevelDataObject : ScriptableObject
{
    [Tooltip("In Seconds")] public float GameLengthSeconds = 120f;
    public int startMoney = 100;
    [Tooltip("In Seconds")] public float moneyDrainRate = 0.5f;
    [Range(0.001f, 1)] public float newPatientRate = 0.05f;
    public float DeadPatientScorePenalty = 50;
    [Tooltip("IN ORDER: D, C, B, A, S")]
    public int[] rankRequirements = { 0, 50, 80, 120, 150 };

    public bool isInitialized;

    [Foldout("Save Data")]
    public LevelData data;

    void OnEnable()
    {
        if (!isInitialized)
        {
            data.level = SceneManager.GetActiveScene().buildIndex;
            isInitialized = true;
        }
    }

    [Button]
    void ResetSavedData()
    {
        if (data != null)
        {
            data.players = 0;
            data.score = new List<float>();
            data.bestScore = 0;
            data.rank = String.Empty;
        }
    }
}