using System;
using Unity.Netcode;

public class LobbyButton : NetworkBehaviour, IInteractableObject {


    public event EventHandler OnPlayerPressButton;


    public void Interact(Player player) {
        OnPlayerPressButton?.Invoke(this, EventArgs.Empty);

        if (IsHost) {
            GameChooseMenuUI.Instance.Show();
        }
    }
}
