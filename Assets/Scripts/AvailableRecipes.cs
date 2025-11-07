using UnityEngine;

[CreateAssetMenu(fileName = "new List", menuName = "Scriptable Objects/AvailableRecipes")]
public class AvailableRecipes : ScriptableObject
{
    [Header("Available Recipes")]
    public Sprite[] pages;
}
