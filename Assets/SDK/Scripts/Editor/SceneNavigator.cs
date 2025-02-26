using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Change variables and functions --  as per you game
/// </summary>
public class SceneNavigator : MonoBehaviour
{
    public const string INIT_SCENE_PATH = "Assets/Match 3 Project Pototype/Scenes/Init.unity";
    public const string GAME_SCENE_PATH = "Assets/Match 3 Project Pototype/Scenes/ProtoScene.unity";

    [MenuItem("SNGames/Scenes/OpenScene/Init")]
    private static void NavigateToInitScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(INIT_SCENE_PATH);
        }
    }

    [MenuItem("SNGames/Scenes/OpenScene/GameScene")]
    private static void NavigateToGameScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(GAME_SCENE_PATH);
        }
    }

    [MenuItem("SNGames/Scenes/PlayGame _F12")]
    private static void PlayGame()
    {
        if (EditorApplication.isPlaying)
            return;

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(INIT_SCENE_PATH);
            EditorApplication.isPlaying = true;
        }
    }
}
