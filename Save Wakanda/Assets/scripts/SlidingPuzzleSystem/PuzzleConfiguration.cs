using UnityEngine;

namespace SlidingPuzzle
{
    /// <summary>
    /// ScriptableObject to configure individual puzzles
    /// Create via: Assets > Create > Sliding Puzzle > Puzzle Config
    /// </summary>
    [CreateAssetMenu(fileName = "PuzzleConfig", menuName = "Sliding Puzzle/Puzzle Config")]
    public class PuzzleConfiguration : ScriptableObject
    {
        [Header("Puzzle Setup")]
        [Tooltip("The image/texture to use for this puzzle")]
        public Texture2D puzzleImage;
        
        [Tooltip("Grid size (3 = 3x3 puzzle with 8 tiles)")]
        [Range(2, 5)]
        public int gridSize = 3;
        
        [Header("Reward")]
        [Tooltip("The mask prefab to spawn when puzzle is solved")]
        public GameObject maskPrefab;
        
        [Header("Ghost Defeat")]
        [Tooltip("Reference to the ghost GameObject - leave empty to find by tag 'Ghost' at runtime")]
        public GameObject ghostObject;
        
        [Tooltip("Tag to find ghost if not manually assigned")]
        public string ghostTag = "Ghost";
        
        [Tooltip("Animator parameter name to trigger defeat animation")]
        public string defeatAnimationTrigger = "Defeat";
        
        [Tooltip("Delay before destroying ghost after defeat animation starts")]
        public float ghostDestroyDelay = 3f;
    }
}
