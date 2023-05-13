using UnityEngine;

[CreateAssetMenu()]
public class GameSceneSO : ScriptableObject {

    public Loader.Scene gameScene;
    public Sprite gamePreview;
    public string gameName;
    [TextArea] public string gameDescription;
}