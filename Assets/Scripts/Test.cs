using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float x;
    public float y;

    private void Update()
    {
        int horizontal = (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0) + (Input.GetKey(KeyCode.RightArrow) ? -1 : 0);
        int vertical = (Input.GetKey(KeyCode.DownArrow) ? -1 : 0) + (Input.GetKey(KeyCode.UpArrow) ? 1 : 0);
        horizontal = 1;
        vertical = 1;
        transform.Rotate(y * vertical * Time.deltaTime, x * horizontal * Time.deltaTime, 0f);
    }
}
