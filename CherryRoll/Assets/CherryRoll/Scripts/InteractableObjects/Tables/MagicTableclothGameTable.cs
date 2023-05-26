using Unity.Netcode;

public class MagicTableclothGameTable : NetworkBehaviour, IInteractableObject {

    public void Interact(Player player) {
        if (player.HasItem()) {
            // Player is carrying something
            MagicTableclothGameManager.Instance.IncreasePlayerScoreServerRpc(player.GetItem().GetItemCost());
            Item.DestroyItem(player.GetItem());
            MagicTableclothGameManager.Instance.GetPlayersScores();
        }
    }
}
