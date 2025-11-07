using Unity.Cinemachine;
using UnityEngine;

public class Desk : MonoBehaviour
{
    [SerializeField] private CinemachineCamera deskCamera;
    public CinemachineCamera DeskCamera => deskCamera;
    [SerializeField] private Transform dropLocation;
    public Transform DropLocation => dropLocation;

    void OnValidate()
    {
        if (!deskCamera) deskCamera = GetComponentInChildren<CinemachineCamera>(false);
    }
}
