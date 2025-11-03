using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private int activePriority = 10;
    [SerializeField] private int inactivePriority = 0;

    [SerializeField] private CinemachineCamera activeCamera;

    public void ChangeCamera(CinemachineCamera _newCamera)
    {
        if(activeCamera) activeCamera.Priority = inactivePriority;
        activeCamera = _newCamera;
        activeCamera.Priority = activePriority;
    }
}
