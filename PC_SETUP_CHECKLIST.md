# Quick Setup Checklist for PC Controls

## ✅ Pre-Setup Requirements
- [ ] Unity 2021.1.0f1 or higher installed
- [ ] ProjectNurture project open in Unity
- [ ] All new scripts copied to `Assets/Scripts/` folder

## ✅ Scene Configuration

### 1. Create PC Player
- [ ] Create Empty GameObject named "PCPlayer"
- [ ] Add `Character Controller` component
- [ ] Add `PCPlayerController` script
- [ ] Add `PCInteractionController` script  
- [ ] Add `PCUIController` script
- [ ] Position at spawn point (0, 1, 0)

### 2. Create PC Camera
- [ ] Create child of PCPlayer named "PCCamera"
- [ ] Add `Camera` component
- [ ] Set position (0, 1.6, 0) - eye height
- [ ] Tag as "MainCamera"
- [ ] Assign to PCPlayerController's `cameraTransform` field

### 3. Create Ground Check
- [ ] Create child of PCPlayer named "GroundCheck"
- [ ] Position at (0, -0.9, 0) - foot level
- [ ] Assign to PCPlayerController's `groundCheck` field

### 4. Setup Input Manager
- [ ] Create Empty GameObject named "InputManager"
- [ ] Add `PCInputManager` script
- [ ] Assign `PCPlayer` to "PC Camera" field
- [ ] Assign VR rig to "VR Camera Rig" field
- [ ] Assign VR controllers to "VR Controllers" array
- [ ] Check "Auto Detect" option

## ✅ Project Settings

### 1. Create Layers
- [ ] Create layer "Interactable" (Layer 8 or any available)
- [ ] Create layer "WorldUI" (optional, for 3D UI)

### 2. Create Tags
- [ ] Tag: "Spade"
- [ ] Tag: "WateringCan"
- [ ] Tag: "Seed"
- [ ] Tag: "Soil"
- [ ] Tag: "Harvestable"

### 3. Input Settings
Go to Edit > Project Settings > Input Manager:
- [ ] Verify "Horizontal" axis exists (A/D, Left/Right arrows)
- [ ] Verify "Vertical" axis exists (W/S, Up/Down arrows)
- [ ] Verify "Jump" button exists (Space)
- [ ] Verify "Mouse X" axis exists
- [ ] Verify "Mouse Y" axis exists

## ✅ Object Configuration

### Tools Setup
For each tool (Spade, Watering Can, etc.):
- [ ] Has `Rigidbody` component
- [ ] Has `Collider` component (Box/Mesh/Capsule)
- [ ] Tagged appropriately (Spade, WateringCan, etc.)
- [ ] Layer set to "Interactable"

### Seeds Setup
- [ ] Has `PlantScript` component
- [ ] Has `Rigidbody` component
- [ ] Tagged as "Seed"
- [ ] Layer set to "Interactable"
- [ ] Add `PCGrabbableAdapter` component (optional, added automatically)

### Soil Mounds Setup
- [ ] Has `Soil` script
- [ ] Has `Collider` component
- [ ] Tagged as "Soil"
- [ ] Layer set to "Interactable"

### Harvestable Plants Setup
- [ ] Has `HarvestScript` component (modified version)
- [ ] Has `Rigidbody` component
- [ ] Tagged as "Harvestable"
- [ ] Layer set to "Interactable"

## ✅ UI Configuration

### Create Crosshair
- [ ] Open Canvas in scene
- [ ] Create UI > Image
- [ ] Name it "Crosshair"
- [ ] Anchor to center
- [ ] Position (0, 0)
- [ ] Add crosshair sprite/texture
- [ ] Assign to PCInteractionController > "Crosshair UI"

### Create Interaction Prompt
- [ ] Create UI > Text
- [ ] Name it "InteractionPrompt"
- [ ] Position below crosshair or at bottom
- [ ] Set font size (16-24)
- [ ] Set color (white with outline)
- [ ] Assign to PCInteractionController > "Interaction Prompt Text"

### Quiz UI (if applicable)
- [ ] Add `Box Collider` to quiz buttons
- [ ] Set layer to "WorldUI" or "UI"
- [ ] Ensure `AnswerScript` components are attached
- [ ] Configure PCUIController > "UI Layer" to include button layer

## ✅ PCPlayerController Settings

### Movement
- [ ] Walk Speed: 3.0
- [ ] Run Speed: 6.0
- [ ] Gravity: -9.81
- [ ] Jump Height: 1.0

### Mouse Look
- [ ] Mouse Sensitivity: 2.0
- [ ] Vertical Look Limit: 80.0

### References
- [ ] Camera Transform: Assigned (PCCamera)
- [ ] Ground Check: Assigned
- [ ] Ground Distance: 0.4
- [ ] Ground Mask: Set to "Default" or ground layer

## ✅ PCInteractionController Settings

### Interaction
- [ ] Interaction Range: 3.0
- [ ] Interactable Layer: "Interactable" layer selected
- [ ] Interact Key: E
- [ ] Grab Key: Mouse0 (Left Click)
- [ ] Use Key: Mouse1 (Right Click)

### Visual Feedback
- [ ] Crosshair UI: Assigned
- [ ] Interaction Prompt Text: Assigned
- [ ] Highlight Color: Yellow or preferred color

### Held Object
- [ ] Hold Distance: 1.5
- [ ] Throw Force: 10.0

## ✅ Testing Checklist

### Basic Movement
- [ ] WASD moves character
- [ ] Mouse looks around smoothly
- [ ] Shift makes character run
- [ ] Space makes character jump
- [ ] Character doesn't fall through floor
- [ ] ESC toggles cursor lock

### Object Interaction
- [ ] Crosshair shows when looking at objects
- [ ] Prompt text appears when near interactable
- [ ] Left-click picks up tools
- [ ] Objects follow camera when held
- [ ] Left-click again drops objects
- [ ] Dropped objects have physics

### Tool Usage
- [ ] Can pick up spade
- [ ] Right-click with spade performs dig animation
- [ ] Spade triggers soil collision properly
- [ ] Can pick up watering can
- [ ] Right-click with watering can pours water
- [ ] Water stream appears when pouring

### Seed Planting
- [ ] Can pick up seeds
- [ ] Seeds can be placed in dug holes
- [ ] Seeds snap to correct position
- [ ] Soil script recognizes seed placement

### Harvesting
- [ ] Mature plants are highlightable
- [ ] Can pick up harvestable vegetables
- [ ] Harvesting triggers proper events

### Quiz System
- [ ] Can click quiz buttons with mouse
- [ ] Answers register correctly
- [ ] Score updates properly

## ✅ Build Configuration

### PC Build Settings
- [ ] Go to File > Build Settings
- [ ] Platform: PC, Mac & Linux Standalone
- [ ] Target Platform: Windows
- [ ] Architecture: x86_64
- [ ] Development Build: Check for testing
- [ ] Add Main scene to "Scenes In Build"

### Player Settings
- [ ] Company Name: Set
- [ ] Product Name: ProjectNurture
- [ ] Default Icon: Set
- [ ] Fullscreen Mode: Fullscreen Window or Windowed
- [ ] Default Screen Width: 1920
- [ ] Default Screen Height: 1080

## ✅ Optional Enhancements

- [ ] Add pause menu (ESC key)
- [ ] Add settings menu for sensitivity
- [ ] Add keybinding configuration
- [ ] Add on-screen control hints
- [ ] Add tutorial for PC controls
- [ ] Create separate PC build without VR assets

## 🎯 Final Verification

Before distributing:
- [ ] Test full gameplay loop in PC mode
- [ ] Test full gameplay loop in VR mode (if keeping both)
- [ ] Verify no errors in console
- [ ] Test build executable (not just editor)
- [ ] Create README with PC controls
- [ ] Test on different Windows PC if possible

## 📝 Notes Section

Use this space for project-specific notes:

```
Date: ___________
Tester: ___________
Issues Found:
- 
- 
- 

Additional Comments:
- 
- 
```

---

**When checked all items, your PC controls should be fully functional!**

For detailed instructions, see `PC_CONTROLS_SETUP.md`
