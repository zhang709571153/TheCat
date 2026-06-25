using System.Collections.Generic;
using System.IO;
using TheCat.Gameplay;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.EditorTools
{
    public static class P0CatRoomSceneBuilder
    {
        private const string SceneFolder = "Assets/TheCat/Scenes";
        private const string ScenePath = SceneFolder + "/P0CatRoom.unity";
        private const string RootName = "P0CatRoomRoot";

        [MenuItem("TheCat/P0/Create P0 Cat Room Scene", false, 88)]
        public static void CreateP0CatRoomScene()
        {
            EnsureScene();
            EnsureBuildSettings();
            AssetDatabase.SaveAssets();
            Debug.Log("[TheCat] P0 cat room scene is ready: " + ScenePath);
        }

        public static void CreateP0CatRoomSceneForBatchmode()
        {
            CreateP0CatRoomScene();
        }

        private static void EnsureScene()
        {
            Directory.CreateDirectory(SceneFolder);
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject root = new GameObject(RootName);
            root.AddComponent<CatRoomController>();

            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.08f, 0.08f, 0.1f, 1f);
            camera.orthographic = true;
            camera.orthographicSize = 5f;

            EditorSceneManager.SaveScene(scene, ScenePath);
        }

        private static void EnsureBuildSettings()
        {
            string[] p0Paths =
            {
                "Assets/TheCat/Scenes/" + P0SceneFlow.MainMenuSceneName + ".unity",
                ScenePath,
                "Assets/TheCat/Scenes/" + P0SceneFlow.RouteMapSceneName + ".unity",
                "Assets/TheCat/Scenes/" + P0SceneFlow.GrayboxBattleSceneName + ".unity"
            };

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            HashSet<string> p0PathSet = new HashSet<string>(p0Paths);
            for (int i = 0; i < p0Paths.Length; i++)
            {
                scenes.Add(new EditorBuildSettingsScene(p0Paths[i], true));
            }

            EditorBuildSettingsScene[] existing = EditorBuildSettings.scenes;
            for (int i = 0; i < existing.Length; i++)
            {
                if (!p0PathSet.Contains(existing[i].path))
                {
                    scenes.Add(existing[i]);
                }
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
