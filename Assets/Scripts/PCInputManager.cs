using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages input detection and switches between VR and PC control modes
/// </summary>
public class PCInputManager : MonoBehaviour
{
    public static PCInputManager Instance { get; private set; }

    public enum InputMode
    {
        VR,
        PC
    }

    [Header("Input Mode")]
    public InputMode currentInputMode = InputMode.PC;
    public bool autoDetect = true;

    [Header("PC Components")]
    public GameObject pcCamera;
    public PCPlayerController pcPlayerController;
    public PCInteractionController pcInteractionController;

    [Header("VR Components")]
    public GameObject vrCameraRig;
    public GameObject[] vrControllers;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (autoDetect)
        {
            DetectInputMode();
        }
        else
        {
            SetInputMode(currentInputMode);
        }
    }

    private void DetectInputMode()
    {
        // Check if VR headset is connected
        bool vrPresent = UnityEngine.XR.XRSettings.isDeviceActive;

        if (vrPresent)
        {
            SetInputMode(InputMode.VR);
        }
        else
        {
            SetInputMode(InputMode.PC);
        }
    }

    public void SetInputMode(InputMode mode)
    {
        currentInputMode = mode;

        switch (mode)
        {
            case InputMode.VR:
                EnableVRMode();
                break;
            case InputMode.PC:
                EnablePCMode();
                break;
        }
    }

    private void EnableVRMode()
    {
        Debug.Log("Enabling VR Mode");

        // Enable VR components
        if (vrCameraRig != null)
            vrCameraRig.SetActive(true);

        foreach (var controller in vrControllers)
        {
            if (controller != null)
                controller.SetActive(true);
        }

        // Disable PC components
        if (pcCamera != null)
            pcCamera.SetActive(false);

        if (pcPlayerController != null)
            pcPlayerController.enabled = false;

        if (pcInteractionController != null)
            pcInteractionController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void EnablePCMode()
    {
        Debug.Log("Enabling PC Mode");

        // Enable PC components
        if (pcCamera != null)
            pcCamera.SetActive(true);

        if (pcPlayerController != null)
            pcPlayerController.enabled = true;

        if (pcInteractionController != null)
            pcInteractionController.enabled = true;

        // Disable VR components
        if (vrCameraRig != null)
            vrCameraRig.SetActive(false);

        foreach (var controller in vrControllers)
        {
            if (controller != null)
                controller.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsVRMode()
    {
        return currentInputMode == InputMode.VR;
    }

    public bool IsPCMode()
    {
        return currentInputMode == InputMode.PC;
    }

    private void Update()
    {
        // Toggle cursor lock with Escape key in PC mode
        if (IsPCMode() && Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
