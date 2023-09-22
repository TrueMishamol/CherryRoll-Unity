using Unity.Netcode;

public class Knife : NetworkBehaviour {

    private float knifeLifeTime = 2.5f;

    private void Start() {
        if (!IsServer) return;

        Destroy(gameObject, knifeLifeTime);
    }
}
