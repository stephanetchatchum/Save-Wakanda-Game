using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SlidingPuzzle
{
    /// <summary>
    /// Handles ghost spawning, mask reward, and ghost defeat sequence.
    /// Ghosts are Instantiated at game start so they can be safely Destroyed on defeat.
    /// </summary>
    public class MaskRewardSystem : MonoBehaviour
    {
        [Header("References")]
        public PuzzleConfiguration currentConfig;
        public GhostCounter ghostCounter;
        
        [Header("Spawn Settings")]
        [Tooltip("The player transform - mask will spawn above them")]
        public Transform playerTransform;
        
        [Tooltip("Height above player to spawn mask")]
        public float spawnHeightAbovePlayer = 2f;
        
        [Tooltip("Forward offset from player")]
        public float spawnForwardOffset = 1f;
        
        [Header("Ghost Defeat Motion")]
        [Tooltip("How high the ghost floats up before disappearing")]
        public float ghostFloatHeight = 5f;
        
        [Header("Ghost Spawn Points")]
        [Tooltip("One spawn point per puzzle, in the same order as puzzleConfigs on PuzzleGameManager. Drag empty GameObjects from the Hierarchy here.")]
        public List<GameObject> ghostSpawnPoints = new List<GameObject>();
        
        [Header("Effects (Optional)")]
        public ParticleSystem spawnEffect;
        public ParticleSystem explosionEffect;
        public AudioClip maskSpawnSound;
        public AudioClip explosionSound;
        public AudioClip ghostDefeatSound;
        
        [Header("Timing")]
        [Tooltip("Time before mask explodes after spawning")]
        public float timeBeforeExplosion = 2f;
        
        [Tooltip("Time explosion effect plays before ghost defeat")]
        public float explosionDuration = 1f;
        
        private AudioSource audioSource;
        private GameObject spawnedMask;
        private bool maskAwarded = false;
        
        // Tracks every ghost we spawned, keyed by puzzle index
        private Dictionary<int, GameObject> spawnedGhosts = new Dictionary<int, GameObject>();
        
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && (maskSpawnSound != null || ghostDefeatSound != null))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        /// <summary>
        /// Called once at game start. Spawns all ghosts at their spawn points.
        /// Because these are Instantiated copies, we can safely Destroy them later.
        /// </summary>
        public void SpawnAllGhosts(List<PuzzleConfiguration> configs)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];
                
                if (config.ghostPrefab == null)
                {
                    Debug.LogWarning($"Puzzle {i}: ghostPrefab is null. Skipping ghost spawn.");
                    continue;
                }
                
                if (i >= ghostSpawnPoints.Count || ghostSpawnPoints[i] == null)
                {
                    Debug.LogWarning($"Puzzle {i}: no spawn point at index {i} in ghostSpawnPoints list on MaskRewardSystem. Skipping.");
                    continue;
                }
                
                GameObject spawnPoint = ghostSpawnPoints[i];
                
                GameObject ghost = Instantiate(
                    config.ghostPrefab,
                    spawnPoint.transform.position,
                    spawnPoint.transform.rotation
                );
                
                spawnedGhosts[i] = ghost;
                Debug.Log($"Spawned ghost for puzzle {i} at {spawnPoint.transform.position}");
            }
        }
        
        /// <summary>
        /// Award the mask to the player
        /// </summary>
        public void AwardMask(int puzzleIndex)
        {
            if (maskAwarded)
            {
                Debug.LogWarning("Mask already awarded!");
                return;
            }
            
            if (currentConfig == null || currentConfig.maskPrefab == null)
            {
                Debug.LogError("No mask configuration or prefab set!");
                return;
            }
            
            maskAwarded = true;
            
            // Auto-find player if not assigned
            if (playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerTransform = player.transform;
                }
            }
            
            // Calculate spawn position above and in front of player
            Vector3 spawnPos;
            if (playerTransform != null)
            {
                spawnPos = playerTransform.position + 
                          Vector3.up * spawnHeightAbovePlayer + 
                          playerTransform.forward * spawnForwardOffset;
            }
            else
            {
                spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 3f;
                Debug.LogWarning("Player not found! Spawning mask in front of camera.");
            }
            
            // Spawn the mask
            spawnedMask = Instantiate(currentConfig.maskPrefab, spawnPos, Quaternion.identity);
            Debug.Log($"Mask spawned at {spawnPos}");
            
            // Play spawn effect
            if (spawnEffect != null)
            {
                spawnEffect.transform.position = spawnPos;
                spawnEffect.Play();
            }
            
            // Play spawn sound
            if (maskSpawnSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(maskSpawnSound);
            }
            
            // Trigger explosion sequence
            StartCoroutine(MaskExplosionSequence(spawnPos, puzzleIndex));
        }
        
        /// <summary>
        /// Handle mask explosion then ghost defeat
        /// </summary>
        private IEnumerator MaskExplosionSequence(Vector3 maskPosition, int puzzleIndex)
        {
            yield return new WaitForSeconds(timeBeforeExplosion);
            
            Debug.Log("Mask exploding!");
            
            if (explosionEffect != null)
            {
                explosionEffect.transform.position = maskPosition;
                explosionEffect.Play();
            }
            
            if (explosionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(explosionSound);
            }
            
            // Scale mask down and destroy it (safe — it's an Instantiate copy)
            if (spawnedMask != null)
            {
                StartCoroutine(ScaleDownAndDestroy(spawnedMask, 0.3f));
            }
            
            yield return new WaitForSeconds(explosionDuration);
            
            // Defeat the ghost for this puzzle
            StartCoroutine(DefeatGhostSequence(puzzleIndex));
        }
        
        /// <summary>
        /// Scale down and destroy. Safe to call on Instantiated objects.
        /// </summary>
        private IEnumerator ScaleDownAndDestroy(GameObject obj, float duration)
        {
            Vector3 originalScale = obj.transform.localScale;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                obj.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
                yield return null;
            }
            
            Destroy(obj);
        }
        
        /// <summary>
        /// Ghost defeat: floats upward while scaling to zero, then gets destroyed.
        /// No materials are touched at all.
        /// </summary>
        private IEnumerator DefeatGhostSequence(int puzzleIndex)
        {
            if (!spawnedGhosts.ContainsKey(puzzleIndex))
            {
                Debug.LogWarning($"No spawned ghost found for puzzle index {puzzleIndex}!");
                if (ghostCounter != null) ghostCounter.OnGhostDefeated();
                yield break;
            }
            
            GameObject ghost = spawnedGhosts[puzzleIndex];
            
            if (ghost == null)
            {
                Debug.LogWarning($"Ghost for puzzle {puzzleIndex} is null!");
                if (ghostCounter != null) ghostCounter.OnGhostDefeated();
                yield break;
            }
            
            Debug.Log($"Ghost defeat sequence for puzzle {puzzleIndex}: {ghost.name}");
            
            if (ghostDefeatSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ghostDefeatSound);
            }
            
            // Ghost floats UP and scales to zero simultaneously
            Vector3 startPos = ghost.transform.position;
            Vector3 endPos = startPos + Vector3.up * ghostFloatHeight;
            Vector3 startScale = ghost.transform.localScale;
            float duration = currentConfig.ghostDefeatDuration;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                
                // Float upward
                ghost.transform.position = Vector3.Lerp(startPos, endPos, t);
                // Scale to zero
                ghost.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                
                yield return null;
            }
            
            // Safe to destroy — this is an Instantiated copy, not a prefab asset
            Destroy(ghost);
            spawnedGhosts.Remove(puzzleIndex);
            Debug.Log($"Ghost {puzzleIndex} defeated and destroyed!");
            
            if (ghostCounter != null)
            {
                ghostCounter.OnGhostDefeated();
            }
        }
        
        /// <summary>
        /// Update the configuration at runtime
        /// </summary>
        public void SetConfiguration(PuzzleConfiguration config)
        {
            currentConfig = config;
            maskAwarded = false;
        }
    }
}