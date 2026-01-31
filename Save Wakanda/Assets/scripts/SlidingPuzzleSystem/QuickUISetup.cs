using UnityEngine;
using UnityEngine.UI;

namespace SlidingPuzzle
{
    /// <summary>
    /// Quick helper script to auto-create basic UI elements
    /// Attach to Canvas and press the context menu option to generate UI
    /// Use this for rapid prototyping during hackathon!
    /// </summary>
    public class QuickUISetup : MonoBehaviour
    {
        [Header("Auto-Generated References")]
        public GameObject puzzlePanel;
        public GameObject ghostCounterPanel;
        public Text movesText;
        public Text timerText;
        public Text ghostCountText;
        
        [ContextMenu("Auto-Create Puzzle UI")]
        public void AutoCreatePuzzleUI()
        {
            Canvas canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("This script must be on a Canvas!");
                return;
            }
            
            // Create Puzzle Panel
            if (puzzlePanel == null)
            {
                puzzlePanel = new GameObject("PuzzlePanel");
                puzzlePanel.transform.SetParent(transform);
                
                RectTransform rect = puzzlePanel.AddComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.sizeDelta = Vector2.zero;
                
                Image bg = puzzlePanel.AddComponent<Image>();
                bg.color = new Color(0, 0, 0, 0.8f);
                
                CanvasGroup cg = puzzlePanel.AddComponent<CanvasGroup>();
                
                puzzlePanel.SetActive(false);
            }
            
            // Create Tile Container
            GameObject tileContainer = new GameObject("TileContainer");
            tileContainer.transform.SetParent(puzzlePanel.transform);
            
            RectTransform tileRect = tileContainer.AddComponent<RectTransform>();
            tileRect.anchorMin = new Vector2(0.5f, 0.5f);
            tileRect.anchorMax = new Vector2(0.5f, 0.5f);
            tileRect.pivot = new Vector2(0, 1);
            tileRect.sizeDelta = new Vector2(500, 500);
            tileRect.anchoredPosition = new Vector2(-250, 250);
            
            // Create Stats Panel
            GameObject statsPanel = new GameObject("StatsPanel");
            statsPanel.transform.SetParent(puzzlePanel.transform);
            
            RectTransform statsRect = statsPanel.AddComponent<RectTransform>();
            statsRect.anchorMin = new Vector2(0.5f, 1f);
            statsRect.anchorMax = new Vector2(0.5f, 1f);
            statsRect.pivot = new Vector2(0.5f, 1f);
            statsRect.sizeDelta = new Vector2(500, 80);
            statsRect.anchoredPosition = new Vector2(0, -20);
            
            // Create Moves Text
            GameObject movesObj = new GameObject("MovesText");
            movesObj.transform.SetParent(statsPanel.transform);
            
            RectTransform movesRect = movesObj.AddComponent<RectTransform>();
            movesRect.anchorMin = new Vector2(0, 0.5f);
            movesRect.anchorMax = new Vector2(0.5f, 0.5f);
            movesRect.sizeDelta = Vector2.zero;
            movesRect.anchoredPosition = Vector2.zero;
            
            movesText = movesObj.AddComponent<Text>();
            movesText.text = "Moves: 0";
            movesText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            movesText.fontSize = 24;
            movesText.alignment = TextAnchor.MiddleLeft;
            movesText.color = Color.white;
            
            // Create Timer Text
            GameObject timerObj = new GameObject("TimerText");
            timerObj.transform.SetParent(statsPanel.transform);
            
            RectTransform timerRect = timerObj.AddComponent<RectTransform>();
            timerRect.anchorMin = new Vector2(0.5f, 0.5f);
            timerRect.anchorMax = new Vector2(1f, 0.5f);
            timerRect.sizeDelta = Vector2.zero;
            timerRect.anchoredPosition = Vector2.zero;
            
            timerText = timerObj.AddComponent<Text>();
            timerText.text = "Time: 00:00";
            timerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            timerText.fontSize = 24;
            timerText.alignment = TextAnchor.MiddleRight;
            timerText.color = Color.white;
            
            // Create Ghost Counter Panel
            if (ghostCounterPanel == null)
            {
                ghostCounterPanel = new GameObject("GhostCounterPanel");
                ghostCounterPanel.transform.SetParent(transform);
                
                RectTransform ghostRect = ghostCounterPanel.AddComponent<RectTransform>();
                ghostRect.anchorMin = new Vector2(0.5f, 1f);
                ghostRect.anchorMax = new Vector2(0.5f, 1f);
                ghostRect.pivot = new Vector2(0.5f, 1f);
                ghostRect.sizeDelta = new Vector2(400, 60);
                ghostRect.anchoredPosition = new Vector2(0, -20);
                
                Image ghostBg = ghostCounterPanel.AddComponent<Image>();
                ghostBg.color = new Color(0, 0, 0, 0.7f);
                
                GhostCounter counter = ghostCounterPanel.AddComponent<GhostCounter>();
                counter.totalGhosts = 3;
                
                // Create Ghost Count Text
                GameObject ghostTextObj = new GameObject("GhostCountText");
                ghostTextObj.transform.SetParent(ghostCounterPanel.transform);
                
                RectTransform ghostTextRect = ghostTextObj.AddComponent<RectTransform>();
                ghostTextRect.anchorMin = Vector2.zero;
                ghostTextRect.anchorMax = Vector2.one;
                ghostTextRect.sizeDelta = Vector2.zero;
                
                ghostCountText = ghostTextObj.AddComponent<Text>();
                ghostCountText.text = "Ghosts Defeated: 0/3";
                ghostCountText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                ghostCountText.fontSize = 28;
                ghostCountText.alignment = TextAnchor.MiddleCenter;
                ghostCountText.color = Color.white;
                
                counter.ghostCountText = ghostCountText;
            }
            
            Debug.Log("✅ UI Auto-Created! Now:");
            Debug.Log("1. Create a tile prefab (Button with Image and Text)");
            Debug.Log("2. Assign references in PuzzleUIController");
            Debug.Log("3. Create puzzle configurations");
            
            puzzlePanel.SetActive(true); // Show it so you can see it
        }
    }
}
