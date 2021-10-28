using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Planet planet;
        [SerializeField] private PivotMovement pivotMovement;

        private void Start()
        {
            pivotMovement.SetMoveEvent(planet.MouseClickEvent);
        }
    }
}

