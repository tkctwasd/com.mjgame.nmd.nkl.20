using System.Collections;
using UnityEngine;

/// <summary>
/// Rung camera mot nhip (vd: tiep dat manh, trung don). Cac kieu rung keo dai theo co che
/// rieng cua game thi game tu tinh roi goi MainCamController.SetShakeAmplitude.
/// </summary>
public class CameraShake : MonoBehaviour
{
    /// <summary>Dang rung theo xung, game khong nen ghi de bien do luc nay.</summary>
    public bool IsImpulsing { get; private set; }

    public void ShakeImpulse(float duration = 1f, float amplitude = 3f)
    {
        StartCoroutine(ShakeImpulseRoutine(duration, amplitude));
    }

    private IEnumerator ShakeImpulseRoutine(float duration, float amplitude)
    {
        IsImpulsing = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            MainCamController.Instance.SetShakeAmplitude(Mathf.Lerp(amplitude, 0f, t), amplitude);
            yield return null;
        }

        MainCamController.Instance.SetShakeAmplitude(0f, amplitude);
        IsImpulsing = false;
    }
}
