# 🧹 CLEAN HIERARCHY SETUP GUIDE

## Current Issues I See

Looking at your hierarchy, I can see:
1. ✅ PCPlayer exists and is configured
2. ⚠️ Need to add **PCCrosshairUI** component
3. ⚠️ Need to verify object tags
4. ⚠️ Need to check Ground Mask setting

---

## 🎯 QUICK FIX STEPS

### STEP 1: Add Crosshair System

1. **Select PCPlayer** in Hierarchy
2. In **Inspector**, click **Add Component**
3. Type: **PCCrosshairUI**
4. Press Enter

✅ **Crosshair will auto-create!**
- White crosshair in center of screen
- Turns GREEN when looking at tools/seeds
- Turns YELLOW when looking at soil

---

### STEP 2: Verify Ground Mask (IMPORTANT!)

I see in your screenshot: **Ground Mask is "Everything"** ✅ **GOOD!**

But if you have issues:
1. Select **PCPlayer**
2. Find **PC Player Controller (Script)**
3. Set **Ground Mask** to "Default" or "Everything"

---

### STEP 3: Verify Tags on Objects

Your tools need correct tags:

#### To Check/Fix Tags:

**For Spade:**
1. Find the **Spade** object in Hierarchy (look in ToolStation or on table)
2. Select it
3. In Inspector, top dropdown next to the name
4. Select **Tag** → **Spade**
5. If "Spade" tag doesn't exist, click "Add Tag" and create it

**For Watering Can:**
1. Find **WateringCan** object
2. Tag → **WateringCan**

**For Seeds:**
1. Find seed objects
2. Tag → **Seed**

**For Soil Mounds:**
1. Find soil objects (the dirt mounds)
2. Tag → **Soil**

---

### STEP 4: Set Layers

1. Select all tools (Spade, WateringCan, Seeds)
2. In Inspector, set **Layer** → **Interactable**
3. If "Interactable" doesn't exist:
   - Go to Edit → Project Settings → Tags and Layers
   - Find empty layer slot
   - Type: **Interactable**

---

### STEP 5: Verify PCInteractionController Settings

1. Select **PCPlayer**
2. Find **PC Interaction Controller (Script)**
3. Check these settings:

```
Interaction Range: 3.0
Interactable Layer: Select "Interactable"
Grab Key: Mouse0
Use Key: Mouse1
Hold Distance: 1.5
Throw Force: 10
```

---

## 📋 CLEAN HIERARCHY STRUCTURE

Your hierarchy should look like:

```
Main (Scene)
├── Terrain ✅
├── Crate ✅
├── OVRPlayerController (VR - can disable in PC mode)
├── PCPlayer ⭐ YOUR PC PLAYER
│   ├── Character Controller
│   ├── PC Player Controller (Script)
│   ├── PC Interaction Controller (Script)
│   ├── PC UI Controller (Script)
│   ├── PC Crosshair UI (Script) ⭐ ADD THIS
│   │
│   ├── PCCamera
│   │   ├── Camera
│   │   └── HoldPosition
│   │
│   └── GroundCheck
│
├── InputManager (if you have one)
│   └── PC Input Manager (Script)
│
├── Canvas
│   ├── Crosshair (auto-created by PCCrosshairUI)
│   └── InteractionPrompt (text element)
│
├── ToolStation (or wherever your tools are)
│   ├── Spade (Tag: Spade, Layer: Interactable)
│   ├── WateringCan (Tag: WateringCan, Layer: Interactable)  
│   └── SeedBag (Tag: Seed, Layer: Interactable)
│
├── Plots (or soil areas)
│   ├── SoilMound1 (Tag: Soil, Layer: Interactable)
│   ├── SoilMound2 (Tag: Soil, Layer: Interactable)
│   └── ...
│
└── [Other scene objects...]
```

---

## 🎨 TESTING THE CROSSHAIR

After adding **PCCrosshairUI**:

1. **Press Play**
2. **Look at the center of screen**
3. **You should see a WHITE crosshair** (+ shape with dot)
4. **Walk to tools** (WASD)
5. **Look at spade**
6. **Crosshair turns GREEN** ✅
7. **Text appears at bottom** ✅
8. **Left click to pick up**

---

## 🔍 FINDING YOUR OBJECTS

### To Find Spade:
1. In Hierarchy search box (top), type: **spade**
2. Or look in: **ToolStation** → Tools → Spade
3. Or look for objects with **Spade** in name

### To Find Watering Can:
1. Search: **water**
2. Look for: WateringCan, WaterCan, or similar

### To Find Seeds:
1. Search: **seed**
2. Look for: SeedBag, Seeds, or similar

### To Find Soil:
1. Search: **soil**
2. Or look for objects with mesh that looks like dirt mounds
3. Usually under: Plots, Garden, or SoilAreas

---

## ⚙️ QUICK SETUP SCRIPT

**Want to do this automatically?**

1. Create empty GameObject: **"SetupHelper"**
2. Add script: **PCSetupHelper**
3. Right-click component → **Auto Setup PC Controls**
4. It will create everything!

---

## 🎯 MINIMAL WORKING SETUP

**Absolute minimum to work:**

1. ✅ PCPlayer with all components
2. ✅ PCCrosshairUI component added
3. ✅ Ground Mask set to "Default" or "Everything"
4. ✅ At least ONE object tagged "Spade"
5. ✅ At least ONE object tagged "Soil"

That's it! You can then pick up the spade and dig!

---

## 🐛 TROUBLESHOOTING

### "I don't see a crosshair!"
- Check Canvas exists
- Check PCCrosshairUI component is on PCPlayer
- Press Play to see it

### "Crosshair doesn't change color!"
- Check objects have correct tags
- Check you're close enough (within 3 meters)
- Check you're looking directly at the object

### "Nothing highlights when I look at it!"
- Tags are missing or wrong
- Layer is not "Interactable"
- Object doesn't have a Collider

### "Can't find objects in Hierarchy!"
- Use search box at top of Hierarchy
- Check if objects are children of other objects (expand arrows)
- Look in common parent objects like "Tools", "ToolStation", "Plots"

---

## ✅ FINAL CHECKLIST

Before testing:
- [ ] PCPlayer has PCCrosshairUI component
- [ ] Ground Mask is set (not "Nothing")
- [ ] Spade has "Spade" tag
- [ ] WateringCan has "WateringCan" tag
- [ ] Seeds have "Seed" tag
- [ ] Soil has "Soil" tag
- [ ] All tools/seeds on "Interactable" layer
- [ ] Canvas exists in scene
- [ ] Press Play to test!

---

**Once this is done, you'll have:**
- ✅ Visible crosshair that changes color
- ✅ Clear visual feedback
- ✅ Easy to demonstrate
- ✅ Professional look

**Need help finding specific objects? Tell me what you see in your Hierarchy and I'll guide you!**
