using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private Player player;
    public Player Player => player;
    [SerializeField] private InputReader input;
    public InputReader Input => input;
    [SerializeField] private CameraManager cameraManager;
    public CameraManager CameraManager => cameraManager;

    private void OnValidate()
    {
        cameraManager = GetComponent<CameraManager>();
    }
}
