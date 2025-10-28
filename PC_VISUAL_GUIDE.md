# ProjectNurture PC Setup - Visual Guide

## Scene Hierarchy Structure

```
YourScene
├── InputManager (NEW)
│   └── PCInputManager (Component)
│
├── PCPlayer (NEW)
│   ├── CharacterController (Component)
│   ├── PCPlayerController (Component)
│   ├── PCInteractionController (Component)
│   ├── PCUIController (Component)
│   │
│   ├── PCCamera (Child)
│   │   ├── Camera (Component, Tag: MainCamera)
│   │   │
│   │   └── HoldPosition (Child)
│   │       └── (Empty, for holding objects)
│   │
│   └── GroundCheck (Child)
│       └── (Empty, positioned at feet)
│
├── OVRCameraRig (Existing VR)
│   └── ... (Your existing VR setup)
│
├── Canvas
│   ├── Crosshair (NEW)
│   │   └── Image (Component)
│   │
│   └── InteractionPrompt (NEW)
│       └── Text (Component)
│
├── FarmingTools
│   ├── Spade
│   │   ├── Tag: Spade
│   │   ├── Layer: Interactable
│   │   ├── Rigidbody (Component)
│   │   ├── Collider (Component)
│   │   └── PCGrabbableAdapter (Optional)
│   │
│   └── WateringCan
│       ├── Tag: WateringCan
│       ├── Layer: Interactable
│       ├── Rigidbody (Component)
│       ├── Collider (Component)
│       ├── PourDetector (Component)
│       └── PCGrabbableAdapter (Optional)
│
├── SoilMounds
│   └── SoilMound_01
│       ├── Tag: Soil
│       ├── Layer: Interactable
│       ├── Soil (Component)
│       └── Collider (Component)
│
├── Seeds
│   └── TomatoSeed
│       ├── Tag: Seed
│       ├── Layer: Interactable
│       ├── Rigidbody (Component)
│       ├── PlantScript (Component)
│       └── PCGrabbableAdapter (Optional)
│
└── Plants
    └── TomatoPlant
        ├── Tag: Harvestable
        ├── Layer: Interactable
        ├── Rigidbody (Component)
        ├── HarvestScript (Component - Modified)
        └── PCGrabbableAdapter (Optional)
```

## Component Configuration Diagrams

### PCPlayer Setup
```
┌─────────────────────────────────────────┐
│          PCPlayer GameObject            │
├─────────────────────────────────────────┤
│ Transform                               │
│   Position: (0, 1, 0)                   │
├─────────────────────────────────────────┤
│ Character Controller                    │
│   Height: 1.8                           │
│   Radius: 0.3                           │
│   Center: (0, 0.9, 0)                   │
├─────────────────────────────────────────┤
│ PCPlayerController                      │
│   Walk Speed: 3.0                       │
│   Run Speed: 6.0                        │
│   Mouse Sensitivity: 2.0                │
│   Camera Transform: → PCCamera          │
│   Ground Check: → GroundCheck           │
├─────────────────────────────────────────┤
│ PCInteractionController                 │
│   Interaction Range: 3.0                │
│   Interactable Layer: Interactable      │
│   Crosshair UI: → Crosshair             │
│   Interaction Prompt: → InteractionText │
│   Hold Position: → HoldPosition         │
├─────────────────────────────────────────┤
│ PCUIController                          │
│   UI Interaction Range: 5.0             │
└─────────────────────────────────────────┘
```

### InputManager Setup
```
┌─────────────────────────────────────────┐
│       InputManager GameObject           │
├─────────────────────────────────────────┤
│ PCInputManager                          │
│   Current Input Mode: PC                │
│   Auto Detect: ✓                        │
│   ─────────────────────────────────     │
│   PC Components:                        │
│     PC Camera: → PCPlayer               │
│     PC Player Controller: → (auto)      │
│     PC Interaction Controller: → (auto) │
│   ─────────────────────────────────     │
│   VR Components:                        │
│     VR Camera Rig: → OVRCameraRig       │
│     VR Controllers:                     │
│       [0] → LeftHandAnchor              │
│       [1] → RightHandAnchor             │
└─────────────────────────────────────────┘
```

## Interaction Flow Diagram

```
Player Action Flow (PC Mode):
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

1. LOOKING AT OBJECTS
   ┌──────────────┐
   │ Player moves │
   │ mouse        │
   └──────┬───────┘
          │
          ▼
   ┌──────────────────────┐
   │ PCInteraction        │
   │ Controller casts ray │
   │ from camera center   │
   └──────┬───────────────┘
          │
          ▼
   ┌──────────────────────┐    YES    ┌─────────────────┐
   │ Hit interactable? ───┼──────────→│ Highlight object│
   └──────┬───────────────┘           │ Show prompt     │
          │                            └─────────────────┘
          │ NO
          ▼
   ┌─────────────────┐
   │ Clear highlight │
   └─────────────────┘

2. PICKING UP OBJECTS
   ┌──────────────────┐
   │ Left Mouse Click │
   └──────┬───────────┘
          │
          ▼
   ┌─────────────────────────┐    NO
   │ Currently holding? ──────┼────────┐
   └──────┬──────────────────┘        │
          │ YES                        │
          ▼                            ▼
   ┌─────────────┐            ┌───────────────┐
   │ Drop object │            │ Grab object   │
   │ Add physics │            │ Disable       │
   │ Throw force │            │ physics       │
   └─────────────┘            │ Attach to     │
                              │ hold position │
                              └───────────────┘

3. USING TOOLS
   ┌──────────────────────┐
   │ Right Mouse HOLD     │
   └──────┬───────────────┘
          │
          ▼
   ┌──────────────────────┐
   │ Is tool held?        │
   └──────┬───────────────┘
          │ YES
          ▼
   ┌──────────────────────┐
   │ PCToolController     │
   │ determines tool type │
   └──────┬───────────────┘
          │
          ├─→ SPADE ──────→ Dig animation
          │                 Trigger collision
          │
          └─→ WATER CAN ──→ Tilt rotation
                            Activate pour
```

## Object Tagging Reference

```
TAG ASSIGNMENT GUIDE
═══════════════════════════════════════════

Tools:
├─ 🔧 Spade ──────────→ Tag: "Spade"
├─ 💧 WateringCan ────→ Tag: "WateringCan"
└─ 🪴 SeedBag ────────→ Tag: "Seed" (or custom)

Environment:
├─ 🟫 SoilMounds ─────→ Tag: "Soil"
└─ 🌱 PlotAreas ──────→ Tag: (custom)

Plants:
├─ 🌱 Seeds ──────────→ Tag: "Seed"
├─ 🌿 Growing Plants ─→ Tag: (custom)
└─ 🍅 Harvestable ────→ Tag: "Harvestable"

UI:
└─ 📋 Quiz Buttons ───→ Layer: "UI" or "WorldUI"


LAYER ASSIGNMENT GUIDE
═══════════════════════════════════════════

Layer: "Interactable" (ID: 8)
├─ All tools
├─ All seeds
├─ Soil mounds
├─ Harvestable plants
└─ Any grabbable objects

Layer: "WorldUI" (ID: 9) - Optional
└─ 3D UI elements (quiz, signs)

Layer: "Default" (ID: 0)
├─ Ground plane
├─ Fences
└─ Static decorations
```

## Control Mapping Visual

```
KEYBOARD & MOUSE CONTROLS
═════════════════════════════════════════════

   ┌───┬───┬───┬───┐
   │Tab│ Q │ W │ E │    W - Forward
   ├───┼───┼───┼───┤    A - Left
   │Cap│ A │ S │ D │    S - Backward
   ├───┼───┼───┼───┤    D - Right
   │Sft│ Z │ X │ C │
   └───┴───┴───┴───┘    Shift - Run
                         E - Interact (future)

   ┌─────────────┐
   │ SPACE BAR   │       Space - Jump
   └─────────────┘

   ┌───────────────┐
   │  Left Mouse   │     Pick Up / Drop Object
   │   (Click)     │
   └───────────────┘

   ┌───────────────┐
   │  Right Mouse  │     Use Tool (Hold)
   │   (Hold)      │     • Spade: Dig
   └───────────────┘     • Watering Can: Pour

   🖱️ Mouse Movement       Look Around (360°)

   ESC                    Toggle Cursor Lock
```

## Gameplay Loop Flowchart

```
PC MODE FARMING WORKFLOW
═══════════════════════════════════════════════

START
  │
  ▼
┌─────────────────┐
│ Walk to tools   │ ← WASD Movement
│ (table)         │   Mouse Look
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Look at spade   │ ← Mouse to aim at object
│ See highlight   │   Crosshair + Highlight
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Left-click      │ ← Pick up spade
│ Pick up spade   │   Object follows view
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Walk to soil    │ ← WASD to move
│ mound           │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Look at soil    │ ← Aim at soil mound
│ Right-click 2x  │   Hold right mouse
│ Dig hole        │   Repeat twice
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Left-click      │ ← Drop spade
│ Drop spade      │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Pick up seeds   │ ← Left-click on seeds
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Left-click      │ ← Place seeds in hole
│ in hole         │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Pick up spade   │ ← Cover hole
│ Right-click     │   One dig motion
│ on hole         │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Pick up water   │ ← Left-click watering can
│ can             │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Right-click +   │ ← Hold to pour
│ hold to pour    │   Release to stop
│ Watch bar fill  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Wait for plant  │ ← Automated growth
│ to grow         │   Visual feedback
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Left-click      │ ← Harvest mature plant
│ Harvest plant   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Go to quiz      │ ← Walk to quiz area
│ Click answers   │   Click with mouse
└────────┬────────┘
         │
         ▼
       END
```

## Troubleshooting Decision Tree

```
PROBLEM SOLVING FLOWCHART
═════════════════════════════════════════════

Can't move character?
├─ NO → Check PCPlayerController enabled
├─ NO → Check CharacterController present
└─ NO → Press ESC to lock cursor

Can't pick up objects?
├─ Check object has tag (Spade, Seed, etc.)
├─ Check object on "Interactable" layer
├─ Check object has Collider
├─ Check interaction range (default 3m)
└─ Check crosshair pointing at object

Tool doesn't work?
├─ SPADE not digging?
│  └─ Hold RIGHT mouse, not left
│  └─ Must be looking at soil
│  └─ Soil must have "Soil" tag
│
└─ WATER not pouring?
   └─ Hold RIGHT mouse
   └─ PourDetector component present?
   └─ Wait for tilt animation

VR mode when you want PC?
├─ Check InputManager
├─ Uncheck "Auto Detect"
└─ Set to "PC" mode manually

PC mode when you want VR?
├─ Check VR headset connected
├─ Check XR Plugin enabled
└─ InputManager auto-detect ON
```

---

## Quick Reference Card

```
╔═══════════════════════════════════════════╗
║     PROJECTNURTURE PC QUICK REFERENCE     ║
╠═══════════════════════════════════════════╣
║ MOVEMENT                                  ║
║  W/A/S/D ......... Move                   ║
║  Mouse ........... Look                   ║
║  Shift ........... Run                    ║
║  Space ........... Jump                   ║
║  ESC ............. Toggle Cursor          ║
║                                           ║
║ INTERACTION                               ║
║  Left Click ...... Pick Up/Drop           ║
║  Right Click ..... Use Tool               ║
║                                           ║
║ SETUP CHECKLIST                           ║
║  □ PCPlayer created                       ║
║  □ InputManager configured                ║
║  □ Tags assigned                          ║
║  □ Layers assigned                        ║
║  □ UI elements created                    ║
║                                           ║
║ HELP                                      ║
║  See: PC_CONTROLS_SETUP.md                ║
║  See: PC_SETUP_CHECKLIST.md               ║
╚═══════════════════════════════════════════╝
```
