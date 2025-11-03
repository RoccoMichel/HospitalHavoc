using UnityEngine;

[CreateAssetMenu(fileName = "new LevelData", menuName = "Scriptable Objects/LevelDataObject")]
public class LevelDataObject : ScriptableObject
{
    public float GameLengthSeconds = 120f;
    public int startMoney = 100;
    public float moneyDrainRate = 0.1f;
    public float newPatientRate = 0.05f;
}
