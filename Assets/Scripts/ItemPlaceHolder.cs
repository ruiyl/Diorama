using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class ItemPlaceHolder : MonoBehaviour // Placeholder for planted item. So that growing animation can be reused easily.
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

        public void StartGrowing(Transform item) // Start animation that changes y-scale from 0 to 1
        {
            transform.localScale = Vector3.zero;
            currentGrowingItem = item;
            animator.SetTrigger("Grow");
        }

        public void OnGrowEnd() // Called animation event when the animation ends. Remove child to reuse this object on others items. Also triggers listening event
        {
            currentGrowingItem.SetParent(transform.parent);
            GrowEndEvent?.Invoke();
        }
    }
}
