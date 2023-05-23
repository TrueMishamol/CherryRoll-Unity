using Unity.Netcode;

public class CollectThePlateGameTable : NetworkBehaviour, IInteractableObject {


    public static CollectThePlateGameTable Instance { get; private set; }


    private void Awake() {
        Instance = this;
    }

    public void Interact(Player player) {
        CollectThePlateGameManager.Instance.DeliverItem(player.GetItem());
    }
}
