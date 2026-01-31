using UnityEngine;
using System.Collections;

namespace SlidingPuzzle
{
    /// <summary>
    /// Handles mask spawning and ghost defeat sequence
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
        
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && (maskSpawnSound != null || ghostDefeatSound != null))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        /// <summary>
        /// Award the mask to the player
        /// </summary>
        public void AwardMask()
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
                // Fallback if no player found
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
            
            // Trigger explosion sequence after delay
            StartCoroutine(MaskExplosionSequence(spawnPos));
        }
        
        /// <summary>
        /// Handle mask explosion and ghost defeat
        /// </summary>
        private IEnumerator MaskExplosionSequence(Vector3 maskPosition)
        {
            // Wait before explosion
            yield return new WaitForSeconds(timeBeforeExplosion);
            
            Debug.Log("Mask exploding!");
            
            // Play explosion effect
            if (explosionEffect != null)
            {
                explosionEffect.transform.position = maskPosition;
                explosionEffect.Play();
            }
            
            // Play explosion sound
            if (explosionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(explosionSound);
            }
            
            // Make mask disappear/destroy with explosion
            if (spawnedMask != null)
            {
                // Optional: Add a quick scale-down animation
                StartCoroutine(ScaleDownAndDestroy(spawnedMask, 0.3f));
            }
            
            // Wait for explosion effect to play
            yield return new WaitForSeconds(explosionDuration);
            
            // Now trigger ghost defeat
            StartCoroutine(DefeatGhostSequence());
        }
        
        /// <summary>
        /// Scale down and destroy object
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
        /// Handle the ghost defeat animation and destruction
        /// </summary>
        private IEnumerator DefeatGhostSequence()
        {
            if (currentConfig.ghostObject == null)
            {
                Debug.LogWarning("No ghost object assigned!");
                yield break;
            }
            
            Debug.Log("Ghost defeat sequence starting...");
            
            // Play defeat sound
            if (ghostDefeatSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ghostDefeatSound);
            }
            
            // Trigger defeat animation
            Animator ghostAnimator = currentConfig.ghostObject.GetComponent<Animator>();
            if (ghostAnimator != null && !string.IsNullOrEmpty(currentConfig.defeatAnimationTrigger))
            {
                ghostAnimator.SetTrigger(currentConfig.defeatAnimationTrigger);
                Debug.Log($"Ghost defeat animation triggered: {currentConfig.defeatAnimationTrigger}");
            }
            
            // Wait for animation to play
            yield return new WaitForSeconds(currentConfig.ghostDestroyDelay);
            
            // Destroy or disable the ghost
            if (currentConfig.ghostObject != null)
            {
                Destroy(currentConfig.ghostObject);
                Debug.Log("Ghost defeated and destroyed!");
                
                // Increment ghost counter
                if (ghostCounter != null)
                {
                    ghostCounter.OnGhostDefeated();
                }
            }
        }
        
        /// <summary>
        /// Reset the reward system for a new puzzle
        /// </summary>
        public void Reset()
        {
            if (spawnedMask != null)
            {
                Destroy(spawnedMask);
            }
            
            maskAwarded = false;
        }
        
        /// <summary>
        /// Update the configuration at runtime
        /// </summary>
        public void SetConfiguration(PuzzleConfiguration config)
        {
            currentConfig = config;
        }
    }
}
