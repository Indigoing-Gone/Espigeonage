using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionGrabber : SlotGrabber
{
    static public event Action<List<Vector2Int>> MissionCompleted;

    public void CompleteMission()
    {
        if (currentGrabbable == null) return;

        MonoBehaviour _grabbableObject = currentGrabbable as MonoBehaviour;
        if (_grabbableObject == null) return;

        _grabbableObject.TryGetComponent<BlueprintGrid>(out BlueprintGrid _blueprint);
        if (_blueprint == null) return;
        MissionCompleted?.Invoke(_blueprint.SpyPath);
    }
}
