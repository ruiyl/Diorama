using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Planet planet;
        [SerializeField] private Player player;

        private void Start()
        {
            player.SetInputEvent(planet.MouseClickEvent);
        }
    }
}

