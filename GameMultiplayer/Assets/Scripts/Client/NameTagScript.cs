using UnityEngine;

public class NameTagScript : MonoBehaviour
{
    private Transform camera;

    private void Start()
    {
        camera = GameObject.Find("LocalPlayerCamera").transform;
        Debug.Log(camera.gameObject.name);
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position+camera.rotation*Vector3.forward,camera.rotation*Vector3.up);
    }
}
