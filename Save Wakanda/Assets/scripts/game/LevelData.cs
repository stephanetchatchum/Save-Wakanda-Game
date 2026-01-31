using UnityEngine;

/// <summary>
/// ScriptableObject that defines the properties of a puzzle level.
/// Create instances in Unity via: Assets > Create > Puzzle > Level Data
/// </summary>
[CreateAssetMenu(fileName = "Level", menuName = "Puzzle/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Level Info")]
    public int levelNumber;
    public string levelName;
    
    [Header("Grid Settings")]
    [Tooltip("Grid size (3 = 3x3, 4 = 4x4, 5 = 5x5)")]
    [Range(3, 5)]
    public int gridSize = 3;
    
    [Header("Mask Image")]
    [Tooltip("The complete mask image to be revealed")]
    public Sprite maskSprite;
    
    [Header("Difficulty Settings")]
    [Tooltip("Number of shuffle moves to scramble the puzzle")]
    [Range(10, 100)]
    public int shuffleMoves = 30;
    
    [Header("Optional: Custom Colors")]
    public Color tileBackgroundColor = Color.white;
    public Color gridBackgroundColor = new Color(0.2f, 0.2f, 0.2f);
}
