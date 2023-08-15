using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnCounter : BaseCounter {


    [SerializeField] private RandomItemSpawnListSO itemList;

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

    private ItemSO GetRandomItem() {
        List<RandomItemSpawnListSO.RandomItem> itemsList = itemList.RandomItemSpawnList;

        //^ Суммируем общую редкость всех предметов
        int totalRarity = 0;
        foreach (RandomItemSpawnListSO.RandomItem item in itemsList) {
            totalRarity += item.rarity;
        }

        //^ Генерируем случайное число от 0 до общей редкости
        int randomRarity = Random.Range(0, totalRarity);

        //^ Ищем предмет соответствующий сгенерированному числу
        foreach (RandomItemSpawnListSO.RandomItem item in itemsList) {
            if (randomRarity < item.rarity) {
                return item.itemSO;
            }
            randomRarity -= item.rarity;
        }

        //^ Если ни один предмет не соответствует сгенерированному числу, возвращаем первый предмет в списке
        return itemList.RandomItemSpawnList[0].itemSO;
    }

    private void SpawnItemOnCounter() {
        if (!HasItem()) {
            //^ There is no Item on Counter
            Item.SpawnItem(GetRandomItem(), this);
        }
    }
}
