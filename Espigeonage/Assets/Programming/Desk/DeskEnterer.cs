using System;
using UnityEngine;

public class DeskEnterer : MonoBehaviour, IInteractable
{
    [SerializeField] private Desk desk;
    public static event Action<Desk> EnteringDesk;

    private void Awake()
    {
        desk = GetComponent<Desk>();
    }

    public void Interact(Interactor _interactor)
    {
        EnteringDesk?.Invoke(desk);
    }
}
