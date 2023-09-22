using Unity.Netcode;
using UnityEngine.SceneManagement;

public static class Loader {

    public enum Scene {
        MenuMainMenuScene,
        MenuLoadingScene,
        MenuMultiplayerScene,
        MenuCharacterSetupScene,
        GameLobbyScene,
        GameBigBunScene,
        GameMagicTableclothScene,
        GameCollectThePlateScene,
    }

    private static Scene targetScene;


    public static void Load(Scene targetScene) {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(targetScene.ToString());
    }

    public static void LoadNetwork(Scene targetScene) {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
