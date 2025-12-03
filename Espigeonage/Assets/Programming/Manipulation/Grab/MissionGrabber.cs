using System;
using System.Collections.Generic;
using UnityEngine;

public struct MissionData
{
    public string Name;
    public List<Vector2Int> Path;

    public MissionData(string _name, List<Vector2Int> _path)
    {
        Name = _name;
        Path = _path;
    }
}

public class MissionGrabber : SlotGrabber
{
    static public event Action<MissionData> MissionCompleted;

    public void CompleteMission()
    {
        if (currentGrabbable == null) return;

        MonoBehaviour _grabbableObject = currentGrabbable as MonoBehaviour;
        if (_grabbableObject == null) return;

        _grabbableObject.TryGetComponent(out Blueprint _blueprint);
        MissionData data;
        if (_blueprint == null) data = new("", new List<Vector2Int>());
        else data = new(_blueprint.LocationName, _blueprint.SpyPath);
        MissionCompleted?.Invoke(data);
    }
}
