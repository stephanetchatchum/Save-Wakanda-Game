# VS Code Integration Guide for Unity

## 🔧 Setting Up VS Code with Unity

### Step 1: Install VS Code
1. Download from https://code.visualstudio.com/
2. Install on your system

### Step 2: Install Unity Extension
1. Open VS Code
2. Click Extensions (Ctrl+Shift+X)
3. Search for "Unity" by Tobiah Zarlez
4. Click Install
5. Optionally install:
   - C# extension by Microsoft
   - Unity Code Snippets
   - Unity Tools

### Step 3: Configure Unity to Use VS Code
1. Open Unity
2. Go to: Edit → Preferences (Windows) or Unity → Preferences (Mac)
3. Select "External Tools"
4. External Script Editor → Browse
5. Navigate to VS Code executable:
   - Windows: `C:\Users\[YourName]\AppData\Local\Programs\Microsoft VS Code\Code.exe`
   - Mac: `/Applications/Visual Studio Code.app`
   - Linux: `/usr/bin/code`
6. Click "Regenerate project files"
7. Close and reopen Unity

### Step 4: Open Project in VS Code
**Method 1: From Unity**
- Double-click any C# script in Unity
- VS Code will open automatically

**Method 2: From VS Code**
- Open VS Code
- File → Open Folder
- Navigate to your Unity project root folder
- Click "Select Folder"

---

## 🎯 VS Code Workspace Setup

### Recommended Extensions
```
- C# (Microsoft) - IntelliSense, debugging
- Unity Code Snippets - Code completion
- Unity Tools - Additional Unity support
- Debugger for Unity - Attach debugger
- Bracket Pair Colorizer - Easier code reading
```

### VS Code Settings for Unity
Create `.vscode/settings.json` in your project:

```json
{
  "files.exclude": {
    "**/.git": true,
    "**/.DS_Store": true,
    "**/Library": true,
    "**/Temp": true,
    "**/Obj": true,
    "**/*.csproj": true,
    "**/*.sln": true
  },
  "omnisharp.useModernNet": true
}
```

---

## 📝 Editing Scripts in VS Code

### Opening Scripts
1. **From Unity**: Double-click script → Opens in VS Code
2. **From VS Code**: Navigate file tree on left

### IntelliSense Features
- **Auto-complete**: Type and press Ctrl+Space
- **Parameter hints**: Type method name, see parameters
- **Go to definition**: F12 or Ctrl+Click
- **Find references**: Shift+F12
- **Rename symbol**: F2

### Useful Shortcuts
```
Ctrl+Space         - IntelliSense suggestions
Ctrl+.             - Quick actions (add using, etc.)
Ctrl+K Ctrl+D      - Format document
Ctrl+/             - Toggle comment
Ctrl+Shift+F       - Find in all files
F12                - Go to definition
Alt+↑/↓            - Move line up/down
Ctrl+D             - Select next occurrence
```

---

## 🐛 Debugging in VS Code

### Setup Debugger
1. Install "Debugger for Unity" extension
2. In VS Code: Run → Add Configuration
3. Select "Unity Debugger"
4. Creates `.vscode/launch.json`

### Debug Session
1. Set breakpoints (click left margin of line numbers)
2. In Unity: Edit → Preferences → External Tools
3. Check "Editor Attaching"
4. In VS Code: Run → Start Debugging (F5)
5. Select "Attach to Unity"
6. Play your game in Unity
7. Code pauses at breakpoints

### Debug Actions
```
F5       - Start/Continue
F10      - Step Over
F11      - Step Into
Shift+F11 - Step Out
Shift+F5  - Stop Debugging
```

---

## 🔄 Workflow Best Practices

### Typical Workflow
1. **Edit in VS Code**
   - Make code changes
   - Save files (Ctrl+S)

2. **Test in Unity**
   - Unity auto-detects changes
   - Wait for compilation (bottom-right spinner)
   - Click Play to test

3. **Iterate**
   - Stop Play mode
   - Return to VS Code
   - Make more changes
   - Repeat

### Multi-File Editing
- Ctrl+P: Quick file search
- Ctrl+Tab: Switch between open files
- Split editor: Ctrl+\
- Side-by-side editing: Drag tabs

---

## 📂 Project Structure in VS Code

```
YourUnityProject/
├── .vscode/              (VS Code settings)
│   ├── settings.json
│   └── launch.json
├── Assets/
│   ├── Scripts/          ← Your C# files here
│   │   ├── Tile.cs
│   │   ├── PuzzleManager.cs
│   │   └── ...
│   ├── Prefabs/
│   ├── Sprites/
│   └── Scenes/
├── Library/              (Unity cache - hidden)
├── Packages/             (Unity packages)
└── ProjectSettings/      (Unity settings)
```

---

## 💡 Tips for Unity + VS Code

### 1. Fix IntelliSense Issues
If auto-complete stops working:
```
1. Close VS Code
2. In Unity: Edit → Preferences → External Tools
3. Click "Regenerate project files"
4. Reopen VS Code
5. Reload window: Ctrl+Shift+P → "Reload Window"
```

### 2. Using Statements
VS Code can auto-add using statements:
```csharp
// Type "GameObject" without using statement
GameObject obj; // Red underline

// Press Ctrl+. on the underline
// Select "using UnityEngine;"
```

### 3. Code Snippets
Type these and press Tab:
```
mbox    → MonoBehaviour class
prop    → Property with get/set
ctor    → Constructor
```

### 4. Format on Save
Add to settings.json:
```json
{
  "editor.formatOnSave": true
}
```

### 5. File Watching
If changes don't reflect in Unity:
- Check file is saved (no dot on tab)
- Check Unity console for compilation errors
- Verify file is in Assets folder

---

## 🚨 Common Issues & Solutions

### Issue: VS Code doesn't open from Unity
**Solution**:
1. Unity → Edit → Preferences → External Tools
2. Reselect VS Code executable
3. Regenerate project files

### Issue: No IntelliSense
**Solution**:
1. Install C# extension
2. Restart VS Code
3. Unity → Regenerate project files
4. Open any .cs file
5. Check bottom bar says "OmniSharp"

### Issue: Can't find Unity methods (Start, Update)
**Solution**:
- Ensure script inherits from MonoBehaviour
- Add `using UnityEngine;` at top
- Regenerate project files

### Issue: Changes don't appear in Unity
**Solution**:
1. Save file in VS Code (Ctrl+S)
2. Check Unity console for errors
3. Wait for compilation to finish
4. Stop and restart Play mode

---

## 🎓 Learning Resources

### Documentation
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)
- [VS Code Unity Setup](https://code.visualstudio.com/docs/other/unity)
- [C# Language Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)

### Keyboard Shortcuts Cheat Sheet
```
General:
Ctrl+P          - Quick file open
Ctrl+Shift+P    - Command palette
Ctrl+B          - Toggle sidebar

Editing:
Ctrl+X          - Cut line
Ctrl+C          - Copy line
Ctrl+V          - Paste
Ctrl+Z          - Undo
Ctrl+Y          - Redo
Ctrl+F          - Find
Ctrl+H          - Replace

Multi-cursor:
Alt+Click       - Add cursor
Ctrl+Alt+↑/↓    - Add cursor above/below
Ctrl+D          - Select next match
```

---

## ✅ Quick Start Checklist

- [ ] VS Code installed
- [ ] Unity extension installed
- [ ] C# extension installed
- [ ] Unity configured to use VS Code
- [ ] Project files regenerated
- [ ] Can open scripts from Unity
- [ ] IntelliSense working
- [ ] Auto-save enabled

---

## 🎯 Your Mask Puzzle Files

When editing your puzzle game:
```
Focus on these files:
├── Tile.cs              - Tile behavior
├── PuzzleManager.cs     - Main game logic
├── LevelManager.cs      - Level system
├── LevelData.cs         - Level config
└── UIManager.cs         - UI controls
```

### Quick Edit Example
1. Open `PuzzleManager.cs` in VS Code
2. Find `moveSpeed = 10f;` in Tile.cs
3. Change to `20f` for faster animation
4. Save (Ctrl+S)
5. Return to Unity
6. Click Play to test

---

Happy Coding! 🚀
