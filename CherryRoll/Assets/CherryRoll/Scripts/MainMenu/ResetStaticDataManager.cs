using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour {

    private void Awake() {
        Player.ResetStaticData();
    }
}
