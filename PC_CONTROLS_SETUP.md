# PC Keyboard & Mouse Support for ProjectNurture

## Overview
This document explains the PC compatibility system added to ProjectNurture, allowing the VR farming game to be played with keyboard and mouse on Windows.

## New Scripts Created

### Core System Scripts

1. **PCInputManager.cs**
   - Manages switching between VR and PC input modes
   - Auto-detects if VR headset is connected
   - Enables/disables appropriate camera and control systems
   - Location: `Assets/Scripts/PCInputManager.cs`

2. **PCPlayerController.cs**
   - First-person camera and movement controller
   - WASD for movement, Mouse for looking
   - Shift to run, Space to jump
   - Location: `Assets/Scripts/PCPlayerController.cs`

3. **PCInteractionController.cs**
   - Raycast-based object interaction system
   - Replaces VR hand grabbing with mouse clicks
   - Handles picking up, holding, and using tools
   - Location: `Assets/Scripts/PCInteractionController.cs`

4. **PCToolController.cs**
   - Tool-specific behavior for PC mode
   - Handles spade digging and watering can pouring
   - Automatically attached to tools when picked up
   - Location: `Assets/Scripts/PCToolController.cs`

5. **PCUIController.cs**
   - Makes VR UI elements clickable with mouse
   - Handles quiz interactions
   - Location: `Assets/Scripts/PCUIController.cs`

6. **PCGrabbableAdapter.cs**
   - Compatibility layer for AutoHand Grabbable component
   - Provides same interface for VR and PC modes
   - Location: `Assets/Scripts/PCGrabbableAdapter.cs`

### Modified Scripts

1. **HarvestScript.cs**
   - Updated to work with both VR (AutoHand) and PC (PCGrabbableAdapter)
   - Now checks for available input mode and adapts accordingly

## PC Controls

### Movement
- **W** - Move forward
- **A** - Move left
- **S** - Move backward
- **D** - Move right
- **Mouse** - Look around
- **Left Shift** - Run (hold)
- **Space** - Jump
- **Escape** - Toggle cursor lock

### Interaction
- **Left Mouse Button** - Pick up / Drop object
- **Right Mouse Button** - Use tool (hold)
  - With Spade: Dig soil
  - With Watering Can: Pour water
- **E** - Generic interact (if needed for future features)

### UI Interaction
- **Left Mouse Button** - Click buttons and quiz answers

## Setup Instructions

### 1. Scene Setup

#### A. Create PC Camera System
1. In your Main scene, create a new Empty GameObject
2. Name it "PCPlayer"
3. Add these components:
   - Character Controller (adjust height/radius to match player size)
   - PCPlayerController script
   - PCInteractionController script
   - PCUIController script

4. Under PCPlayer, create a child GameObject named "PCCamera"
5. Add a Camera component to PCCamera
6. Position PCCamera at approximate eye height (Y = 1.6)

#### B. Create Ground Check
1. Under PCPlayer, create an empty child named "GroundCheck"
2. Position it slightly below the character's feet (Y = -0.9 typically)
3. In PCPlayerController, assign GroundCheck to the groundCheck field

#### C. Setup Input Manager
1. Create an empty GameObject in the scene named "InputManager"
2. Add the PCInputManager script
3. Configure the script:
   - **PC Camera**: Drag the PCPlayer GameObject
   - **VR Camera Rig**: Drag your OVRCameraRig or similar VR rig
   - **VR Controllers**: Drag your left and right VR hand controllers
   - **PC Player Controller**: Will auto-assign from PCPlayer
   - **PC Interaction Controller**: Will auto-assign from PCPlayer

### 2. Layer Setup
1. Create a new layer called "Interactable"
2. Assign this layer to:
   - All tool objects (spade, watering can, etc.)
   - Soil mounds
   - Seed bags
   - Harvestable plants
   - Any other interactive objects

3. In PCInteractionController, set the "Interactable Layer" to include the "Interactable" layer

### 3. Tag Setup
Ensure the following tags exist and are assigned:
- **Spade** - Assigned to spade tool
- **WateringCan** - Assigned to watering can
- **Seed** - Assigned to seed objects
- **Soil** - Assigned to soil mounds
- **Harvestable** - Assigned to mature plants that can be harvested

### 4. UI Setup

#### A. Create Crosshair
1. In your Canvas, create an Image
2. Name it "Crosshair"
3. Set it to center of screen
4. Use a simple crosshair sprite
5. Assign it to the PCInteractionController's "Crosshair UI" field

#### B. Create Interaction Prompt
1. In your Canvas, create a Text element
2. Name it "InteractionPrompt"
3. Position it near the crosshair or bottom center
4. Assign it to PCInteractionController's "Interaction Prompt Text" field

#### C. Make Existing UI 3D Clickable
For quiz buttons and other VR UI:
1. Add Box Collider components to button objects
2. Set their layer to "UI" or create a "WorldUI" layer
3. Configure PCUIController to detect this layer

### 5. Object Setup

#### A. Tools (Spade, Watering Can, etc.)
For each tool:
1. Ensure it has a Rigidbody component
2. Ensure it has a Collider component
3. Add appropriate tag (Spade, WateringCan)
4. Set layer to "Interactable"
5. If using AutoHand in VR mode, keep the Grabbable component

#### B. Seeds
1. Add PCGrabbableAdapter component
2. Ensure PlantScript is attached
3. Tag as "Seed"
4. Layer as "Interactable"

#### C. Harvestable Plants
1. Ensure HarvestScript is attached (now compatible with both modes)
2. Tag as "Harvestable"
3. Layer as "Interactable"

### 6. Build Settings
1. Go to File > Build Settings
2. Ensure "PC, Mac & Linux Standalone" is selected
3. Target Platform: Windows
4. Architecture: x86_64

## Testing

### Test in Editor
1. Make sure no VR headset is connected, or disable XR in Project Settings
2. PCInputManager should automatically switch to PC mode
3. Press Play
4. You should spawn at PCPlayer position
5. Test all controls

### Force PC Mode
If you want to force PC mode even with VR headset connected:
1. Select the InputManager GameObject
2. In PCInputManager component, uncheck "Auto Detect"
3. Set "Current Input Mode" to "PC"

### Force VR Mode
1. Uncheck "Auto Detect"
2. Set "Current Input Mode" to "VR"

## Troubleshooting

### Camera Not Moving
- Check that PCPlayerController is enabled
- Verify Camera is assigned in PCPlayerController
- Check cursor is locked (press ESC to toggle)

### Can't Pick Up Objects
- Verify objects have correct tags
- Check layer mask in PCInteractionController includes "Interactable"
- Ensure objects have Collider components
- Check interaction range (default 3 units)

### Watering Can Not Pouring
- Ensure PourDetector component is on the watering can
- PCToolController will automatically handle tilting
- Hold Right Mouse Button to pour

### Spade Not Digging
- Ensure soil has proper colliders
- Tag must be "Spade" on spade object
- Hold Right Mouse Button while looking at soil

### VR Mode Not Working
- Ensure XR Plugin Management is installed
- Check Oculus Integration is properly configured
- AutoHand components should still be on objects
- PCInputManager should detect VR headset automatically

## Advanced Configuration

### Adjusting Movement Speed
In PCPlayerController:
- `walkSpeed` - Normal walking speed (default: 3)
- `runSpeed` - Running speed when holding Shift (default: 6)
- `mouseSensitivity` - Mouse look sensitivity (default: 2)

### Adjusting Interaction Range
In PCInteractionController:
- `interactionRange` - How far you can interact (default: 3 units)

### Customizing Controls
To change key bindings, modify the KeyCode values in PCInteractionController:
```csharp
public KeyCode interactKey = KeyCode.E;
public KeyCode grabKey = KeyCode.Mouse0;
public KeyCode useKey = KeyCode.Mouse1;
```

## Performance Optimization

### For PC-Only Build
If building PC-only version:
1. Remove or disable VR components in build
2. In Project Settings > XR Plugin Management, disable XR
3. This will reduce build size and improve performance

### For Dual VR/PC Build
Keep both systems, PCInputManager will handle switching automatically based on VR headset detection.

## Known Limitations

1. **AutoHand Compatibility**: Some advanced AutoHand features may not work in PC mode
2. **Two-Handed Interactions**: Currently, PC mode doesn't support two-handed tool usage
3. **Haptic Feedback**: Obviously not available in PC mode
4. **Physical Gestures**: VR-specific gestures are replaced with button presses

## Future Enhancements

Potential improvements for PC mode:
1. Add inventory system (1-9 number keys for tool selection)
2. Implement mouse wheel for additional interactions
3. Add contextual tooltips
4. Implement gamepad support
5. Add custom cursor based on held object
6. Improve tool animations in PC mode

## Support

For issues or questions:
1. Check console for error messages
2. Verify all setup steps completed
3. Test in a fresh scene with minimal setup
4. Check Unity version compatibility (tested with 2021.1.0f1)

---

**Version**: 1.0
**Last Updated**: October 2025
**Compatible Unity Version**: 2021.1.0f1+
