using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    private Renderer rd;
    private Rigidbody rb;
    private bool isColorRed;

    private void Start()
    {
        if (!TryGetComponent(out rd))
        {
            Debug.LogWarning("Missing renderer component.");
        }
        if (!TryGetComponent(out rb))
        {
            Debug.LogWarning("Missing Rigidbody component.");
        }
        Debug.Log("Hello World");
        isColorRed = rd.material.color == Color.red;
    }

    private void OnMouseDown()
    {
        ChangeColor();
        MakeGravityActive();
    }

    // Enable gravity
    private void MakeGravityActive()
    {
        rb.useGravity = true;
    }

    // Change color of renderer's material
    private void ChangeColor()
    {
        if (isColorRed)
        {
            rd.material.color = Color.blue;
        }
        else
        {
            rd.material.color = Color.red;
        }
        isColorRed = !isColorRed;
    }
}
