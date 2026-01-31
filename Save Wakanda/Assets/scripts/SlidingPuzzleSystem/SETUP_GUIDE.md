# Sliding Puzzle Game - Quick Setup Guide

## 📁 File Structure
All scripts should be placed in your Unity project under:
```
Assets/Scripts/SlidingPuzzle/
```

## 🎯 Quick Setup (5 Steps)

### Step 1: Create the UI Structure
1. In your scene, create a **Canvas** (if you don't have one)
2. Under Canvas, create an **Empty GameObject** called `PuzzlePanel`
   - Add **CanvasGroup** component (to fade in/out if needed)
3. Under PuzzlePanel, create an **Empty GameObject** called `TileContainer`
   - Set **RectTransform** anchor to top-left
   - This is where tiles will spawn

### Step 2: Create Tile Prefab
1. Right-click in Project → UI → Button
2. Rename it to `PuzzleTile`
3. Make it a prefab by dragging to Project folder
4. Configure the tile:
   - Remove any child Text if you don't want numbers shown
   - Adjust Button colors (optional)
   - Add Image component if not present
   - Set size to 100x100 (this will be overridden at runtime)

### Step 3: Create Manager Objects in Scene
Create empty GameObjects for each system:

1. **PuzzleSystemManager** (parent object to keep things organized)
   - Add `PuzzleGameManager.cs`
   
2. Under PuzzleSystemManager, create:
   - **PuzzleLogic** → Add `PuzzleManager.cs`
   - **MaskRewards** → Add `MaskRewardSystem.cs`

3. On your Canvas or PuzzlePanel:
   - Add `PuzzleUIController.cs`

### Step 4: Create Puzzle Configuration Asset
1. Right-click in Project → Create → **Sliding Puzzle → Puzzle Config**
2. Name it something like `Puzzle_01`
3. Configure it:
   - **Puzzle Image**: Drag your texture/image for the puzzle
   - **Grid Size**: 3 (for 3x3 = 8 tiles, easier) or 4 (for 4x4 = 15 tiles)
   - **Mask Prefab**: Drag your 3D mask model here
   - **Mask Spawn Offset**: Adjust where mask appears (e.g., `0, 1.5, 2`)
   - **Ghost Object**: This will be assigned at runtime or you can assign in inspector
   - **Defeat Animation Trigger**: Name of your animator trigger (e.g., "Defeat")
   - **Ghost Destroy Delay**: How long before ghost disappears (e.g., 3 seconds)

### Step 5: Wire Everything Together

#### On PuzzleGameManager:
```
Puzzle Configs: Add your puzzle configuration(s)
Puzzle Manager: Drag PuzzleLogic object
Puzzle UI Controller: Drag your Canvas/PuzzlePanel
Mask Reward System: Drag MaskRewards object
Puzzle UI Panel: Drag the PuzzlePanel GameObject
Player Controller: Drag your player controller script
```

#### On PuzzleManager:
```
Current Puzzle: Leave empty (set at runtime)
Debug Mode: ✓ Check this for testing
Debug Win Key: W (press W to auto-solve)
```

#### On PuzzleUIController:
```
Puzzle Manager: Drag PuzzleLogic object
Tile Container: Drag the TileContainer RectTransform
Tile Prefab: Drag your PuzzleTile prefab
Puzzle Size: 500 (adjust based on your UI)
Tile Spacing: 5
```

#### On MaskRewardSystem:
```
Current Config: Leave empty (set at runtime)
Spawn Reference Point: Drag the puzzle table transform
```

#### On Puzzle Table (3D Object):
1. Add `PuzzleInteractable.cs`
2. Configure:
```
Game Manager: Drag PuzzleSystemManager
Interact Key: E
Interaction Range: 3
```

## 🎮 Testing Workflow

### Quick Test (No Ghost Required)
1. Create a simple cube in your scene as the "puzzle table"
2. Add PuzzleInteractable to it
3. Press Play
4. Walk up to cube and press E
5. Puzzle UI should appear
6. Press W to auto-solve (if Debug Mode enabled)
7. Mask should spawn

### Full Test (With Ghost)
1. Place your ghost model in the scene
2. Make sure it has an Animator with "Defeat" trigger
3. In your Puzzle Configuration:
   - Drag the ghost to **Ghost Object** field
4. Complete puzzle normally or press W
5. Ghost should play defeat animation and disappear

## 🔧 Troubleshooting

**Puzzle UI doesn't appear:**
- Check that PuzzlePanel is a child of Canvas
- Verify PuzzleUIController has all references set
- Check Console for errors

**Tiles don't show images:**
- Make sure your puzzle image is readable (Import Settings → Read/Write Enabled)
- Check that texture is assigned in Puzzle Configuration

**Can't interact with puzzle table:**
- Make sure player has "Player" tag
- Check interaction range (increase if needed)
- Add a collider to puzzle table if using triggers

**Mask doesn't spawn:**
- Verify Mask Prefab is assigned in configuration
- Check Spawn Reference Point is set
- Look at console for spawn position

**Ghost doesn't defeat:**
- Check Ghost Object is assigned
- Verify Animator has the trigger name you specified
- Make sure ghost isn't destroyed before animation plays

## 🚀 Hackathon Speed Tips

### For Multiple Puzzles:
1. Duplicate your Puzzle Configuration
2. Change the image and mask
3. Add to the Puzzle Configs list in PuzzleGameManager
4. They'll auto-progress after each completion

### Debug Shortcuts:
- **Press W** during puzzle to auto-solve (when Debug Mode enabled)
- **Right-click PuzzleGameManager** → Skip Current Puzzle
- **Right-click PuzzleGameManager** → Reset All Puzzles

### Audio (Optional):
Add AudioSource to PuzzleUIController and MaskRewardSystem, then assign:
- Tile Click Sound
- Puzzle Solved Sound  
- Mask Spawn Sound
- Ghost Defeat Sound

## 📋 Checklist Before Building

- [ ] All scripts compiled without errors
- [ ] At least one Puzzle Configuration created
- [ ] Tile prefab created and assigned
- [ ] UI hierarchy set up correctly
- [ ] All manager references connected
- [ ] Player controller can be disabled
- [ ] Puzzle table has interactable script
- [ ] Tested interaction (press E)
- [ ] Tested puzzle solving (press W)
- [ ] Mask spawns correctly
- [ ] Ghost defeat works

## 🎨 Customization Ideas

**Easy Wins:**
- Adjust tile spacing and puzzle size for different screen sizes
- Add particle effects at mask spawn
- Add screen fade when entering/exiting puzzle
- Add timer to track solve speed
- Add move counter

**Medium:**
- Add difficulty selection (different grid sizes)
- Add shuffle animation
- Add tile slide animation
- Multiple camera angles

**Advanced:**
- Save/load puzzle progress
- Leaderboard for fastest times
- Hint system (show correct position briefly)
- Progressive difficulty (larger grids)

Good luck with your hackathon! 🎉
