using Unity.Netcode;

public class Table : NetworkBehaviour, IInteractableObject {

    public void Interact(Player player) {
        if (player.HasItem()) {
            // Player is carrying something
            BaseGameManager.Instance.IncreasePlayerScoreByOneServerRpc();
            Item.DestroyItem(player.GetItem());
            BaseGameManager.Instance.GetPlayersScores();
        }
    }
}
