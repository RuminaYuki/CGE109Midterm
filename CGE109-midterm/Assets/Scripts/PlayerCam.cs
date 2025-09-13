using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float senX, senY;
    public Transform OrienRotation;

    private float XRota, YRotat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (senY*2);

        YRotat += mouseX;

        XRota += mouseY;
        XRota = Mathf.Clamp(XRota, -90f, 90f);

        transform.rotation = Quaternion.Euler(XRota, YRotat, 0);
        OrienRotation.rotation = Quaternion.Euler(0, YRotat, 0);
    }
}
