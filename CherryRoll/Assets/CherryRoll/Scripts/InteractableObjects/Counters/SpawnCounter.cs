using UnityEngine;

public class SpawnCounter : BaseCounter {


    [SerializeField] private ItemSO itemSO;

    private float spawnCountdown;
    private float spawnCountdownMin = 5f;
    private float spawnCountdownMax = 30f;


    private void Start() {
        if (!IsServer) return;

        spawnCountdown = Random.Range(spawnCountdownMin, spawnCountdownMax);
    }

    private void Update() {
        if (!IsServer) return;

        spawnCountdown -= Time.deltaTime;

        if (spawnCountdown < 0f) {
            SpawnItemOnCounter();

            spawnCountdown = Random.Range(spawnCountdownMin, spawnCountdownMax);
        }
    }

    private void SpawnItemOnCounter() {
        if (!HasItem()) {
            // There is no Item on Counter
            Item.SpawnItem(itemSO, this);
        }
    }
}
