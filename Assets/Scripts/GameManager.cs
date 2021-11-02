using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Planet planet;
        [SerializeField] private Player player;
        [SerializeField] private ItemSelection itemSelectionScript;
        [SerializeField] private PlantItem plantItemScript;
        [SerializeField] private List<PlantableItemPair> plantableItemPairs;

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
            plantItemScript.SetItemPrefabList(itemPrefabs);
            itemSelectionScript.SetItemDemoList(demoItems);
            itemSelectionScript.ScrollItemEvent.AddListener(plantItemScript.SetPrefabIndex);
        }

        private void Start()
        {
            player.SetInputEvent(planet.MouseClickEvent);
        }
    }
}

