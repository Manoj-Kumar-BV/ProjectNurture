using UnityEngine;

public class CrosshairSimple : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color interactableColor = Color.green;
    public Color soilColor = Color.yellow;
    public int size = 20;
    public int thickness = 2;
    
    private PCInteractionController interactionController;
    private Color currentColor;

    void Start()
    {
        currentColor = normalColor;
        interactionController = GetComponent<PCInteractionController>();
        if (interactionController == null)
        {
            interactionController = FindObjectOfType<PCInteractionController>();
        }
        Debug.Log("CrosshairSimple started!");
    }

    void Update()
    {
        currentColor = normalColor;
        
        if (interactionController != null)
        {
            GameObject target = interactionController.GetCurrentLookTarget();
            if (target != null)
            {
                string tag = target.tag;
                if (tag == "Spade" || tag == "WateringCan" || tag == "Seed")
                {
                    currentColor = interactableColor;
                }
                else if (tag == "Soil")
                {
                    currentColor = soilColor;
                }
            }
        }
    }

    void OnGUI()
    {
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;
        
        // Draw horizontal line
        DrawRect(centerX - size, centerY - thickness/2, size * 2, thickness, currentColor);
        
        // Draw vertical line
        DrawRect(centerX - thickness/2, centerY - size, thickness, size * 2, currentColor);
        
        // Draw center dot
        DrawRect(centerX - 3, centerY - 3, 6, 6, currentColor);
    }
    
    void DrawRect(float x, float y, float width, float height, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.DrawTexture(new Rect(x, y, width, height), texture);
    }
}
