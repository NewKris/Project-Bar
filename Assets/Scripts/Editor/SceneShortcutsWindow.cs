using System;
using System.IO;
using System.Linq;
using Runtime.Utility;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Editor {
    public class SceneShortcutsWindow : EditorWindow {
        private static SceneShortcutsWindow Instance;
        private const string SCENE_EXTENSION = ".unity";
        
        [MenuItem("Bar Stuff/Scene Shortcuts")]
        public static void OpenWindow() {
            Instance = GetWindow<SceneShortcutsWindow>("Scene Shortcuts");
        }

        private void CreateGUI() {
            DrawContents();
        }

        private void DrawContents() {
            rootVisualElement.Clear();
            rootVisualElement.Add(CreateToolBar());
            rootVisualElement.Add(CreateButtonList());
        }

        private VisualElement CreateToolBar() {
            VisualElement toolBar = new VisualElement();

            toolBar.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
            toolBar.style.alignItems = Align.FlexEnd;
            toolBar.style.justifyContent = Justify.Center;
            toolBar.SetPadding(5, 5, 5, 5);
            
            toolBar.Add(CreateNavButton("refresh@2x", "Refresh buttons", DrawContents));
            toolBar.Add(CreateNavButton("editicon.sml", "Edit shortcuts", SelectDatabase));
            
            return toolBar;
        }

        private VisualElement CreateNavButton(string icon, string tooltip, Action callback) {
            Button refreshButton = new Button {
                style = {
                    width = 20,
                    height = 20,
                    alignItems = Align.Center,
                    justifyContent = Justify.Center
                },
                tooltip = tooltip
            };
            
            refreshButton.Add(new Image() {
                image = EditorGUIUtility.IconContent(icon).image,
                style = {
                    width = 15,
                    height = 15
                }
            });

            refreshButton.clicked += callback;
            
            return refreshButton;
        }

        private void SelectDatabase() {
            if (TryGetDatabase(out SceneShortcutDatabase database)) {
                Selection.activeObject = database;
            }
        }

        private VisualElement CreateButtonList() {
            VisualElement buttonList = new VisualElement();

            if (TryGetDatabase(out SceneShortcutDatabase database)) {
                foreach (Shortcut dbShortcut in database.shortcuts) {
                    buttonList.Add(CreateSceneButton(dbShortcut.buttonText, dbShortcut.scenes));
                }
            }
            
            return buttonList;
        }

        private VisualElement CreateSceneButton(string text, params string[] targetScenes) {
            Button sceneButton = new Button(() => {
                for (int i = 0; i < targetScenes.Length; i++) {
                    OpenSceneMode mode = i == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive;
                    EditorSceneManager.OpenScene(ToFullPath(targetScenes[i]), mode);
                }
            });
            
            sceneButton.text = text;
            
            return sceneButton;
        }

        private string ToFullPath(string shortName) {
            shortName += SCENE_EXTENSION;
            return EditorBuildSettings.scenes.First(x => string.Equals(Path.GetFileName(x.path), shortName)).path;
        }

        private bool TryGetDatabase(out SceneShortcutDatabase database) {
            string[] dbGuids = AssetDatabase.FindAssets($"t:{nameof(SceneShortcutDatabase)}");
            bool foundAsset = dbGuids.Length > 0;
            database = foundAsset ? GetDatabaseFromGuid(dbGuids.First()) : null;

            return foundAsset;
        }

        private SceneShortcutDatabase GetDatabaseFromGuid(string guid) {
            return AssetDatabase.LoadAssetAtPath<SceneShortcutDatabase>(AssetDatabase.GUIDToAssetPath(guid));
        }
    }
}