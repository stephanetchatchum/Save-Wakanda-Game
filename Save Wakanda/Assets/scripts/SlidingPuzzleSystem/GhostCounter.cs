using UnityEngine;
using UnityEngine.UI;

namespace SlidingPuzzle
{
    /// <summary>
    /// Tracks and displays the number of ghosts defeated
    /// Attach to a UI GameObject that will always be visible
    /// </summary>
    public class GhostCounter : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("Text element to display ghost count")]
        public Text ghostCountText;
        
        [Header("Display Settings")]
        [Tooltip("Format string for display. {0} = defeated, {1} = total")]
        public string displayFormat = "Ghosts Defeated: {0}/{1}";
        
        [Tooltip("Show icon instead of/with text")]
        public Image ghostIcon;
        
        [Header("Configuration")]
        [Tooltip("Total number of ghosts in the game")]
        public int totalGhosts = 3;
        
        private int ghostsDefeated = 0;
        
        public int GhostsDefeated => ghostsDefeated;
        public int TotalGhosts => totalGhosts;
        public bool AllGhostsDefeated => ghostsDefeated >= totalGhosts;
        
        void Start()
        {
            UpdateDisplay();
        }
        
        /// <summary>
        /// Call this when a ghost is defeated
        /// </summary>
        public void OnGhostDefeated()
        {
            ghostsDefeated++;
            UpdateDisplay();
            
            Debug.Log($"Ghost defeated! {ghostsDefeated}/{totalGhosts}");
            
            if (AllGhostsDefeated)
            {
                OnAllGhostsDefeated();
            }
        }
        
        /// <summary>
        /// Update the visual display
        /// </summary>
        private void UpdateDisplay()
        {
            if (ghostCountText != null)
            {
                ghostCountText.text = string.Format(displayFormat, ghostsDefeated, totalGhosts);
            }
        }
        
        /// <summary>
        /// Called when all ghosts are defeated
        /// </summary>
        private void OnAllGhostsDefeated()
        {
            Debug.Log("🎉 All ghosts defeated! Victory!");
            
            // You can add victory screen, end game logic, etc. here
            if (ghostCountText != null)
            {
                ghostCountText.color = Color.green;
                ghostCountText.text = "ALL GHOSTS DEFEATED!";
            }
        }
        
        /// <summary>
        /// Reset the counter
        /// </summary>
        public void ResetCounter()
        {
            ghostsDefeated = 0;
            UpdateDisplay();
            
            if (ghostCountText != null)
            {
                ghostCountText.color = Color.white;
            }
        }
        
        /// <summary>
        /// Set total ghosts (useful if it varies per level)
        /// </summary>
        public void SetTotalGhosts(int total)
        {
            totalGhosts = total;
            UpdateDisplay();
        }
    }
}
