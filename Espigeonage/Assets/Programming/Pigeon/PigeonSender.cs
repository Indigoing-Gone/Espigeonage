using UnityEngine;

public class PigeonSender : MonoBehaviour
{
    [SerializeField] private GameObject pigeonPrefab;
    [SerializeField] private Transform pigeonStart;
    [SerializeField] private Transform pigeonEnd;

    [SerializeField] private string successText;
    [SerializeField] private string failureText;

    #region Event Callbacks

    private void OnEnable()
    {
        GameManager.MissionResult += SendMissionResult;
        GameManager.SendPigeon += SendPigeon;
    }

    private void OnDisable()
    {
        GameManager.MissionResult -= SendMissionResult;
        GameManager.SendPigeon -= SendPigeon;
    }

    #endregion

    private void SendMissionResult(bool _result)
    {
        SendPigeon(_result ? successText : failureText); 
    }

    private void SendPigeon(string _text)
    {
        Instantiate(pigeonPrefab).GetComponent<Pigeon>().Init(_text, pigeonStart, pigeonEnd);
    }
}
