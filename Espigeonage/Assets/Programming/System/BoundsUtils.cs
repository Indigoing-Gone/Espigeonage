using UnityEngine;

public static class BoundsUtils
{
    public static bool TryGetLocalBoundsSelf(Transform _root, out Bounds _bounds)
    {
        var _renderers = _root.GetComponents<MeshRenderer>();
        return TryGetLocalBounds(_renderers, _root, out _bounds);
    }

    public static bool TryGetLocalBoundsChildren(Transform _root, out Bounds _bounds)
    {
        var renderers = _root.GetComponentsInChildren<MeshRenderer>();
        return TryGetLocalBounds(renderers, _root, out _bounds);
    }

    private static bool TryGetLocalBounds(Renderer[] _renderers, Transform _space, out Bounds _bounds)
    {
        if (_renderers == null || _renderers.Length == 0)
        {
            _bounds = default;
            return false;
        }

        bool _initialized = false;
        Vector3 _min = Vector3.zero;
        Vector3 _max = Vector3.zero;

        foreach (var r in _renderers)
        {
            if (r == null) continue;

            var lb = r.localBounds;

            Vector3[] _corners =
            {
                new(lb.min.x, lb.min.y, lb.min.z),
                new(lb.min.x, lb.min.y, lb.max.z),
                new(lb.min.x, lb.max.y, lb.min.z),
                new(lb.min.x, lb.max.y, lb.max.z),
                new(lb.max.x, lb.min.y, lb.min.z),
                new(lb.max.x, lb.min.y, lb.max.z),
                new(lb.max.x, lb.max.y, lb.min.z),
                new(lb.max.x, lb.max.y, lb.max.z),
            };

            for (int i = 0; i < _corners.Length; i++)
            {
                Vector3 _world = r.transform.TransformPoint(_corners[i]);
                Vector3 _inSpace = _space.InverseTransformPoint(_world);

                if (!_initialized)
                {
                    _min = _max = _inSpace;
                    _initialized = true;
                }
                else
                {
                    _min = Vector3.Min(_min, _inSpace);
                    _max = Vector3.Max(_max, _inSpace);
                }
            }
        }

        if (!_initialized)
        {
            _bounds = default;
            return false;
        }

        _bounds = new Bounds();
        _bounds.SetMinMax(_min, _max);
        return true;
    }

    public static Vector3 BoundsCenter(Bounds _bounds)
    {
        return _bounds.center;
    }

    public static Vector3 BoundsBottomCenterY(Bounds _bounds)
    {
        return new Vector3(
            _bounds.center.x,
            _bounds.min.y,
            _bounds.center.z
        );
    }

    public static Vector3 BoundsBottomCenterZ(Bounds _bounds)
    {
        return new Vector3(
            _bounds.center.x,
            _bounds.center.y,
            _bounds.min.z
        );
    }
}
