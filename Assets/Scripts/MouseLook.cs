using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    public float minimumY = -60f;
    public float maximumY = 60f;

    float rotationY = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        rotationY += mouseY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y + mouseX, 0);
    }
}
