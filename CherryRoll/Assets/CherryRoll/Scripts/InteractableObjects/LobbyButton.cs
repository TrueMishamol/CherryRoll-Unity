using System;
using Unity.Netcode;

public class LobbyButton : NetworkBehaviour, IInteractableObject {


    public event EventHandler OnPlayerPressButton;


    public void Interact(Player player) {
        GameChooseMenuUI.Instance.SwitchOpenClose();

        InteractLogicServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc() {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc() {
        OnPlayerPressButton?.Invoke(this, EventArgs.Empty);
    }
}
