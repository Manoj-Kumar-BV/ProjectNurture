using UnityEngine;
using UnityEditor;

/// <summary>
/// Automatically tags all objects for PC interaction system
/// Run once: Tools → Setup PC Tags
/// </summary>
public class AutoTagObjects : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Setup PC Tags")]
    public static void SetupAllTags()
    {
        int taggedCount = 0;
        
        Debug.Log("🎯 Starting Auto-Tag Setup...");
        
        // Create tags if they don't exist
        CreateTag("Spade");
        CreateTag("WateringCan");
        CreateTag("Seed");
        CreateTag("Soil");
        
        // Create Interactable layer if it doesn't exist
        CreateLayer("Interactable");
        
        // Find and tag all objects
        taggedCount += TagObjects("Short Spade", "Spade");
        taggedCount += TagObjects("SM_Prop_Tool_Spade_01", "Spade");
        taggedCount += TagObjects("SpadeCollider", "Spade");
        
        taggedCount += TagObjects("WateringCan", "WateringCan");
        taggedCount += TagObjects("WateringCanPrefab", "WateringCan");
        
        taggedCount += TagObjects("ChiliSeedBag", "Seed");
        taggedCount += TagObjects("TomatoSeedBag", "Seed");
        taggedCount += TagObjects("SeedStation", "Seed");
        
        taggedCount += TagObjects("SM_Soil", "Soil");
        taggedCount += TagObjects("WaterablePlot", "Soil");
        
        Debug.Log($"✅ Setup Complete! Tagged {taggedCount} objects.");
        Debug.Log("✅ All tools/seeds set to 'Interactable' layer.");
        Debug.Log("Now add PCCrosshairUI to PCPlayer and press Play!");
    }
    
    static int TagObjects(string searchName, string tagName)
    {
        int count = 0;
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains(searchName) && IsInScene(obj))
            {
                obj.tag = tagName;
                
                // Set layer to Interactable for grabbable items
                if (tagName != "Soil")
                {
                    obj.layer = LayerMask.NameToLayer("Interactable");
                }
                
                count++;
            }
        }
        
        if (count > 0)
        {
            Debug.Log($"✅ Tagged {count} objects as '{tagName}' (searching for: {searchName})");
        }
        
        return count;
    }
    
    static bool IsInScene(GameObject obj)
    {
        return obj.hideFlags == HideFlags.None && obj.scene.IsValid();
    }
    
    static void CreateTag(string tagName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            if (tagsProp.GetArrayElementAtIndex(i).stringValue == tagName)
            {
                found = true;
                break;
            }
        }
        
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            tagsProp.GetArrayElementAtIndex(0).stringValue = tagName;
            tagManager.ApplyModifiedProperties();
            Debug.Log($"✅ Created tag: {tagName}");
        }
    }
    
    static void CreateLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");
        
        bool found = false;
        for (int i = 8; i < layersProp.arraySize; i++) // Start at 8 (user layers)
        {
            if (layersProp.GetArrayElementAtIndex(i).stringValue == layerName)
            {
                found = true;
                break;
            }
        }
        
        if (!found)
        {
            for (int i = 8; i < layersProp.arraySize; i++)
            {
                if (string.IsNullOrEmpty(layersProp.GetArrayElementAtIndex(i).stringValue))
                {
                    layersProp.GetArrayElementAtIndex(i).stringValue = layerName;
                    tagManager.ApplyModifiedProperties();
                    Debug.Log($"✅ Created layer: {layerName}");
                    break;
                }
            }
        }
    }
#endif
}
