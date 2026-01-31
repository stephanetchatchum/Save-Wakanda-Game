using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class NPCController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float rotationSpeed = 120f;
    public float gravity = -9.8f;
    
    [Header("Wandering Behavior")]
    public float minWalkTime = 2f;
    public float maxWalkTime = 5f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 4f;
    
    [Header("Detection")]
    public float detectionRadius = 2f;
    public LayerMask npcLayer;
    
    [Header("Talking Behavior")]
    public float talkDuration = 3f;
    
    [Header("Animation")]
    public Animator animator;
    
    // Animation parameter names
    private readonly string ANIM_IS_WALKING = "IsWalking";
    private readonly string ANIM_IS_TALKING = "IsTalking";
    
    private CharacterController controller;
    private Vector3 velocity;
    
    // States
    private enum NPCState { Idle, Walking, Talking }
    private NPCState currentState = NPCState.Idle;
    
    // Movement
    private Vector3 moveDirection;
    private float stateTimer;
    
    // Talking
    private bool isTalking = false;
    private NPCController talkingPartner;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Auto-find animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }
        
        // Start with random state
        StartCoroutine(NPCBehaviorLoop());
    }
    
    void Update()
    {
        HandleMovement();
        CheckForNearbyNPCs();
        UpdateAnimations();
    }
    
    IEnumerator NPCBehaviorLoop()
    {
        while (true)
        {
            // Wait for current state to finish
            yield return new WaitForSeconds(stateTimer);
            
            // Don't interrupt talking
            if (currentState == NPCState.Talking)
            {
                continue;
            }
            
            // Randomly choose next state
            float random = Random.value;
            
            if (random < 0.6f) // 60% chance to walk
            {
                StartWalking();
            }
            else // 40% chance to idle
            {
                StartIdle();
            }
        }
    }
    
    void StartWalking()
    {
        currentState = NPCState.Walking;
        stateTimer = Random.Range(minWalkTime, maxWalkTime);
        
        // Pick random direction
        float randomAngle = Random.Range(0f, 360f);
        moveDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;
        
        // Rotate to face direction
        StartCoroutine(RotateToDirection(moveDirection));
    }
    
    void StartIdle()
    {
        currentState = NPCState.Idle;
        stateTimer = Random.Range(minIdleTime, maxIdleTime);
        moveDirection = Vector3.zero;
    }
    
    void StartTalking(NPCController partner)
    {
        if (isTalking) return; // Already talking
        
        currentState = NPCState.Talking;
        isTalking = true;
        talkingPartner = partner;
        moveDirection = Vector3.zero;
        
        // Face the talking partner
        if (partner != null)
        {
            Vector3 directionToPartner = (partner.transform.position - transform.position).normalized;
            directionToPartner.y = 0; // Keep on same Y level
            StartCoroutine(RotateToDirection(directionToPartner));
        }
        
        // Stop talking after duration
        StartCoroutine(StopTalkingAfterDelay());
    }
    
    IEnumerator StopTalkingAfterDelay()
    {
        yield return new WaitForSeconds(talkDuration);
        StopTalking();
    }
    
    void StopTalking()
    {
        isTalking = false;
        talkingPartner = null;
        
        // Return to idle after talking
        StartIdle();
    }
    
    void HandleMovement()
    {
        // Only move when walking
        if (currentState == NPCState.Walking && !isTalking)
        {
            controller.Move(moveDirection * walkSpeed * Time.deltaTime);
        }
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // Reset falling velocity when grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    
    IEnumerator RotateToDirection(Vector3 direction)
    {
        if (direction == Vector3.zero) yield break;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationProgress = 0f;
        Quaternion startRotation = transform.rotation;
        
        while (rotationProgress < 1f)
        {
            rotationProgress += Time.deltaTime * (rotationSpeed / 90f);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationProgress);
            yield return null;
        }
        
        transform.rotation = targetRotation;
    }
    
    void CheckForNearbyNPCs()
    {
        // Don't check if already talking
        if (isTalking) return;
        
        // Find nearby NPCs
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, detectionRadius, npcLayer);
        
        foreach (Collider col in nearbyColliders)
        {
            // Skip self
            if (col.gameObject == gameObject) continue;
            
            // Check if it's an NPC
            if (col.CompareTag("NPC"))
            {
                NPCController otherNPC = col.GetComponent<NPCController>();
                
                if (otherNPC != null && !otherNPC.isTalking)
                {
                    // Start conversation
                    StartTalking(otherNPC);
                    otherNPC.StartTalking(this);
                    break; // Only talk to one NPC at a time
                }
            }
        }
    }
    
    void UpdateAnimations()
    {
        if (animator == null) return;
        
        // Update walking animation
        bool isWalking = currentState == NPCState.Walking && !isTalking;
        animator.SetBool(ANIM_IS_WALKING, isWalking);
        
        // Update talking animation
        animator.SetBool(ANIM_IS_TALKING, isTalking);
    }
    
    // Collision detection as backup
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // If hit another NPC
        if (hit.gameObject.CompareTag("NPC") && !isTalking)
        {
            NPCController otherNPC = hit.gameObject.GetComponent<NPCController>();
            
            if (otherNPC != null && !otherNPC.isTalking)
            {
                StartTalking(otherNPC);
                otherNPC.StartTalking(this);
            }
        }
        else if (currentState == NPCState.Walking)
        {
            // Hit an obstacle, change direction
            Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-90f, 90f), 0) * moveDirection;
            moveDirection = randomDirection.normalized;
            StartCoroutine(RotateToDirection(moveDirection));
        }
    }
    
    // Visualize detection radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
