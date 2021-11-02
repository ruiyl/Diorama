using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class ItemSelection : MonoBehaviour
    {
        private List<GameObject> demoItems;
        private int currentItemIndex;

        [HideInInspector] public UnityEvent<int> ScrollItemEvent;

        public void SetItemDemoList(List<GameObject> itemDemoList)
        {
            demoItems = itemDemoList;
        }

        private void Start()
        {
            foreach (GameObject item in demoItems)
            {
                item.SetActive(false);
            }
            ShowNextItem(0, 0);
        }

        private void ShowNextItem(int currentItem, int nextItem)
        {
            demoItems[currentItem].SetActive(false);
            demoItems[nextItem].SetActive(true);

            ScrollItemEvent?.Invoke(nextItem);
        }

        public void ScrollLeft()
        {
            ShowNextItem(currentItemIndex, --currentItemIndex < 0 ? currentItemIndex += demoItems.Count : currentItemIndex);
        }

        public void ScrollRight()
        {
            ShowNextItem(currentItemIndex, ++currentItemIndex >= demoItems.Count ? currentItemIndex %= demoItems.Count : currentItemIndex);
        }
    }
}
