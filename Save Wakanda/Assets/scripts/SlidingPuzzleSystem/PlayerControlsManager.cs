using UnityEngine;

namespace SlidingPuzzle
{
    /// <summary>
    /// Optional helper script for disabling multiple player components during puzzle
    /// Attach this to your player instead of referencing individual controllers
    /// </summary>
    public class PlayerControlsManager : MonoBehaviour
    {
        [Header("Components to Disable During Puzzle")]
        public MonoBehaviour[] controllerScripts;
        public Rigidbody playerRigidbody;
        
        private bool[] originalEnabledStates;
        private bool originalKinematicState;
        
        void Awake()
        {
            // Store original states
            if (controllerScripts != null && controllerScripts.Length > 0)
            {
                originalEnabledStates = new bool[controllerScripts.Length];
                for (int i = 0; i < controllerScripts.Length; i++)
                {
                    if (controllerScripts[i] != null)
                    {
                        originalEnabledStates[i] = controllerScripts[i].enabled;
                    }
                }
            }
            
            if (playerRigidbody != null)
            {
                originalKinematicState = playerRigidbody.isKinematic;
            }
        }
        
        /// <summary>
        /// Disable all player controls
        /// </summary>
        public void DisableControls()
        {
            if (controllerScripts != null)
            {
                foreach (var script in controllerScripts)
                {
                    if (script != null)
                    {
                        script.enabled = false;
                    }
                }
            }
            
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.isKinematic = true;
            }
        }
        
        /// <summary>
        /// Re-enable all player controls
        /// </summary>
        public void EnableControls()
        {
            if (controllerScripts != null && originalEnabledStates != null)
            {
                for (int i = 0; i < controllerScripts.Length; i++)
                {
                    if (controllerScripts[i] != null && i < originalEnabledStates.Length)
                    {
                        controllerScripts[i].enabled = originalEnabledStates[i];
                    }
                }
            }
            
            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = originalKinematicState;
            }
        }
    }
}
