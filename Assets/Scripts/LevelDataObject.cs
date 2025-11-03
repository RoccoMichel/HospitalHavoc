using UnityEngine;

[CreateAssetMenu(fileName = "new LevelData", menuName = "Scriptable Objects/LevelDataObject")]
public class LevelDataObject : ScriptableObject
{
    public float GameLengthSeconds = 120f;
    public int startMoney = 100;
    public float moneyDrainRate = 0.5f;
    [Range(0.001f, 1)] public float newPatientRate = 0.05f;
    [Tooltip("IN ORDER: D, C, B, A, S")]
    public int[] rankRequirements = { 0, 50, 80, 120, 150 };
}
