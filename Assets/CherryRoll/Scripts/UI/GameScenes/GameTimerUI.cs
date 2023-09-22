using UnityEngine;

public class GameTimerUI : MonoBehaviour {

    [SerializeField] private RectTransform backImage;
    [SerializeField] private RectTransform fillImage;

    private float fullTimerLength;


    private void Start() {
        fullTimerLength = backImage.rect.width;
    }

    private void Update() {
        fillImage.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            fullTimerLength * (1 - GameStateAndTimer.Instance.GetGamePlayingTimerNormalized())
        );
    }

    private void OnRectTransformDimensionsChange() { 
        fullTimerLength = backImage.rect.width;
    }
}
