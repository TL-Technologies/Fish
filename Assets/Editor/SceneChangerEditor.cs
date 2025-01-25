using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneChangerEditor :Editor
{
    [MenuItem("Fish/Open Scenes/1. Main Menu")]
    public static void SplashScreen()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/MyAssets/Scene/Menu.unity");
    }
    
    [MenuItem("Fish/Open Scenes/2. Matching")]
    public static void Matching()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/MyAssets/Scene/Matching.unity");
    }
    
    [MenuItem("Fish/Open Scenes/3. Game")]
    public static void Game()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/MyAssets/Scene/Game.unity");
    }
    
}
