using Unity.Cinemachine;
using UnityEngine;

public class Desk : MonoBehaviour
{
    [SerializeField] private CinemachineCamera deskCamera;
    public CinemachineCamera DeskCamera => deskCamera;

    void OnValidate()
    {
        if (!deskCamera) deskCamera = GetComponentInChildren<CinemachineCamera>(false);
    }
}
