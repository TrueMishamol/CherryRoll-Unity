using System;
using UnityEngine;

public class LobbyButton : MonoBehaviour, IInteractableObject {


    public event EventHandler OnPlayerPressButton;


    public void Interact(Player player) {
        OnPlayerPressButton?.Invoke(this, EventArgs.Empty);

        //! if host
        GameChooseMenuUI.Instance.Show();
    }
}
