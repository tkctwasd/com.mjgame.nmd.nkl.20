using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityExtensions;

public class MainCamController : Singleton<MainCamController>
{
    CinemachineBrain cinemachineBrain;
    public CameraShake CameraShake;
    [SerializeField] CinemachineVirtualCamera CMCam3;
    [SerializeField] CinemachineVirtualCamera CMCam1;
    Cinemachine3rdPersonFollow cinemachine3RdPersonFollow;
    CinemachineBasicMultiChannelPerlin perlinNoise;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        cinemachineBrain = GetComponent<CinemachineBrain>();
        cinemachine3RdPersonFollow = CMCam3.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        perlinNoise = CMCam3.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void SetTimeChangeCam(float timeChangeCam)
    {
        cinemachineBrain.m_DefaultBlend.m_Time = timeChangeCam;
    }
    public void SetFollowPlayer(Transform third, Transform first)
    {
        CMCam3.Follow = third;
        CMCam1.Follow = first;
    }
    public void SetDistance(float dis = 8f)
    {
        cinemachine3RdPersonFollow.CameraDistance = dis;
    }

    public void ResetCameraDistance()
    {
        SetDistance(DataSetting.Current.Distance);
    }

    public void ResetZoom()
    {
        SetDistance(DataSetting.Current.Distance);
    }

    public void SetDampingCamera(float dampingX = 0, float dampingY = 0, float dampingZ = 0)
    {
        cinemachine3RdPersonFollow.Damping = new Vector3(dampingX, dampingY, dampingZ);
    }

    Tween fovTween;
    public void SetFieldOfView(float fov, float duration = 0.35f)
    {
        if (CMCam3 == null) return;

        float from = CMCam3.m_Lens.FieldOfView;
        if (Mathf.Approximately(from, fov)) return;

        fovTween?.Kill();

        if (duration <= 0f)
        {
            CMCam3.m_Lens.FieldOfView = fov;
            return;
        }

        fovTween = DOVirtual.Float(from, fov, duration, value => CMCam3.m_Lens.FieldOfView = value)
                            .SetEase(Ease.OutQuad)
                            .SetUpdate(true);
    }

    public void SetShakeAmplitude(float amplitude, float maxShakeAmplitude)
    {
        if (perlinNoise != null)
        {
            perlinNoise.m_AmplitudeGain = amplitude;
            perlinNoise.m_FrequencyGain = Mathf.Lerp(0.8f, 2.0f, amplitude / maxShakeAmplitude);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        fovTween?.Kill();
    }

    public void ActiveCamera(bool third, bool first)
    {
        GameObjectExtensions.ActiveObj(CMCam3.gameObject, third);
        GameObjectExtensions.ActiveObj(CMCam1.gameObject, first);
    }

    public void SetCameraInit(Transform targetThird, Transform targetFirst)
    {
        SetFollowPlayer(targetThird, targetFirst);
        GameObjectExtensions.ActiveObj(CMCam3.gameObject, true);
        SetDistance(DataSetting.Current.Distance);
        CMCam3.OnTargetObjectWarped(targetThird, targetThird.position - CMCam3.transform.position);
        CMCam3.PreviousStateIsValid = false;
        cinemachineBrain.ManualUpdate();
        ActiveCamera(true, false);
    }

}
