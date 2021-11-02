using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float RotationPerSecond = 1;
    private bool _rotate;

    protected void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotationPerSecond);
    }
}