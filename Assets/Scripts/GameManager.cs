using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour // Class that sets needed references for each class in order to start the game
    {
        [SerializeField] private Planet planet;
        [SerializeField] private Player player;
        [SerializeField] private ItemSelection itemSelectionScript;
        [SerializeField] private PlantItem plantItemScript;
        [SerializeField] private List<PlantableItemPair> plantableItemPairs; // Store the pair of UI's demo items in the scene with their corresponding prefab to be instantiated

        [System.Serializable]
        public struct PlantableItemPair
        {
            public GameObject demoItem;
            public Transform itemPrefab;
        }

        private void Awake()
        {
            List<GameObject> demoItems = new List<GameObject>();
            List<Transform> itemPrefabs = new List<Transform>();
            foreach (PlantableItemPair pair in plantableItemPairs)
            {
                demoItems.Add(pair.demoItem);
                itemPrefabs.Add(pair.itemPrefab);
            }
            plantItemScript.SetItemPrefabList(itemPrefabs); // Set list of prefabs to be instantiated
            itemSelectionScript.SetItemDemoList(demoItems); // Set list of demo items to show in UI
            itemSelectionScript.ScrollItemEvent.AddListener(plantItemScript.SetPrefabIndex); // Notify when item is changed from UI
        }

        private void Start()
        {
            player.SetInputEvent(planet.MouseClickEvent); // Notify when planet is clicked.
        }
    }
}

