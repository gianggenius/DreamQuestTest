using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private Transform         spawnerButtonParent;
        [SerializeField] private ObjectSpawnButton spawnButtonPrefab;

        private InventoryManager _inventoryManager;
        private void Start()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;
            GenerateSpawnButton();
        }

        private void GenerateSpawnButton()
        {
            foreach (var item in _inventoryManager.Inventory.Keys)
            {
                var objectData = item as ObjectData;
                if (objectData == null) continue;
                var spawnButton = Instantiate(spawnButtonPrefab, spawnerButtonParent);
                spawnButton.Populate(objectData);
            }
        }
    }
}