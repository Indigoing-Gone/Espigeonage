using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] CinemachineInputAxisController cameraInputController;

    [Header("Mouse Sensitivity")]
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityY;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateSensitivity();
    }

    private void UpdateSensitivity()
    {
        foreach (var controller in cameraInputController.Controllers)
        {
            if (controller.Name == "Look X (Pan)") controller.Input.Gain = sensitivityX; 
            if (controller.Name == "Look Y (Tilt)") controller.Input.Gain = sensitivityY;
        }
    }
}
