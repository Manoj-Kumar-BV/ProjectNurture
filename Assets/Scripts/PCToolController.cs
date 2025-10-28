using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles tool-specific behavior for PC controls (spade digging, watering can pouring)
/// Attached automatically to tools when picked up in PC mode
/// </summary>
public class PCToolController : MonoBehaviour
{
    private GameObject toolObject;
    private string toolType;
    private bool isInitialized = false;

    // For watering can
    private PourDetector pourDetector;
    private float currentPourAngle = 0f;
    private bool isPouring = false;

    // For spade
    private bool isDigging = false;

    public void Initialize(GameObject tool)
    {
        toolObject = tool;
        DetermineToolType();
        isInitialized = true;
    }

    private void DetermineToolType()
    {
        if (toolObject == null) return;

        if (toolObject.CompareTag("WateringCan"))
        {
            toolType = "WateringCan";
            pourDetector = toolObject.GetComponent<PourDetector>();
        }
        else if (toolObject.CompareTag("Spade"))
        {
            toolType = "Spade";
        }
        else if (toolObject.CompareTag("Seed"))
        {
            toolType = "Seed";
        }
    }

    private void Update()
    {
        if (!isInitialized) return;

        // Handle tool-specific updates
        if (toolType == "WateringCan")
        {
            UpdateWateringCan();
        }
    }

    public void UseTool(bool active)
    {
        if (!isInitialized) return;

        switch (toolType)
        {
            case "WateringCan":
                HandleWateringCan(active);
                break;
            case "Spade":
                HandleSpade(active);
                break;
        }
    }

    private void HandleWateringCan(bool shouldPour)
    {
        if (pourDetector == null) return;

        if (shouldPour && !isPouring)
        {
            // Tilt the watering can to pour
            StartCoroutine(TiltWateringCan(true));
            isPouring = true;
        }
        else if (!shouldPour && isPouring)
        {
            // Return to normal position
            StartCoroutine(TiltWateringCan(false));
            isPouring = false;
        }
    }

    private IEnumerator TiltWateringCan(bool pour)
    {
        float targetAngle = pour ? 90f : 0f; // 90 degrees to pour, 0 to stop
        float duration = 0.3f;
        float elapsed = 0f;
        float startAngle = currentPourAngle;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            currentPourAngle = Mathf.Lerp(startAngle, targetAngle, t);
            
            // Apply rotation to the watering can
            if (toolObject != null)
            {
                // Rotate around the right axis to simulate tilting forward
                Quaternion tiltRotation = Quaternion.Euler(currentPourAngle, 0, 0);
                toolObject.transform.localRotation = tiltRotation;
            }

            yield return null;
        }

        currentPourAngle = targetAngle;
    }

    private void UpdateWateringCan()
    {
        // The PourDetector will handle the actual pouring based on angle
        // We just need to maintain the tilt
    }

    private void HandleSpade(bool shouldDig)
    {
        if (toolObject == null) return;
        
        if (shouldDig && !isDigging)
        {
            // Perform digging animation/action
            StartCoroutine(PerformDigAction());
            isDigging = true;
        }
    }

    private IEnumerator PerformDigAction()
    {
        // Dig animation - move spade forward and down into soil
        if (toolObject == null) yield break;

        Transform cameraTransform = Camera.main.transform;
        Vector3 originalPos = toolObject.transform.position;
        Quaternion originalRot = toolObject.transform.rotation;
        
        // Calculate dig position (forward and down from camera)
        Vector3 digDirection = cameraTransform.forward + Vector3.down * 0.5f;
        Vector3 digPos = originalPos + digDirection.normalized * 0.5f;
        
        float duration = 0.3f;
        
        // Move down into soil
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            toolObject.transform.position = Vector3.Lerp(originalPos, digPos, t);
            yield return null;
        }

        // Hold at bottom to trigger soil collision
        yield return new WaitForSeconds(0.2f);

        // Move back up
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            toolObject.transform.position = Vector3.Lerp(digPos, originalPos, t);
            yield return null;
        }

        // Reset to exact original position
        toolObject.transform.position = originalPos;
        toolObject.transform.rotation = originalRot;
        
        isDigging = false;
    }

    private void OnDisable()
    {
        // Reset tool state when dropped
        isPouring = false;
        isDigging = false;
        
        if (toolObject != null)
        {
            toolObject.transform.localRotation = Quaternion.identity;
        }
    }
}
