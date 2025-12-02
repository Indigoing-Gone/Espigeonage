using System.Collections;
using TMPro;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    [SerializeField] private float flyTime;
    [SerializeField] private TextMeshProUGUI noteText;
    private Transform startTransform;
    private Transform perchTransform;

    #region Init

    public void Init(string _text, Transform _start, Transform _end)
    {
        noteText.text = _text;
        transform.SetPositionAndRotation(_start.position, _start.rotation);
        startTransform = _start;
        perchTransform = _end;
        StartCoroutine(FlyRoutine(_start, _end));
    }

    #endregion

    #region Pigeon Behavior

    // Coos
    private void Coo()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFXType.PIGEON, transform.position);
    }

    // Easing function for the pigeon's movement from start to end transform
    private float PigeonEase(float _per)
    {
        return _per;
    }

    private IEnumerator FlyRoutine(Transform _start, Transform _end)
    {
        Coo();
        for (float i = 0; i < flyTime; i += Time.deltaTime)
        {
            float per = PigeonEase(i / flyTime);
            transform.SetPositionAndRotation(Vector3.Lerp(_start.position, _end.position, per),
                                             Quaternion.Lerp(_start.rotation, _end.rotation, per));
            yield return null;
        }
        transform.SetPositionAndRotation(_end.position, _end.rotation);
        Coo();
        yield break;
    }

    public void FlyAway()
    {
        StartCoroutine(FlyAwayRoutine());
    }

    private IEnumerator FlyAwayRoutine()
    {
        yield return StartCoroutine(FlyRoutine(perchTransform, startTransform));
        Destroy(gameObject);
    }

    #endregion
}
