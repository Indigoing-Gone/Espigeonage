using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Desk : MonoBehaviour
{
    [Header("Desk Components")]
    [SerializeField] private CinemachineCamera deskCamera;
    public CinemachineCamera DeskCamera => deskCamera;
    [SerializeField] private Transform dropLocation;
    public Transform DropLocation => dropLocation;

    [SerializeField] private List<GameObject> objectsOnDesk;

    private void OnValidate()
    {
        if (!deskCamera) deskCamera = GetComponentInChildren<CinemachineCamera>(false);
    }
}
