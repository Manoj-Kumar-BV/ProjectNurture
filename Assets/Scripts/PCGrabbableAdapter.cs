using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// PC-compatible wrapper for AutoHand Grabbable component
/// Provides the same interface for both VR and PC modes
/// </summary>
public class PCGrabbableAdapter : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onGrab;
    public UnityEvent onRelease;

    [Header("Settings")]
    public bool canBeGrabbed = true;
    public bool destroyOnRelease = false;

    private bool isGrabbed = false;
    private Transform originalParent;

    // Check if we're in PC mode
    private bool IsPCMode()
    {
        return PCInputManager.Instance != null && PCInputManager.Instance.IsPCMode();
    }

    public void TriggerGrab()
    {
        if (!canBeGrabbed || isGrabbed) return;

        isGrabbed = true;
        originalParent = transform.parent;

        onGrab?.Invoke();
        Debug.Log($"PCGrabbableAdapter: Grabbed {gameObject.name}");
    }

    public void TriggerRelease()
    {
        if (!isGrabbed) return;

        isGrabbed = false;

        onRelease?.Invoke();
        Debug.Log($"PCGrabbableAdapter: Released {gameObject.name}");

        if (destroyOnRelease)
        {
            Destroy(gameObject);
        }
    }

    public bool IsGrabbed()
    {
        return isGrabbed;
    }

    // Called from PCInteractionController
    public void OnPCGrab()
    {
        TriggerGrab();
    }

    public void OnPCRelease()
    {
        TriggerRelease();
    }
}
