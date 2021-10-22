using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

namespace Assets.Scripts
{
    public class KeyboardChange : MonoBehaviour
    {
        private Animator anim;
        private bool isChangingColor;
        private bool isMoving;

        private void Awake()
        {
            if (!TryGetComponent(out anim))
            {
                Debug.LogWarning("Missing Animator component.");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C) && !isMoving)
            {
                isChangingColor = !isChangingColor;
                anim.SetBool("isChangingColor", isChangingColor);
            }
            if (Input.GetKeyDown(KeyCode.M) && !isChangingColor)
            {
                isMoving = !isMoving;
                anim.SetBool("isMoving", isMoving);
            }
        }
    }
}