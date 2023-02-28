using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUpEffector : MonoBehaviour
{
    [Header("Floating effect parameters")]
    [SerializeField] float floatDistance = 0.5f;
    [SerializeField] float floatDuration = 1f;
    [Header("Screen shake parameters")]
    [SerializeField] float duration;
    [SerializeField] float strength = 1;
    [SerializeField] int vibrato = 10;
    float randomness = 90;

    private Vector3 startPos;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        startPos = transform.position;

        Float();
    }

    private void Float()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(startPos.y + floatDistance, floatDuration).SetEase(Ease.InOutSine));
        sequence.Append(transform.DOMoveY(startPos.y, floatDuration).SetEase(Ease.InOutSine));
        sequence.SetLoops(-1);
        sequence.Play();
    }

    public void CameraShake()
    {
        mainCamera.transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
    }
}
