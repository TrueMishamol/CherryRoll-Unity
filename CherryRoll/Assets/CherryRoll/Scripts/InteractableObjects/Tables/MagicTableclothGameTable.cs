using Unity.Netcode;

public class MagicTableclothGameTable : NetworkBehaviour, IInteractableObject {

    public void Interact(Player player) {
        if (player.HasItem()) {
            // Player is carrying something
            MagicTableclothGameManager.Instance.IncreasePlayerScoreByOneServerRpc();
            Item.DestroyItem(player.GetItem());
            MagicTableclothGameManager.Instance.GetPlayersScores();
        }
    }
}
