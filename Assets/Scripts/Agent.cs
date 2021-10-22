using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private Camera trackCam;
        [SerializeField] private NavMeshAgent agent;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetDestinationToMousePosition();
            }
        }

        private void SetDestinationToMousePosition()
        {
            RaycastHit hit;
            Ray ray = trackCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}