public class TutorialUI : BaseMenuUI {

    private void Start() {
        Open();
        UpdateVisual();

        if (!GameStateAndTimerManager.Instance.IsWaitingToStart()) {
            Close();
        }
    }

    private void UpdateVisual() {
        //! Game name & detailed Description
        //! Also maybe button bindings
    }

    protected override void Close() {
        base.Close();
        GameStateAndTimerManager.Instance.SetLocalPlayerReady();
    }
}
