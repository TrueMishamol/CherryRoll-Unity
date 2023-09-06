using Unity.Netcode;

public class Knife : NetworkBehaviour {

    private float knifeLifeTime = 1.5f; //^ 2.5f

    private void Start() {
        if (!IsServer) return;

        Destroy(gameObject, knifeLifeTime);
    }
}
