using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles PC mouse-based interaction with objects (grab, use, interact)
/// Replaces VR controller grabbing with raycast-based interaction
/// </summary>
public class PCInteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3.0f;
    public LayerMask interactableLayer;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode grabKey = KeyCode.Mouse0; // Left mouse button
    public KeyCode useKey = KeyCode.Mouse1; // Right mouse button

    [Header("Visual Feedback")]
    public GameObject crosshairUI;
    public Text interactionPromptText;
    public Color highlightColor = Color.yellow;
    public Color canGrabColor = Color.green;
    public float highlightIntensity = 1.5f;
    
    [Header("Audio Feedback")]
    public AudioClip hoverSound;
    public AudioClip pickupSound;
    public AudioClip dropSound;
    private AudioSource audioSource;

    [Header("Held Object Settings")]
    public Transform holdPosition;
    public float holdDistance = 1.5f;
    public float throwForce = 10f;

    private Camera playerCamera;
    private GameObject currentLookTarget;
    private GameObject heldObject;
    private Rigidbody heldObjectRb;
    private Renderer currentHighlight;
    private Color originalColor;
    private bool hasOriginalColor;

    // For tools like spade and watering can
    private PCToolController currentTool;
    private bool isUsingTool;

    private void Start()
    {
        playerCamera = Camera.main;

        if (holdPosition == null)
        {
            // Create hold position in front of camera
            GameObject holdObj = new GameObject("HoldPosition");
            holdObj.transform.SetParent(playerCamera.transform);
            holdObj.transform.localPosition = new Vector3(0.3f, -0.3f, holdDistance);
            holdPosition = holdObj.transform;
        }

        if (interactionPromptText != null)
        {
            interactionPromptText.text = "";
            interactionPromptText.fontSize = 24;
        }

        // Setup audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // 2D sound
    }

    private void Update()
    {
        if (!enabled) return;

        PerformRaycast();
        HandleInteraction();
        UpdateHeldObject();
    }

    private void PerformRaycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Clear previous highlight
        ClearHighlight();

        // DEBUG: Try raycast WITHOUT layer mask first
        bool hitSomething = false;
        if (interactableLayer.value == 0)
        {
            // If layer mask is "Nothing", raycast everything
            hitSomething = Physics.Raycast(ray, out hit, interactionRange);
            if (hitSomething)
            {
                Debug.Log($"[PC] Hit object: {hit.collider.gameObject.name} (Tag: {hit.collider.gameObject.tag}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)})");
            }
        }
        else
        {
            hitSomething = Physics.Raycast(ray, out hit, interactionRange, interactableLayer);
            if (hitSomething)
            {
                Debug.Log($"[PC] Hit object: {hit.collider.gameObject.name} with layer mask");
            }
        }

        if (hitSomething)
        {
            GameObject hitObject = hit.collider.gameObject;
            currentLookTarget = hitObject;

            // Highlight the object
            HighlightObject(hitObject);

            // Show interaction prompt
            ShowInteractionPrompt(hitObject);
        }
        else
        {
            currentLookTarget = null;
            if (interactionPromptText != null)
            {
                interactionPromptText.text = "";
            }
        }
    }

    private void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            currentHighlight = renderer;
            if (!hasOriginalColor && renderer.material.HasProperty("_Color"))
            {
                originalColor = renderer.material.color;
                hasOriginalColor = true;
            }
            
            if (renderer.material.HasProperty("_Color"))
            {
                // Use green for grabbable items, yellow for soil
                bool isGrabbable = obj.CompareTag("Spade") || obj.CompareTag("WateringCan") || obj.CompareTag("Seed");
                Color targetColor = isGrabbable ? canGrabColor : highlightColor;
                
                // DEBUG: Log what we're highlighting
                if (obj.CompareTag("Seed") || obj.name.Contains("Seed"))
                {
                    Debug.Log($"Highlighting SEED: {obj.name}, Tag: {obj.tag}, IsGrabbable: {isGrabbable}, Color: {(isGrabbable ? "GREEN" : "YELLOW")}");
                }
                
                renderer.material.color = targetColor * highlightIntensity;
            }

            // Play hover sound once
            if (hoverSound != null && audioSource != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(hoverSound, 0.3f);
            }
        }
    }

    private void ClearHighlight()
    {
        if (currentHighlight != null && hasOriginalColor)
        {
            if (currentHighlight.material.HasProperty("_Color"))
            {
                currentHighlight.material.color = originalColor;
            }
            currentHighlight = null;
            hasOriginalColor = false;
        }
    }

    private void ShowInteractionPrompt(GameObject obj)
    {
        if (interactionPromptText == null) return;

        string promptText = "";
        string itemName = GetFriendlyName(obj);

        if (heldObject == null)
        {
            // Not holding anything
            if (obj.CompareTag("Spade"))
            {
                promptText = $"🔧 [LEFT CLICK] Pick up SPADE\n(Used to dig soil and cover seeds)";
            }
            else if (obj.CompareTag("WateringCan"))
            {
                promptText = $"💧 [LEFT CLICK] Pick up WATERING CAN\n(Used to water plants)";
            }
            else if (obj.CompareTag("Seed"))
            {
                promptText = $"🌱 [LEFT CLICK] Pick up SEEDS\n(Place in dug hole to plant)";
            }
            else if (obj.CompareTag("Harvestable"))
            {
                promptText = $"🍅 [LEFT CLICK] Harvest {itemName}";
            }
            else if (obj.CompareTag("Soil"))
            {
                promptText = "⚠️ Pick up a SPADE first to dig here";
            }
        }
        else
        {
            // Already holding something
            if (obj.CompareTag("Soil"))
            {
                if (currentTool != null && currentTool.gameObject.CompareTag("Spade"))
                {
                    promptText = $"⛏️ [HOLD RIGHT CLICK] DIG with Spade\n[LEFT CLICK] Drop Spade";
                }
                else if (currentTool != null && currentTool.gameObject.CompareTag("WateringCan"))
                {
                    promptText = $"💧 [HOLD RIGHT CLICK] POUR Water\n[LEFT CLICK] Drop Watering Can";
                }
                else
                {
                    promptText = $"[LEFT CLICK] Drop {GetFriendlyName(heldObject)}";
                }
            }
            else
            {
                promptText = $"Holding: {GetFriendlyName(heldObject)}\n[LEFT CLICK] Drop here";
            }
        }

        interactionPromptText.text = promptText;
    }

    private string GetFriendlyName(GameObject obj)
    {
        if (obj == null) return "Item";
        
        if (obj.CompareTag("Spade")) return "Spade";
        if (obj.CompareTag("WateringCan")) return "Watering Can";
        if (obj.CompareTag("Seed")) return "Seeds";
        if (obj.CompareTag("Soil")) return "Soil";
        if (obj.CompareTag("Harvestable")) return "Vegetable";
        
        return obj.name;
    }

    private void HandleInteraction()
    {
        // Grab/Drop object
        if (Input.GetKeyDown(grabKey))
        {
            if (heldObject == null && currentLookTarget != null)
            {
                TryGrabObject(currentLookTarget);
            }
            else if (heldObject != null)
            {
                DropObject();
            }
        }

        // Use tool or interact
        if (Input.GetKey(useKey))
        {
            if (currentTool != null)
            {
                currentTool.UseTool(true);
                isUsingTool = true;
            }
        }
        else if (isUsingTool)
        {
            if (currentTool != null)
            {
                currentTool.UseTool(false);
            }
            isUsingTool = false;
        }

        // Generic interaction
        if (Input.GetKeyDown(interactKey) && currentLookTarget != null)
        {
            TriggerInteraction(currentLookTarget);
        }
    }

    private void TryGrabObject(GameObject obj)
    {
        // Check if object is grabbable
        if (obj.CompareTag("Spade") || obj.CompareTag("WateringCan") || obj.CompareTag("Seed") || 
            obj.CompareTag("Harvestable") || obj.GetComponent<Rigidbody>() != null)
        {
            GrabObject(obj);
        }
    }

    private void GrabObject(GameObject obj)
    {
        heldObject = obj;
        heldObjectRb = obj.GetComponent<Rigidbody>();

        // Play pickup sound
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound, 0.5f);
        }

        // Disable physics while holding
        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = true;
            heldObjectRb.useGravity = false;
        }

        // DON'T disable collider - we need it for tools like spade to detect soil!
        // The spade needs its collider to trigger SpadeCollider.OnTriggerEnter on soil
        /*
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        */

        // Check if it's a tool
        currentTool = obj.GetComponent<PCToolController>();
        if (currentTool == null)
        {
            currentTool = obj.AddComponent<PCToolController>();
            currentTool.Initialize(obj);
        }

        // Trigger grab events (for compatibility with HarvestScript)
        var harvestScript = obj.GetComponent<HarvestScript>();
        if (harvestScript != null)
        {
            // The HarvestScript expects AutoHand events, we'll simulate it
            SendMessage("OnGrab", SendMessageOptions.DontRequireReceiver);
        }

        Debug.Log($"✓ Picked up: {GetFriendlyName(obj)}");
        
        // Update prompt to show what you're holding
        if (interactionPromptText != null)
        {
            interactionPromptText.text = $"Holding: {GetFriendlyName(obj)}\nLook at soil to use or click to drop";
        }
    }

    private void DropObject()
    {
        if (heldObject == null) return;

        string droppedName = GetFriendlyName(heldObject);

        // Play drop sound
        if (dropSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dropSound, 0.4f);
        }

        // Re-enable physics
        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = false;
            heldObjectRb.useGravity = true;

            // Drop in front of player, not throw too far
            Vector3 dropDirection = playerCamera.transform.forward;
            Vector3 dropPosition = playerCamera.transform.position + dropDirection * 1.5f;
            
            // Make sure it drops at a reasonable height (not below ground)
            dropPosition.y = Mathf.Max(dropPosition.y, 0.5f);
            heldObject.transform.position = dropPosition;
            
            // Add gentle forward force
            heldObjectRb.velocity = Vector3.zero; // Clear any existing velocity
            heldObjectRb.AddForce(dropDirection * (throwForce * 0.5f), ForceMode.VelocityChange);
        }

        // Collider is already enabled (we never disabled it now)
        // But make sure it's definitely on
        Collider collider = heldObject.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        // Trigger release events
        var harvestScript = heldObject.GetComponent<HarvestScript>();
        if (harvestScript != null)
        {
            SendMessage("OnRelease", SendMessageOptions.DontRequireReceiver);
        }

        Debug.Log($"✓ Dropped: {droppedName} at position {heldObject.transform.position}");

        heldObject = null;
        heldObjectRb = null;
        currentTool = null;

        // Clear prompt
        if (interactionPromptText != null)
        {
            interactionPromptText.text = "";
        }
    }

    private void UpdateHeldObject()
    {
        if (heldObject != null && holdPosition != null)
        {
            // Smoothly move held object to hold position
            heldObject.transform.position = Vector3.Lerp(
                heldObject.transform.position,
                holdPosition.position,
                Time.deltaTime * 10f
            );

            // Optionally rotate to face forward
            heldObject.transform.rotation = Quaternion.Lerp(
                heldObject.transform.rotation,
                holdPosition.rotation,
                Time.deltaTime * 5f
            );
        }
    }

    private void TriggerInteraction(GameObject obj)
    {
        // Send interaction message to the object
        obj.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);

        // Check for specific components
        var button = obj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke();
        }

        Debug.Log($"Interacted with: {obj.name}");
    }

    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    public bool IsHoldingObject()
    {
        return heldObject != null;
    }

    public GameObject GetCurrentLookTarget()
    {
        return currentLookTarget;
    }

    private void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionRange);
        }
    }
}
