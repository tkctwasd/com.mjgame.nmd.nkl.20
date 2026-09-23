using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public enum DirectionDoor { A = 1, B = -1 }
[Serializable]
public class Door
{
    public UnityEvent onStartOpen;
    public UnityEvent onClosedComplete;

    public enum DoorType { Rotating, Sliding }
    public enum RotateAxis { X, Y, Z }
    public enum DoorState { Closed, Opening, Open, Closing }

    [Header("Settings")]
    [SerializeField] private DoorType doorType = DoorType.Rotating;
    [SerializeField] private RotateAxis pivotAxis = RotateAxis.Y;
    [SerializeField] private float openTime = 0.5f;
    [SerializeField] private bool autoClose = false;
    [SerializeField] private float closeDelay = 2f;
    [SerializeField] private bool onlyOpenOnce = false;

    [ShowIf(nameof(doorType), (int)DoorType.Rotating)]
    [SerializeField, ColorLabel] private float rotateAngle = 90f;

    [ShowIf(nameof(doorType), (int)DoorType.Sliding)]
    [SerializeField, ColorLabel(1f, 0.65f, 0f)] private float slideDistance = 1f;

    [Header("Audio")]
    public TypeAudio audio = TypeAudio.VentilationDoor;
    private Transform doorTransform;
    private Vector3 startPos;
    private Quaternion startRot;

    private Tween currentTween;
    private DoorState state = DoorState.Closed;
    private bool isOpen = false;
    private bool isAutoOperating = false;
    private float currentDirection = 1f;

    public bool IsOpen => isOpen;

    public void Init(Transform door)
    {
        doorTransform = door;
        startPos = door.localPosition;
        startRot = door.localRotation;

        state = DoorState.Closed;
        isOpen = false;
        isAutoOperating = false;
        currentDirection = 1f;
    }
    public void ToggleDoor(DirectionDoor dir = DirectionDoor.A)
    {
        if (doorTransform == null) return;
        if (isAutoOperating) return;
        if (state == DoorState.Opening || state == DoorState.Closing) return;

        if (isOpen)
        {
            CloseDoor();
            return;
        }

        float openDir = doorType == DoorType.Rotating
            ? Mathf.Sign((int)dir)
            : 1f;

        OpenDoor(openDir);
    }

    private void OpenDoor(float direction)
    {
        if (onlyOpenOnce && isOpen) return;

        state = DoorState.Opening;
        isOpen = true;
        currentDirection = direction;

        onStartOpen?.Invoke();
        PlaySound();
        currentTween?.Kill();

        Vector3 axis = GetAxisVector(pivotAxis);
        Sequence seq = DOTween.Sequence();

        if (doorType == DoorType.Rotating)
        {
            Quaternion targetRot =
                startRot * Quaternion.AngleAxis(rotateAngle * direction, axis);

            seq.Join(
                doorTransform
                    .DOLocalRotateQuaternion(targetRot, openTime)
                    .SetEase(Ease.OutSine)
            );
        }
        else
        {
            Vector3 targetPos =
                startPos + axis.normalized * slideDistance;

            seq.Join(
                doorTransform
                    .DOLocalMove(targetPos, openTime)
                    .SetEase(Ease.OutSine)
            );
        }

        seq.OnComplete(() =>
        {
            state = DoorState.Open;

            if (autoClose && !onlyOpenOnce)
            {
                isAutoOperating = true;
                DOVirtual.DelayedCall(closeDelay, () =>
                {
                    CloseDoor();
                    isAutoOperating = false;
                });
            }
        });

        currentTween = seq;
    }

    public void CloseDoor()
    {
        if (onlyOpenOnce) return;

        state = DoorState.Closing;
        isOpen = false;

        PlaySound();
        currentTween?.Kill();

        Sequence seq = DOTween.Sequence();

        if (doorType == DoorType.Rotating)
        {
            seq.Join(
                doorTransform
                    .DOLocalRotateQuaternion(startRot, openTime)
                    .SetEase(Ease.InSine)
            );
        }
        else
        {
            seq.Join(
                doorTransform
                    .DOLocalMove(startPos, openTime)
                    .SetEase(Ease.InSine)
            );
        }

        seq.OnComplete(() =>
        {
            state = DoorState.Closed;
            onClosedComplete?.Invoke();
        });

        currentTween = seq;
    }

    private Vector3 GetAxisVector(RotateAxis axis)
    {
        return axis switch
        {
            RotateAxis.X => Vector3.right,
            RotateAxis.Y => Vector3.up,
            RotateAxis.Z => Vector3.forward,
            _ => Vector3.up
        };
    }

    private void PlaySound()
    {
        switch (audio)
        {
            case TypeAudio.None:
                break;
            default:
                AudioManager.Instance.PlaySoundOne(audio.ToString());
                break;
        }
    }
}

public enum TypeAudio
{
    None, VentilationDoor, WoodDoor,
}