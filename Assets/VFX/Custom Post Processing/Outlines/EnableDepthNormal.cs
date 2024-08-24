using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class EnableDepthNormal : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }
}