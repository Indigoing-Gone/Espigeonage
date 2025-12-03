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
        if (currentGrabbable == null)
        {
            MissionCompleted?.Invoke(new("", new List<Vector2Int>()));
            return;
        }

        MonoBehaviour _grabbableObject = currentGrabbable as MonoBehaviour;
        if (_grabbableObject == null)
        {
            MissionCompleted?.Invoke(new("", new List<Vector2Int>()));
            return;
        }

        _grabbableObject.TryGetComponent(out Blueprint _blueprint);
        if (_blueprint == null)
        {
            MissionCompleted?.Invoke(new("", new List<Vector2Int>()));
            return;
        }
        
        MissionCompleted?.Invoke(new(_blueprint.LocationName, _blueprint.SpyPath));
    }
}
