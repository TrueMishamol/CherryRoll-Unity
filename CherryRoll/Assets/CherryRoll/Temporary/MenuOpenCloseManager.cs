using System;
using UnityEngine;

public class MenuOpenCloseManager : MonoBehaviour {

    //public static MenuOpenCloseManager Instance { get; private set; }

    //public event EventHandler OnGamePaused;
    //public event EventHandler OnGameUnpaused;

    //public event EventHandler OnMenuOppened;
    //public event EventHandler OnMenuClosed;

    //private bool isGamePaused = false;
    //private bool isMenuOppened = false;

    //private void Awake() {
    //    Instance = this;
    //}

    //private void Start() {
    //    GameInput.Instance.OnMenuOpenCloseAction += GameInput_OnMenuOpenCloseAction;
    //}

    //private void GameInput_OnMenuOpenCloseAction(object sender, System.EventArgs e) {
    //    ToggleOpenMenu();
    //}

    //private void ToggleOpenMenu() {
    //    isMenuOppened = !isMenuOppened;
    //    if (isMenuOppened) {
    //        OnMenuOppened?.Invoke(this, EventArgs.Empty);
    //    } else {
    //        OnMenuClosed?.Invoke(this, EventArgs.Empty);
    //    }
    //}

    //private void TogglePauseGame() {
    //    isGamePaused = !isGamePaused;
    //    if (isGamePaused) {
    //        Time.timeScale = 0f;
    //        OnGamePaused?.Invoke(this, EventArgs.Empty);
    //    } else {
    //        Time.timeScale = 1f;
    //        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    //    }
    //}
}
