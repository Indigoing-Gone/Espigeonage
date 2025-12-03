using System;
using UnityEngine;

public class PigeonSender : MonoBehaviour
{
    public static Action SendPigeonEvent;

    public void SendPigeon()
    {
        SendPigeonEvent?.Invoke();
    }
}