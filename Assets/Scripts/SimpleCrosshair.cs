using UnityEngine;
using UnityEngine.UI;

public class SimpleCrosshair : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color interactableColor = Color.green;
    public Color soilColor = Color.yellow;
    public float size = 30f;
    
    private PCInteractionController interactionController;
    private Image[] lines;

    void Start()
    {
        CreateCrosshair();
        interactionController = GetComponent<PCInteractionController>();
        if (interactionController == null)
        {
            interactionController = FindObjectOfType<PCInteractionController>();
        }
    }

    void CreateCrosshair()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            canvasObj.AddComponent<CanvasScaler>();
        }

        GameObject container = new GameObject("Crosshair");
        container.transform.SetParent(canvas.transform, false);
        
        lines = new Image[5];
        lines[0] = MakeLine(container, "Top", 0, size/2, 3, size/2);
        lines[1] = MakeLine(container, "Bottom", 0, -size/2, 3, size/2);
        lines[2] = MakeLine(container, "Left", -size/2, 0, size/2, 3);
        lines[3] = MakeLine(container, "Right", size/2, 0, size/2, 3);
        lines[4] = MakeLine(container, "Dot", 0, 0, 6, 6);
        
        Debug.Log("Crosshair created!");
    }
    
    Image MakeLine(GameObject parent, string name, float x, float y, float w, float h)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent.transform, false);
        
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(x, y);
        rt.sizeDelta = new Vector2(w, h);
        
        Image img = obj.AddComponent<Image>();
        img.sprite = null;
        img.color = normalColor;
        
        // Force it to show as a solid color box
        img.raycastTarget = false;
        
        return img;
    }

    void Update()
    {
        if (interactionController == null || lines == null) return;

        GameObject target = interactionController.GetCurrentLookTarget();
        Color color = normalColor;

        if (target != null)
        {
            string tag = target.tag;
            if (tag == "Spade" || tag == "WateringCan" || tag == "Seed")
            {
                color = interactableColor;
            }
            else if (tag == "Soil")
            {
                color = soilColor;
            }
        }

        foreach (Image line in lines)
        {
            if (line != null) line.color = color;
        }
    }
}
