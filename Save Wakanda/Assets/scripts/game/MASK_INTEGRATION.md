# Integrating Your Custom Mask Images

## 📸 Preparing Your Mask Images

### Image Specifications
- **Format**: PNG (with transparency preferred)
- **Dimensions**: Square aspect ratio (e.g., 512x512, 1024x1024, 2048x2048)
- **Content**: Any mask design - venetian, tribal, animal, fantasy, etc.
- **Quality**: High resolution for better tile detail

### Example Mask Types
1. **Level 1** - Simple design (3x3 grid works best with simpler patterns)
2. **Level 2** - Moderate detail (4x4 grid)
3. **Level 3** - Complex design (5x5 grid shows intricate details)

---

## 🎨 Adding Masks to Unity

### Step-by-Step Guide

#### 1. Import Your Images
```
1. Create folder: Assets/Sprites/Masks
2. Drag your 3 mask images into this folder:
   - mask_level1.png
   - mask_level2.png
   - mask_level3.png
```

#### 2. Configure Import Settings
For EACH image:
```
1. Select image in Project window
2. In Inspector, change settings:
   
   Texture Type: Sprite (2D and UI)
   Sprite Mode: Single
   Pixels Per Unit: 100
   Filter Mode: Bilinear
   Compression: None (or High Quality)
   
   ⚠️ CRITICAL SETTING:
   Advanced → Read/Write Enabled: ✅ CHECK THIS
   
3. Click "Apply"
```

**Why Read/Write is Critical:**
The `CreateTileSprite()` function in PuzzleManager needs to read pixel data from your mask image to create individual tile sprites. Without Read/Write enabled, Unity can't access the texture data, and tiles will be blank.

#### 3. Assign to Level Data
```
1. Go to: Assets/Levels/
2. Select Level1.asset
3. In Inspector:
   - Drag mask_level1.png to "Mask Sprite" field
4. Repeat for Level2 and Level3
```

---

## 🔧 Advanced: Programmatic Mask Assignment

If you want to load masks dynamically or from Resources:

```csharp
using UnityEngine;

public class MaskLoader : MonoBehaviour
{
    void Start()
    {
        // Load mask from Resources folder
        Sprite maskSprite = Resources.Load<Sprite>("Masks/mask_level1");
        
        // Assign to level
        LevelData level = GetLevelData();
        level.maskSprite = maskSprite;
    }
}
```

---

## 🎭 Mask Design Tips

### For 3x3 Grid (Level 1)
- **Bold, simple shapes**
- **High contrast colors**
- **Minimal fine details** (they get lost in 9 tiles)
- **Centered focal point**

Examples:
- Simple venetian mask with large eye holes
- Tribal mask with bold patterns
- Animal face (cat, fox, owl) with clear features

### For 4x4 Grid (Level 2)
- **Moderate detail level**
- **Distinct color zones**
- **Some decorative elements**
- **Clear symmetry or asymmetry**

Examples:
- Ornate carnival mask with feathers
- Japanese oni mask with details
- Decorative theater mask

### For 5x5 Grid (Level 3)
- **Intricate patterns**
- **Fine details visible**
- **Complex color gradients**
- **Multiple decorative elements**

Examples:
- Venetian masquerade mask with jewels
- Intricate tribal war mask
- Fantasy/mythical creature mask

---

## 🖼️ Example Mask Creation Workflow

### Using Photoshop/GIMP
```
1. Create new document: 1024x1024px
2. Draw/design your mask
3. Use layers for different elements
4. Export as PNG with transparency
5. Save to Assets/Sprites/Masks/
```

### Using AI Art Generators
```
Prompts to try:
- "ornate venetian carnival mask, frontal view, detailed, transparent background"
- "tribal mask with geometric patterns, centered, PNG"
- "fantasy fox mask, decorative, gold and red, transparent background"

After generation:
1. Remove background if needed
2. Resize to square (1024x1024)
3. Export as PNG
```

### Using Online Mask Resources
Free resources:
- Freepik (search "mask PNG")
- Pexels (high-res mask images)
- Unsplash (mask photography)

Remember to:
- Check licenses (use only royalty-free)
- Remove backgrounds
- Ensure square aspect ratio

---

## 🎨 Color Palette Suggestions

### Classic Venetian
```
Gold: #FFD700
Deep Red: #8B0000
Black: #000000
White: #FFFFFF
```

### Tribal Earth Tones
```
Burnt Orange: #CC5500
Deep Brown: #5C4033
Cream: #FFFDD0
Charcoal: #36454F
```

### Fantasy Magical
```
Purple: #9B59B6
Gold: #F39C12
Teal: #16A085
Silver: #BDC3C7
```

### Nature Inspired
```
Forest Green: #228B22
Wooden Brown: #8B4513
Leaf Green: #90EE90
Earth Brown: #654321
```

---

## 🔍 Testing Your Masks

### Visual Quality Check
After importing:
```
1. Select your sprite in Project window
2. Right-click → Show in Explorer/Finder
3. Verify original quality
4. In Unity, check preview in Inspector
```

### In-Game Test
```
1. Assign mask to Level1
2. Play game
3. Check if:
   - Image is clear in tiles
   - Colors are vibrant
   - Details are visible
   - No artifacts or blurring
```

### Troubleshooting Poor Quality
If tiles look blurry:
- Increase source image resolution
- Change compression to "None"
- Increase Pixels Per Unit
- Check mipmap settings

---

## 📋 Quick Checklist

Before using your mask in-game:
- [ ] Image is square (width = height)
- [ ] Format is PNG
- [ ] Resolution is at least 512x512
- [ ] Read/Write Enabled is checked
- [ ] Texture Type is "Sprite (2D and UI)"
- [ ] Image looks good in Unity preview
- [ ] Assigned to appropriate LevelData asset

---

## 🎯 Example: Complete Workflow

Let's say you received 3 mask images from a designer:

### Step 1: Prepare Files
```
Received:
- venetian_mask.jpg (2000x1500) ❌ Not square
- tribal_mask.png (1024x1024) ✅ Perfect
- animal_mask.png (800x600) ❌ Not square

Actions:
1. Open venetian_mask.jpg in image editor
2. Crop to 1500x1500, center the mask
3. Export as PNG
4. Repeat for animal_mask
```

### Step 2: Import to Unity
```
1. Drag all 3 PNGs into: Assets/Sprites/Masks/
2. Select all 3 images
3. In Inspector:
   - Texture Type: Sprite (2D and UI)
   - Read/Write Enabled: ✅
4. Click Apply
```

### Step 3: Assign to Levels
```
Level1.asset → venetian_mask
Level2.asset → tribal_mask  
Level3.asset → animal_mask
```

### Step 4: Test
```
1. Play game
2. Check each level
3. Verify tiles show correct image portions
4. Solve puzzle, verify complete image looks correct
```

Done! 🎉

---

## 💡 Pro Tips

1. **Keep Originals**: Save your original high-res files outside Unity
2. **Version Control**: Name files descriptively (mask_venetian_gold_v1.png)
3. **Test Early**: Import and test masks before creating all levels
4. **Backup**: Keep a backup folder of all mask images
5. **Consistency**: Use similar color palettes across all levels for visual cohesion

---

## 🆘 Common Issues

### Issue: Tiles are completely black
**Cause**: Read/Write not enabled
**Fix**: Select sprite → Inspector → Read/Write Enabled ✅ → Apply

### Issue: Tiles show wrong image sections
**Cause**: Image is not square
**Fix**: Re-crop image to square dimensions

### Issue: Image is pixelated
**Cause**: Low resolution or high compression
**Fix**: Use higher res image (1024x1024+) and compression = None

### Issue: Transparency looks wrong
**Cause**: JPG format doesn't support transparency
**Fix**: Use PNG format

---

Your masks are now ready to use! 🎭
