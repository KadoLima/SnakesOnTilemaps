using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUpEffector : MonoBehaviour
{
    [Header("Floating effect parameters")]
    [SerializeField] float floatDistance = 0.5f;
    [SerializeField] float floatDuration = 1f;

    private Vector3 startPos;

    private void Start()
    {
        Float();
    }

    private void Float()
    {
        startPos = transform.position;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(startPos.y + floatDistance, floatDuration).SetEase(Ease.InOutSine));
        sequence.Append(transform.DOMoveY(startPos.y, floatDuration).SetEase(Ease.InOutSine));
        sequence.SetLoops(-1);
        sequence.Play();
    }


}
