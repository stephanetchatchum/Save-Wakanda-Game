using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Editor utility to quickly create level data assets.
/// This script helps you create Level 1, 2, and 3 data files.
/// 
/// HOW TO USE:
/// 1. In Unity, go to: Tools → Puzzle Game → Create All Levels
/// 2. This creates 3 level assets in Assets/Levels/
/// 3. Assign your mask sprites to each level manually
/// </summary>
#if UNITY_EDITOR
public class LevelCreator : EditorWindow
{
    [MenuItem("Tools/Puzzle Game/Create All Levels")]
    static void CreateLevels()
    {
        // Create Levels folder if it doesn't exist
        if (!AssetDatabase.IsValidFolder("Assets/Levels"))
        {
            AssetDatabase.CreateFolder("Assets", "Levels");
        }

        // Level 1 - Easy (3x3)
        CreateLevel(
            levelNumber: 1,
            levelName: "Easy Mask",
            gridSize: 3,
            shuffleMoves: 30,
            fileName: "Level1"
        );

        // Level 2 - Medium (4x4)
        CreateLevel(
            levelNumber: 2,
            levelName: "Medium Mask",
            gridSize: 4,
            shuffleMoves: 50,
            fileName: "Level2"
        );

        // Level 3 - Hard (5x5)
        CreateLevel(
            levelNumber: 3,
            levelName: "Hard Mask",
            gridSize: 5,
            shuffleMoves: 80,
            fileName: "Level3"
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("✅ Created 3 levels in Assets/Levels/");
        Debug.Log("⚠️ Don't forget to assign mask sprites to each level!");
    }

    static void CreateLevel(int levelNumber, string levelName, int gridSize, int shuffleMoves, string fileName)
    {
        LevelData level = ScriptableObject.CreateInstance<LevelData>();
        
        level.levelNumber = levelNumber;
        level.levelName = levelName;
        level.gridSize = gridSize;
        level.shuffleMoves = shuffleMoves;
        
        // Default colors
        level.tileBackgroundColor = Color.white;
        level.gridBackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);

        string path = $"Assets/Levels/{fileName}.asset";
        AssetDatabase.CreateAsset(level, path);
        
        Debug.Log($"Created: {path}");
    }

    [MenuItem("Tools/Puzzle Game/Open Levels Folder")]
    static void OpenLevelsFolder()
    {
        // Select Levels folder in Project window
        Object obj = AssetDatabase.LoadAssetAtPath("Assets/Levels", typeof(Object));
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);
    }
}
#endif
