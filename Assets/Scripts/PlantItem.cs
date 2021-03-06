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

        public void SetPlantingPosition(Vector3 position)
        {
            placeHolder.transform.position = position;
        }

        public void SetPlantingRotation(Quaternion rotation)
        {
            placeHolder.transform.rotation = rotation;
        }

        public void BeginPlanting() // Instantiate item as the PlaceHolder's child and trigger growing animation
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

