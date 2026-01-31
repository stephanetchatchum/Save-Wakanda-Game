# Mask Puzzle Game - Unity Setup Guide

## 📋 Project Overview
A sliding tile puzzle game with multiple levels where players reveal mask images by solving puzzles.

## 🎯 Features
- ✅ 3 difficulty levels (3x3, 4x4, 5x5 grids)
- ✅ Level progression system
- ✅ Smooth tile animations
- ✅ Move counter
- ✅ Win detection
- ✅ Custom mask images per level
- ✅ Smart shuffling (always solvable)

---

## 📂 File Structure
```
Assets/
├── Scripts/
│   ├── Tile.cs              (Individual tile behavior)
│   ├── PuzzleManager.cs     (Main game logic)
│   ├── LevelManager.cs      (Level progression)
│   ├── LevelData.cs         (Level configuration)
│   └── UIManager.cs         (UI controls)
├── Prefabs/
│   ├── TilePrefab.prefab
│   └── LevelButton.prefab
├── Levels/
│   ├── Level1.asset
│   ├── Level2.asset
│   └── Level3.asset
└── Sprites/
    ├── MaskLevel1.png
    ├── MaskLevel2.png
    └── MaskLevel3.png
```

---

## 🚀 Unity Setup Steps

### Step 1: Create Unity Project
1. Open Unity Hub
2. Create new project: **2D** or **Universal 2D**
3. Name it: `MaskPuzzleGame`

### Step 2: Add Scripts
1. In Unity, create folder: `Assets/Scripts`
2. Copy all `.cs` files from this package into `Assets/Scripts`
3. Wait for Unity to compile

### Step 3: Create Tile Prefab
1. In Hierarchy, right-click → UI → Image
2. Rename it to "TilePrefab"
3. Add components:
   - **Button** component
   - **Tile** script (drag from Scripts folder)
4. Configure:
   - Set Image component reference to itself
   - Optional: Add Text child for tile numbers
5. Drag to `Assets/Prefabs` folder to create prefab
6. Delete from Hierarchy

### Step 4: Create Level Data Assets
1. Right-click in Project → Create → Puzzle → Level Data
2. Create 3 levels:
   - `Level1.asset` - 3x3 grid, 30 shuffle moves
   - `Level2.asset` - 4x4 grid, 50 shuffle moves
   - `Level3.asset` - 5x5 grid, 80 shuffle moves
3. Configure each level:
   - Set Level Number (1, 2, 3)
   - Set Level Name ("Easy Mask", "Medium Mask", "Hard Mask")
   - Set Grid Size (3, 4, 5)
   - Set Shuffle Moves
   - **Assign Mask Sprite** (your mask images)

### Step 5: Create Main Scene UI

#### Canvas Setup
1. Hierarchy → UI → Canvas
2. Canvas Scaler: Scale With Screen Size (1920x1080)

#### Game Panel
```
Canvas
└── GamePanel
    ├── GridContainer (RectTransform + Image + Grid Layout Group)
    ├── TopPanel
    │   ├── LevelText (Text)
    │   └── MovesText (Text)
    ├── ButtonPanel
    │   ├── ShuffleButton
    │   ├── ResetButton
    │   └── MenuButton
    └── CompleteMaskImage (Image - Hidden by default)
```

#### Win Panel
```
Canvas
└── WinPanel (Initially disabled)
    ├── Background (Image with semi-transparent black)
    ├── TitleText ("Puzzle Complete!")
    ├── MovesText ("Completed in X moves")
    ├── NextLevelButton
    └── MainMenuButton
```

### Step 6: Setup Game Manager GameObject
1. Hierarchy → Create Empty → Name: "GameManager"
2. Add components:
   - **PuzzleManager** script
   - **LevelManager** script
   - **UIManager** script

### Step 7: Connect References

#### PuzzleManager Inspector
- Current Level: Assign Level1.asset (or leave empty if using LevelManager)
- Grid Container: Drag GridContainer from Hierarchy
- Tile Prefab: Drag TilePrefab from Prefabs folder
- Complete Mask Image: Drag CompleteMaskImage
- Moves Text: Drag MovesText
- Level Text: Drag LevelText
- Win Panel: Drag WinPanel

#### LevelManager Inspector
- Levels: Add all 3 level assets (Level1, Level2, Level3)
- Puzzle Manager: Drag GameManager (it will find the component)

#### UIManager Inspector
- Puzzle Manager: Drag GameManager
- Level Manager: Drag GameManager
- Assign all panel and button references

---

## 🎨 Adding Your Mask Images

### Image Requirements
- Format: PNG with transparency (recommended)
- Size: 512x512 or 1024x1024 pixels (square)
- Import Settings in Unity:
  - Texture Type: Sprite (2D and UI)
  - Sprite Mode: Single
  - Read/Write Enabled: ✅ **IMPORTANT**
  - Compression: None (for best quality)

### Steps
1. Drag your mask images into `Assets/Sprites`
2. Select each image and change import settings (see above)
3. Click "Apply"
4. Assign to Level Data:
   - Select Level1.asset
   - Drag mask image to "Mask Sprite" field
   - Repeat for Level2 and Level3

---

## 🎮 Testing the Game

### In Unity Editor
1. Click Play button
2. Game should start with Level 1 loaded
3. Click "Shuffle" to scramble puzzle
4. Click tiles to move them
5. Solve puzzle to see win screen

### Controls
- **Shuffle**: Randomize puzzle
- **Reset**: Return to solved state
- **Next Level**: After winning, load next level

---

## 🔧 Customization

### Adjust Grid Size
In LevelData asset:
- Grid Size: 3-5 (3x3 to 5x5)

### Adjust Difficulty
In LevelData asset:
- Shuffle Moves: Higher = harder (10-100)

### Change Colors
In LevelData asset:
- Tile Background Color: Individual tile tint
- Grid Background Color: Background of puzzle area

### Tile Size
In PuzzleManager:
- Tile Size: Width/height in pixels (default 100)
- Tile Spacing: Gap between tiles (default 2)

---

## 🐛 Common Issues & Solutions

### Issue: Tiles don't show image pieces
**Solution**: 
- Check "Read/Write Enabled" on sprite import settings
- Ensure mask sprite is assigned to LevelData
- Verify CreateTileSprite function is working

### Issue: Shuffle creates unsolvable puzzle
**Solution**: 
- The shuffle uses only valid moves, so it's always solvable
- If stuck, increase shuffle moves in LevelData

### Issue: Win condition not triggering
**Solution**: 
- Ensure all tiles check IsInCorrectPosition()
- Verify empty space is at (gridSize-1, gridSize-1)

### Issue: Tiles don't move smoothly
**Solution**: 
- Check Tile.cs Update() method is running
- Adjust moveSpeed variable in Tile.cs (default 10)

---

## 📱 Building for Different Platforms

### PC/Mac/Linux
1. File → Build Settings
2. Add current scene
3. Select platform
4. Click "Build"

### Mobile (Android/iOS)
1. File → Build Settings → Switch Platform
2. Player Settings:
   - Orientation: Portrait or Landscape
   - Touch controls already supported
3. Build

---

## 🎯 Next Steps & Enhancements

### Optional Features to Add:
1. **Sound Effects**
   - Tile move sound
   - Win celebration sound
   - Background music

2. **Particle Effects**
   - Confetti on win
   - Sparkles when puzzle completes

3. **Timer System**
   - Track completion time
   - High scores per level

4. **Hint System**
   - Show correct next move
   - Limited hints per level

5. **Save System**
   - Save level progress
   - PlayerPrefs for simple save/load

6. **More Levels**
   - Create additional LevelData assets
   - Add to LevelManager levels list

---

## 📝 Code Architecture

### Tile.cs
- Individual tile behavior
- Handles movement animation
- Stores position data

### PuzzleManager.cs
- Core game logic
- Grid initialization
- Win detection
- Tile sprite creation

### LevelManager.cs
- Level loading
- Progression system
- Level selection UI

### LevelData.cs
- ScriptableObject for level config
- Easy to create new levels
- Designer-friendly

### UIManager.cs
- Button handlers
- Panel management
- Game flow control

---

## 🆘 Support & Resources

### Unity Documentation
- [Unity UI System](https://docs.unity3d.com/Manual/UISystem.html)
- [ScriptableObjects](https://docs.unity3d.com/Manual/class-ScriptableObject.html)
- [Sprite Slicing](https://docs.unity3d.com/Manual/9SliceSprites.html)

### Debugging
1. Enable console: Window → General → Console
2. Check for errors (red messages)
3. Use Debug.Log() to trace execution
4. Add breakpoints in VS Code

---

## ✅ Checklist Before Playing

- [ ] All scripts compiled without errors
- [ ] TilePrefab created with Button + Tile components
- [ ] 3 LevelData assets created and configured
- [ ] Mask sprites assigned to each level
- [ ] UI panels created and connected
- [ ] PuzzleManager references all set
- [ ] LevelManager has all 3 levels assigned
- [ ] Scene saved

---

## 📄 License
Free to use and modify for your projects.

## 🎉 Have Fun!
Your mask puzzle game is ready! Click Shuffle and start playing!
