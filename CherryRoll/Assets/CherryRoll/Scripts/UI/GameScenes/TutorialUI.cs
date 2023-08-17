public class TutorialUI : BaseMenuUI {

    private void Start() {
        Open();
        UpdateVisual();

        if (!GameStateAndTimer.Instance.IsWaitingToStart()) {
            Close();
        }
    }

    private void UpdateVisual() {
        //! Game name & detailed Description
        //! Also maybe button bindings
    }

    protected override void Close() {
        base.Close();
        GameStateAndTimer.Instance.SetLocalPlayerReady();
    }
}
