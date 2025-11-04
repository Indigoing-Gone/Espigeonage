using System;

public interface IGrabbable
{
    public event Action BeingGrabbed;
    public bool Grab(Grabber _grabber);
    public void Release();
}
