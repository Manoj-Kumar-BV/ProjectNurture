using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Helper script to automatically setup PC controls in the scene
/// Attach to an empty GameObject and click "Auto Setup PC Controls" in the inspector
/// </summary>
public class PCSetupHelper : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Auto Setup Configuration")]
    [Tooltip("The VR camera rig in your scene (e.g., OVRCameraRig)")]
    public GameObject vrCameraRig;
    
    [Tooltip("VR controller objects (left and right hand)")]
    public GameObject[] vrControllers;

    [Tooltip("Spawn position for PC player")]
    public Vector3 pcSpawnPosition = new Vector3(0, 1, 0);

    [Header("Layer Settings")]
    [Tooltip("Name of the interactable layer to create/use")]
    public string interactableLayerName = "Interactable";

    [Header("Tag Settings")]
    [Tooltip("Tags to create if they don't exist")]
    public string[] tagsToCreate = new string[] { "Spade", "WateringCan", "Seed", "Soil", "Harvestable" };

    [Header("References (Auto-populated)")]
    public GameObject pcPlayer;
    public GameObject inputManager;

    [ContextMenu("Auto Setup PC Controls")]
    public void AutoSetupPCControls()
    {
        Debug.Log("=== Starting PC Controls Auto Setup ===");

        CreateLayers();
        CreateTags();
        CreatePCPlayer();
        CreateInputManager();
        CreateUIElements();

        Debug.Log("=== PC Controls Auto Setup Complete! ===");
        Debug.Log("Next steps:");
        Debug.Log("1. Assign interactable objects to 'Interactable' layer");
        Debug.Log("2. Tag your tools and objects appropriately");
        Debug.Log("3. Test in Play mode (make sure no VR headset is connected)");
        
        EditorUtility.DisplayDialog("Setup Complete", 
            "PC Controls have been set up!\n\n" +
            "Please check the console for next steps.", "OK");
    }

    private void CreateLayers()
    {
        Debug.Log("Creating layers...");
        
        // Note: Can't create layers programmatically easily in Unity
        // User must do this manually
        Debug.LogWarning($"Please manually create layer '{interactableLayerName}' in Edit > Project Settings > Tags and Layers");
    }

    private void CreateTags()
    {
        Debug.Log("Creating tags...");

        foreach (string tag in tagsToCreate)
        {
            // Check if tag exists
            try
            {
                GameObject.FindGameObjectWithTag(tag);
                Debug.Log($"Tag '{tag}' already exists.");
            }
            catch
            {
                // Tag doesn't exist, need to create it
                Debug.LogWarning($"Please manually create tag '{tag}' in Edit > Project Settings > Tags and Layers");
            }
        }
    }

    private void CreatePCPlayer()
    {
        Debug.Log("Creating PC Player...");

        // Check if PC Player already exists
        pcPlayer = GameObject.Find("PCPlayer");
        if (pcPlayer != null)
        {
            Debug.Log("PCPlayer already exists. Skipping creation.");
            return;
        }

        // Create PC Player
        pcPlayer = new GameObject("PCPlayer");
        pcPlayer.transform.position = pcSpawnPosition;

        // Add Character Controller
        CharacterController cc = pcPlayer.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.radius = 0.3f;
        cc.center = new Vector3(0, 0.9f, 0);

        // Add PC Player Controller
        PCPlayerController playerController = pcPlayer.AddComponent<PCPlayerController>();
        
        // Add PC Interaction Controller
        PCInteractionController interactionController = pcPlayer.AddComponent<PCInteractionController>();
        interactionController.interactionRange = 3.0f;
        interactionController.holdDistance = 1.5f;

        // Add PC UI Controller
        pcPlayer.AddComponent<PCUIController>();

        // Create PC Camera
        GameObject pcCamera = new GameObject("PCCamera");
        pcCamera.transform.SetParent(pcPlayer.transform);
        pcCamera.transform.localPosition = new Vector3(0, 1.6f, 0);
        
        Camera cam = pcCamera.AddComponent<Camera>();
        cam.tag = "MainCamera";

        // Assign camera to player controller
        playerController.cameraTransform = pcCamera.transform;

        // Create Ground Check
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(pcPlayer.transform);
        groundCheck.transform.localPosition = new Vector3(0, -0.9f, 0);
        
        playerController.groundCheck = groundCheck.transform;

        // Create Hold Position
        GameObject holdPosition = new GameObject("HoldPosition");
        holdPosition.transform.SetParent(pcCamera.transform);
        holdPosition.transform.localPosition = new Vector3(0.3f, -0.3f, 1.5f);
        
        interactionController.holdPosition = holdPosition.transform;

        Debug.Log("PC Player created successfully!");
    }

    private void CreateInputManager()
    {
        Debug.Log("Creating Input Manager...");

        // Check if Input Manager already exists
        inputManager = GameObject.Find("InputManager");
        if (inputManager != null)
        {
            Debug.Log("InputManager already exists. Updating configuration.");
        }
        else
        {
            inputManager = new GameObject("InputManager");
            Debug.Log("InputManager created.");
        }

        // Add or get PCInputManager component
        PCInputManager inputMgr = inputManager.GetComponent<PCInputManager>();
        if (inputMgr == null)
        {
            inputMgr = inputManager.AddComponent<PCInputManager>();
        }

        // Configure
        inputMgr.autoDetect = true;
        inputMgr.currentInputMode = PCInputManager.InputMode.PC;
        inputMgr.pcCamera = pcPlayer;
        inputMgr.vrCameraRig = vrCameraRig;
        inputMgr.vrControllers = vrControllers;

        if (pcPlayer != null)
        {
            inputMgr.pcPlayerController = pcPlayer.GetComponent<PCPlayerController>();
            inputMgr.pcInteractionController = pcPlayer.GetComponent<PCInteractionController>();
        }

        Debug.Log("Input Manager configured!");
    }

    private void CreateUIElements()
    {
        Debug.Log("Setting up UI elements...");

        // Find or create Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No Canvas found in scene. Please create UI manually.");
            return;
        }

        // Create Crosshair
        GameObject crosshair = new GameObject("Crosshair");
        crosshair.transform.SetParent(canvas.transform);
        
        RectTransform crosshairRect = crosshair.AddComponent<RectTransform>();
        crosshairRect.anchoredPosition = Vector2.zero;
        crosshairRect.sizeDelta = new Vector2(20, 20);

        UnityEngine.UI.Image crosshairImage = crosshair.AddComponent<UnityEngine.UI.Image>();
        crosshairImage.color = Color.white;

        // Create Interaction Prompt
        GameObject prompt = new GameObject("InteractionPrompt");
        prompt.transform.SetParent(canvas.transform);
        
        RectTransform promptRect = prompt.AddComponent<RectTransform>();
        promptRect.anchoredPosition = new Vector2(0, -200);
        promptRect.sizeDelta = new Vector2(400, 50);

        UnityEngine.UI.Text promptText = prompt.AddComponent<UnityEngine.UI.Text>();
        promptText.text = "";
        promptText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        promptText.fontSize = 18;
        promptText.color = Color.white;
        promptText.alignment = TextAnchor.MiddleCenter;

        // Assign to PC Interaction Controller
        if (pcPlayer != null)
        {
            PCInteractionController interactionController = pcPlayer.GetComponent<PCInteractionController>();
            if (interactionController != null)
            {
                interactionController.crosshairUI = crosshair;
                interactionController.interactionPromptText = promptText;
            }
        }

        Debug.Log("UI elements created!");
    }

    [ContextMenu("Find VR Components")]
    public void FindVRComponents()
    {
        // Try to find VR camera rig
        if (vrCameraRig == null)
        {
            GameObject rig = GameObject.Find("OVRCameraRig");
            if (rig == null) rig = GameObject.Find("VRCameraRig");
            if (rig == null) rig = GameObject.Find("CameraRig");
            
            if (rig != null)
            {
                vrCameraRig = rig;
                Debug.Log($"Found VR Camera Rig: {rig.name}");
            }
        }

        // Try to find VR controllers
        GameObject leftHand = GameObject.Find("LeftHandAnchor");
        GameObject rightHand = GameObject.Find("RightHandAnchor");
        
        if (leftHand != null && rightHand != null)
        {
            vrControllers = new GameObject[] { leftHand, rightHand };
            Debug.Log("Found VR Controllers");
        }
    }
#endif
}
