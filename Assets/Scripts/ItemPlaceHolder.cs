using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class ItemPlaceHolder : MonoBehaviour
    {
        private Animator animator;
        private Transform currentGrowingItem;

        [HideInInspector] public UnityEvent GrowEndEvent;

        private void Awake()
        {
            if (!TryGetComponent(out animator))
            {
                Debug.LogWarning("Missing Animator");
            }
        }

        public void StartGrowing(Transform item)
        {
            currentGrowingItem = item;
            animator.SetTrigger("Grow");
        }

        public void OnGrowEnd()
        {
            currentGrowingItem.SetParent(transform.parent);
            GrowEndEvent?.Invoke();
        }
    }
}
