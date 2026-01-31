# Quick Reference Card - Sliding Puzzle System

## 🎯 Most Common Operations

### Start a Puzzle (from code)
```csharp
// Get reference to game manager
PuzzleGameManager gameManager = FindObjectOfType<PuzzleGameManager>();

// Start puzzle at a specific world position (e.g., puzzle table position)
gameManager.StartPuzzle(puzzleTableTransform.position);
```

### Check Puzzle Status
```csharp
// Is a puzzle currently active?
bool isActive = gameManager.IsPuzzleActive;

// Which puzzle are we on? (0-indexed)
int currentIndex = gameManager.CurrentPuzzleIndex;

// How many total puzzles?
int total = gameManager.TotalPuzzles;
```

### Auto-Solve for Testing
```csharp
// Enable debug mode in PuzzleManager inspector
// Then press W key during puzzle

// Or call directly:
puzzleManager.OnPuzzleSolved?.Invoke();
```

### Add Multiple Puzzles
```csharp
// In PuzzleGameManager inspector:
// Puzzle Configs → Size: 3
// Element 0: Puzzle_01
// Element 1: Puzzle_02  
// Element 2: Puzzle_03

// They will play in order automatically
```

## 🔌 Events You Can Listen To

### Puzzle Manager Events
```csharp
// Subscribe to tile changes
puzzleManager.OnTilesChanged += (tileGrid) => {
    Debug.Log("Tiles moved!");
};

// Subscribe to puzzle solved
puzzleManager.OnPuzzleSolved += () => {
    Debug.Log("Puzzle complete!");
    // Your custom logic here
};
```

### Game Manager Events (add these yourself)
```csharp
// In PuzzleGameManager.cs, you can add:
public System.Action OnPuzzleStarted;
public System.Action OnPuzzleClosed;
public System.Action OnAllPuzzlesComplete;

// Then invoke them at the right places
```

## 🎨 Common Customizations

### Change Puzzle Difficulty at Runtime
```csharp
// Modify the config before starting
puzzleConfig.gridSize = 4; // Makes it harder (4x4 = 15 tiles)
gameManager.StartPuzzle(position);
```

### Custom Mask Spawn Animation
```csharp
// In MaskRewardSystem.cs, modify AwardMask():
spawnedMask = Instantiate(maskPrefab, spawnPos, Quaternion.identity);

// Add your custom animation
spawnedMask.transform.localScale = Vector3.zero;
LeanTween.scale(spawnedMask, Vector3.one, 1f).setEaseOutBack();
```

### Add Move Counter
```csharp
// In PuzzleManager.cs, add:
private int moveCount = 0;

// In TryMoveTile(), increment:
if (moved) {
    moveCount++;
    Debug.Log($"Moves: {moveCount}");
}
```

### Add Timer
```csharp
// In PuzzleManager.cs, add:
private float startTime;

// In InitializePuzzle():
startTime = Time.time;

// In CheckWinCondition():
if (won) {
    float solveTime = Time.time - startTime;
    Debug.Log($"Solved in {solveTime:F2} seconds!");
}
```

## 🐛 Debug Commands

### In Unity Inspector
Right-click on **PuzzleGameManager** component:
- **Skip Current Puzzle** - Immediately solve current puzzle
- **Reset All Puzzles** - Start over from puzzle 1

### Keyboard Shortcuts (when Debug Mode enabled)
- **W Key** - Auto-solve current puzzle
- **E Key** - Interact with puzzle table (default)

### Console Commands
```csharp
// Get current puzzle state
Debug.Log($"Empty tile at: {puzzleManager.EmptyTilePosition}");
Debug.Log($"Current grid:\n{PrintGrid(puzzleManager.TileGrid)}");

// Helper function to print grid
string PrintGrid(int[,] grid) {
    string result = "";
    for (int y = 0; y < puzzleManager.GridSize; y++) {
        for (int x = 0; x < puzzleManager.GridSize; x++) {
            result += grid[x, y] + " ";
        }
        result += "\n";
    }
    return result;
}
```

## ⚡ Performance Tips

### For Hackathon Speed
1. **Use Debug Mode**: Enable it and press W to skip puzzles during testing
2. **Placeholder Art**: Use simple colored squares for tiles initially
3. **Small Grid**: Start with 3x3, easier to test and solve
4. **Skip Animations**: Add them later if time permits

### Optimize Later
- Cache frequently accessed components
- Object pool the tiles instead of Instantiate/Destroy
- Use sprite atlas for tile images
- Compress textures

## 🎮 Integration Examples

### With First Person Controller
```csharp
// In PuzzleGameManager, reference your FPS controller
public FirstPersonController fpsController;

// When starting puzzle:
fpsController.enabled = false;

// When closing puzzle:
fpsController.enabled = true;
```

### With Third Person Controller
```csharp
public ThirdPersonController tpController;
public CinemachineFreeLook cinemachineCamera;

// Lock controls and camera
tpController.enabled = false;
cinemachineCamera.enabled = false;
```

### With Input System (New)
```csharp
public PlayerInput playerInput;

// Disable action map
playerInput.SwitchCurrentActionMap("UI");

// Re-enable
playerInput.SwitchCurrentActionMap("Player");
```

## 📱 Mobile Adaptation (If Needed)

### Touch Input for Tiles
The Button component works automatically with touch.

### Virtual Joystick Toggle
```csharp
public GameObject virtualJoystick;

// Hide during puzzle
virtualJoystick.SetActive(false);

// Show after
virtualJoystick.SetActive(true);
```

## 🎯 Typical Hackathon Flow

1. ✅ Copy all scripts to project
2. ✅ Create UI hierarchy (10 min)
3. ✅ Create tile prefab (5 min)
4. ✅ Create puzzle config (5 min)
5. ✅ Wire up references (10 min)
6. ✅ Test with Debug Mode (press W) (5 min)
7. ✅ Add your mask models (10 min)
8. ✅ Connect ghost animations (15 min)
9. ✅ Polish and juice (remaining time)

Total setup: ~1 hour
Remaining time: Make it look awesome! 🎨

## 🆘 Emergency Fixes

**Nothing works?**
- Check Console for errors
- Verify all public references are assigned
- Make sure Canvas has EventSystem

**UI not showing?**
- Check Canvas render mode
- Verify PuzzlePanel is child of Canvas
- Check PuzzleUIPanel is assigned

**Can't interact?**
- Player needs "Player" tag
- Increase interaction range
- Check if puzzle already active

**Ghost won't defeat?**
- Assign ghost in Puzzle Config
- Check animator trigger name matches
- Verify animator is on ghost object

Good luck! 🚀
