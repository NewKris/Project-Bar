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
        
        [MenuItem("Window/Scene Shortcuts")]
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
            
            toolBar.Add(CreateRefreshButton());
            
            return toolBar;
        }

        private VisualElement CreateRefreshButton() {
            Button refreshButton = new Button {
                style = {
                    width = 20,
                    height = 20,
                    alignItems = Align.Center,
                    justifyContent = Justify.Center
                }
            };
            
            refreshButton.SetPadding(0, 0, 0, 0);

            refreshButton.Add(new Image() {
                image = EditorGUIUtility.IconContent("refresh@2x").image,
                style = {
                    width = 15,
                    height = 15
                }
            });

            refreshButton.clicked += DrawContents;
            
            return refreshButton;
        }

        private VisualElement CreateButtonList() {
            VisualElement buttonList = new VisualElement();

            string[] dbGuids = AssetDatabase.FindAssets($"t:{nameof(SceneShortcutDatabase)}");

            if (dbGuids.Length > 0) {
                SceneShortcutDatabase db = AssetDatabase.LoadAssetAtPath<SceneShortcutDatabase>(AssetDatabase.GUIDToAssetPath(dbGuids.First()));
            
                foreach (Shortcut dbShortcut in db.shortcuts) {
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
    }
}