using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI interactions for PC mode (quiz, buttons, etc.)
/// Makes VR UI elements clickable with mouse
/// </summary>
public class PCUIController : MonoBehaviour
{
    [Header("Settings")]
    public float uiInteractionRange = 5.0f;
    public LayerMask uiLayer;

    [Header("Cursor")]
    public Texture2D cursorTexture;
    public Vector2 cursorHotspot = Vector2.zero;

    private Camera playerCamera;
    private GameObject currentUITarget;

    private void Start()
    {
        playerCamera = Camera.main;

        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }

    private void Update()
    {
        if (!enabled) return;

        DetectUIElements();
        HandleUIInteraction();
    }

    private void DetectUIElements()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, uiInteractionRange, uiLayer))
        {
            currentUITarget = hit.collider.gameObject;
        }
        else
        {
            currentUITarget = null;
        }
    }

    private void HandleUIInteraction()
    {
        if (Input.GetMouseButtonDown(0) && currentUITarget != null)
        {
            // Check for Button component
            Button button = currentUITarget.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
                Debug.Log($"Clicked UI Button: {currentUITarget.name}");
                return;
            }

            // Check for AnswerScript (quiz answers)
            AnswerScript answerScript = currentUITarget.GetComponent<AnswerScript>();
            if (answerScript != null)
            {
                answerScript.Answer();
                Debug.Log($"Selected Answer: {currentUITarget.name}");
                return;
            }

            // Generic UI interaction
            currentUITarget.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
