# Sliding Puzzle System for Unity

A complete, modular sliding puzzle game system for Unity with hybrid 3D/2D gameplay. Perfect for game jams and hackathons!

## 📦 What's Included

### Core Scripts (7 files)

1. **PuzzleConfiguration.cs** - ScriptableObject for easy puzzle setup
   - Configure grid size, images, masks, and ghost settings
   - Create via: Right-click → Create → Sliding Puzzle → Puzzle Config

2. **PuzzleManager.cs** - Core sliding puzzle logic
   - Handles tile movement, shuffling, win detection
   - Debug mode with auto-solve (press W)
   - Event system for extensibility

3. **PuzzleUIController.cs** - UI display and interaction
   - Creates tile grid dynamically
   - Handles image slicing for tiles
   - Touch/mouse input support

4. **PuzzleInteractable.cs** - 3D world interaction
   - Attach to puzzle tables or objects
   - Proximity detection
   - Customizable interaction range and keys

5. **MaskRewardSystem.cs** - Reward and ghost defeat handler
   - Spawns masks on puzzle completion
   - Triggers ghost defeat animations
   - Configurable spawn positions and delays

6. **PuzzleGameManager.cs** - Main orchestrator
   - Manages multiple puzzles
   - Handles player state (lock/unlock)
   - Camera switching
   - Progression tracking

7. **PlayerControlsManager.cs** (Optional) - Helper for player controls
   - Disable multiple components at once
   - Rigidbody freezing
   - State restoration

### Documentation (2 files)

- **SETUP_GUIDE.md** - Complete step-by-step setup instructions
- **QUICK_REFERENCE.md** - Common operations and customizations

## ✨ Features

- ✅ Hybrid 3D/2D gameplay
- ✅ Multiple puzzle support with progression
- ✅ Configurable difficulty (2x2 to 5x5 grids)
- ✅ Image-based puzzles
- ✅ Automatic mask spawning on completion
- ✅ Ghost defeat sequence with animations
- ✅ Debug mode for rapid testing
- ✅ Player control locking during puzzles
- ✅ Optional camera switching
- ✅ Event-driven architecture
- ✅ Inspector-friendly (drag & drop)
- ✅ Low poly optimized

## 🚀 Quick Start

1. Copy all `.cs` files to `Assets/Scripts/SlidingPuzzle/`
2. Follow **SETUP_GUIDE.md** (5 steps, ~30 minutes)
3. Create a puzzle configuration
4. Wire up references in inspector
5. Test with debug mode (press W to solve)

## 🎮 How It Works

### Gameplay Flow
1. Player approaches puzzle table in 3D world
2. Press E to interact
3. 2D puzzle UI appears
4. Solve sliding puzzle
5. Mask spawns in 3D world
6. Ghost plays defeat animation
7. Return to 3D gameplay

### System Architecture
```
PuzzleGameManager (Orchestrator)
├── PuzzleManager (Logic)
│   └── Events: OnTilesChanged, OnPuzzleSolved
├── PuzzleUIController (Display)
│   └── Dynamic tile creation & rendering
├── MaskRewardSystem (Rewards)
│   └── Mask spawn + Ghost defeat
└── PuzzleInteractable (3D Trigger)
    └── Proximity detection
```

## 🛠️ Customization

### Easy
- Change grid size (3x3, 4x4, 5x5)
- Swap puzzle images
- Adjust spawn positions
- Modify interaction keys

### Medium
- Add sound effects
- Particle effects
- Custom animations
- Move counter/timer

### Advanced
- Hint system
- Multiple difficulty modes
- Save/load progress
- Online leaderboards

## 📋 Requirements

- Unity 2021.3+ (should work on earlier versions too)
- TextMeshPro (optional, for better text)
- Animator on ghost object (for defeat animation)

## 🎯 Perfect For

- ✅ Game jams (quick setup)
- ✅ Hackathons (modular design)
- ✅ Educational projects
- ✅ Puzzle game prototypes
- ✅ Adventure game minigames

## 📝 Configuration Example

```csharp
// Create a new Puzzle Configuration asset
// Assign in inspector:
- Puzzle Image: your_texture.png
- Grid Size: 3 (for 3x3 puzzle)
- Mask Prefab: YourMaskModel
- Mask Spawn Offset: (0, 1.5, 2)
- Ghost Object: Assigned at runtime
- Defeat Animation Trigger: "Defeat"
- Ghost Destroy Delay: 3.0
```

## 🐛 Debug Features

- **Auto-solve**: Press W during puzzle (when debug mode enabled)
- **Skip puzzle**: Right-click PuzzleGameManager → Skip Current Puzzle
- **Reset**: Right-click PuzzleGameManager → Reset All Puzzles
- **Console logging**: Detailed state information

## 🤝 Support

Check the documentation files:
- Issues with setup? → **SETUP_GUIDE.md**
- Need code examples? → **QUICK_REFERENCE.md**
- Troubleshooting? → Both files have sections

## 📄 License

Free to use for any project (commercial or non-commercial).
No attribution required, but appreciated! 😊

---

**Built for hackathons, optimized for fun!** 🎮✨

Need help? All scripts have extensive comments and tooltips.
