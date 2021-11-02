using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class ItemSelection : MonoBehaviour // UI script for changing items
    {
        private List<GameObject> demoItems;
        private int currentItemIndex;

        [HideInInspector] public UnityEvent<int> ScrollItemEvent;

        public void SetItemDemoList(List<GameObject> itemDemoList)
        {
            demoItems = itemDemoList;
        }

        private void Start() // Hide all item, then show the first equipped item.
        {
            foreach (GameObject item in demoItems)
            {
                item.SetActive(false);
            }
            ShowNextItem(0, 0);
        }

        private void ShowNextItem(int currentItem, int nextItem) // Hide current item, then show the next item.
        {
            demoItems[currentItem].SetActive(false);
            demoItems[nextItem].SetActive(true);

            ScrollItemEvent?.Invoke(nextItem); // Trigger event for PlantItem to change its prefab.
        }

        public void ScrollLeft() // Scroll left. Cycle to the last if reach 0
        {
            ShowNextItem(currentItemIndex, --currentItemIndex < 0 ? currentItemIndex += demoItems.Count : currentItemIndex);
        }

        public void ScrollRight() // Scroll right. Cycle to the first if reach capacity
        {
            ShowNextItem(currentItemIndex, ++currentItemIndex >= demoItems.Count ? currentItemIndex %= demoItems.Count : currentItemIndex);
        }
    }
}
