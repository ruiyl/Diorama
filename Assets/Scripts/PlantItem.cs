using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PlantItem : MonoBehaviour
    {
        [SerializeField] private ItemPlaceHolder placeHolder;
        
        private List<Transform> itemPrefabs;
        private int currentPrefabIndex;

        public void SetPlantingState(Vector3 position, Quaternion rotation)
        {
            placeHolder.transform.SetPositionAndRotation(position, rotation);
        }

        public void BeginPlanting()
        {
            Transform newItem = Instantiate(itemPrefabs[currentPrefabIndex], placeHolder.transform);
            newItem.SetPositionAndRotation(placeHolder.transform.position, placeHolder.transform.rotation);
            placeHolder.StartGrowing(newItem);
        }

        public UnityEvent GetGrowEndEvent()
        {
            return placeHolder.GrowEndEvent;
        }

        public void SetPrefabIndex(int index)
        {
            currentPrefabIndex = index;
        }

        public void SetItemPrefabList(List<Transform> itemPrefabList)
        {
            itemPrefabs = itemPrefabList;
        }
    }
}

