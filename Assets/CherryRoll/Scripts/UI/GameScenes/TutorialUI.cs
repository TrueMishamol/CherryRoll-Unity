public class TutorialUI : BaseMenuUI {

    private void Start() {
        Open();

        if (!GameStateAndTimer.Instance.IsWaitingToStart()) {
            Close();
        }
    }

    protected override void Close() {
        base.Close();
        GameStateAndTimer.Instance.SetLocalPlayerReady();
    }
}
