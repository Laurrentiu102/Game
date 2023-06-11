using UnityEngine;
using System.Runtime.InteropServices;

public class CameraController : MonoBehaviour
{
    public Transform target;  // Player's transform
    public float distanceFromTarget = 5f;  // Initial distance from the player
    public float height = 2f;  // Height of the camera above the player
    public float rotationSpeed = 1f;  // Speed of camera rotation

    private float mouseX;  // Mouse X input
    private float mouseY;  // Mouse Y input
    private float lastMouseX;  // Last mouse X input
    private float lastMouseY;  // Last mouse Y input

    private RaycastHit hit;
    private Vector2 mousePos;


    private void Start()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);

        Cursor.visible = false;
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0f);
            
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distanceFromTarget + Vector3.up * height;
            
        transform.position = desiredPosition;
            
        transform.LookAt(target.position);
        target.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }

    private void ChangeDistanceFromTarget()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            distanceFromTarget -= 0.5f;
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            distanceFromTarget += 0.5f;

        distanceFromTarget = Mathf.Clamp(distanceFromTarget, 3, 40);
        
        Quaternion rotation = Quaternion.Euler(lastMouseY, lastMouseX, 0f);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distanceFromTarget + Vector3.up * height;
        if (Physics.Linecast(target.transform.position-transform.forward, desiredPosition, out hit))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = desiredPosition;
        }
        transform.LookAt(target.position);
    }

    private void Update()
    {
        ChangeDistanceFromTarget();
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            SetCursorPos((int)mousePos.x, Screen.height-(int)mousePos.y);
        }
        
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            mouseY = Mathf.Clamp(mouseY, -60f, 60f);
            lastMouseX = mouseX;
            lastMouseY = mouseY;
            
            Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0f);
            
            Vector3 desiredPosition = target.position - rotation * Vector3.forward * distanceFromTarget + Vector3.up * height;

            if (Physics.Linecast(target.transform.position-transform.forward, desiredPosition, out hit))
            {
                transform.position = hit.point;
            }
            else
            {
                transform.position = desiredPosition;
            }

            transform.LookAt(target.position);
            target.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        

        Debug.DrawLine(target.transform.position-transform.forward,target.position,Color.red);
    }
    
    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    private static extern bool SetCursorPos(int x, int y);
}