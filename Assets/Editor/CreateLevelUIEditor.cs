    using UnityEngine;
    using UnityEditor;
    using UnityEngine.UI;
    using TMPro;
    using UnityEditor.Events;
    using System.Collections.Generic;

    public class CreateLevelUIEditor : EditorWindow
    {
        private Transform content;
        private GameObject levelUIPrefab;

        private static readonly string[] levelNames = new string[]
        {
            "Abandoned Swamp Village",
            "Old Windmill Path",
            "Moonlit Deserted Village",
            "Vampire Hunter House",
            "Riverside Guard Bridge",
            "Foggy Border Outpost",
            "Forest Trap Crossroad",
            "Deserted Night Fair",
            "Sheep Pen Yard",
            "Lonely Watchtower Hill",
            "Castle Inner Courtyard",
            "Ancestral Portrait Hall",
            "Dusty Wine Cellar",
            "Lantern Statue Garden",
            "Grand Chandelier Stairs",
            "Servants Attic Rooms",
            "Knight Armor Gallery",
            "Hidden Fireplace Passage",
            "Hedge Maze Garden",
            "Trophy Hunting Hall",
            "Abandoned Hill Chapel",
            "Rainy Old Graveyard",
            "Monastery Iron Gates",
            "Cracked Bell Tower",
            "Candle Bone Catacombs",
            "Broken Stone Crypt",
            "Stained Glass Chapel",
            "Underground Tomb Corridor",
            "Monastery Cross Garden",
            "Forgotten Burial Cave"
        };

        [MenuItem("Tools/Create Level UI List")]
        public static void ShowWindow()
        {
            GetWindow<CreateLevelUIEditor>("Level UI Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Level UI Generator", EditorStyles.boldLabel);

            content = (Transform)EditorGUILayout.ObjectField("Content Parent", content, typeof(Transform), true);
            levelUIPrefab = (GameObject)EditorGUILayout.ObjectField("Level UI Prefab", levelUIPrefab, typeof(GameObject), false);

            if (GUILayout.Button("Generate Level UI"))
            {
                if (content == null || levelUIPrefab == null)
                {
                    Debug.LogError("Content or Level UI Prefab is not assigned.");
                    return;
                }

                GenerateLevels();
            }
        }

        private void GenerateLevels()
        {
            Undo.RegisterFullObjectHierarchyUndo(content.gameObject, "Generate Level UI");

            // Очистити старі об'єкти
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(content.GetChild(i).gameObject);
            }

            // Пул для рандомних сцен
            List<string> randomPool = new List<string> { "Level1", "Level2", "Level3", "Level4", "Level5" };

            // Знайти SceneLoader в сцені
            var sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
            if (sceneLoader == null)
            {
                Debug.LogError("SceneLoader not found in scene!");
                return;
            }

            for (int i = 0; i < levelNames.Length; i++)
            {
                GameObject newLevel = (GameObject)PrefabUtility.InstantiatePrefab(levelUIPrefab, content);

                newLevel.name = $"Level_{i + 1}_{levelNames[i]}";

                // Встановити назву рівня
                var levelText = newLevel.transform.Find("LevelNameText")?.GetComponent<TMP_Text>();
                if (levelText != null)
                {
                    levelText.text = levelNames[i];
                }

                // Задати імʼя сцени
                string levelToLoad = (i < 5) ? $"Level{i + 1}" : randomPool[Random.Range(0, randomPool.Count)];

                // Знайти кнопку та задати її onClick
                var button = newLevel.transform.Find("StartLevelButton")?.GetComponent<Button>();
                if (button != null)
                {
                    int count = button.onClick.GetPersistentEventCount();
                    for (int j = count - 1; j >= 0; j--)
                    {
                        UnityEventTools.RemovePersistentListener(button.onClick, j);
                    }

// Додати новий слухач
                    UnityEventTools.AddStringPersistentListener(button.onClick, sceneLoader.LoadLevelEditor, levelToLoad);

                    EditorUtility.SetDirty(newLevel);
                }
                else
                {
                    Debug.LogWarning($"StartLevelButton not found in prefab instance: {newLevel.name}");
                }
            }

            Debug.Log("✅ Level UI generation complete.");
        }
    }
