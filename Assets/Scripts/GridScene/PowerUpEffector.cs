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

    [Space(10)]

    [Header("Psycho Fruit parameters")]
    [SerializeField]bool isPsychoFruit;
    [SerializeField] Color[] psychoFruitColors;

    SpriteRenderer mySpriteRenderer;

    Sequence floatSequence;

    private void Start()
    {
        Float();

        mySpriteRenderer = GetComponent<SpriteRenderer>();

        RandomChangeSpriteColor();
    }

    private void OnDestroy()
    {
        floatSequence.Kill();
    }

    private void Float()
    {
        startPos = transform.position;

        floatSequence= DOTween.Sequence();
        floatSequence.Append(transform.DOMoveY(startPos.y + floatDistance, floatDuration).SetEase(Ease.InOutSine));
        floatSequence.Append(transform.DOMoveY(startPos.y, floatDuration).SetEase(Ease.InOutSine));
        floatSequence.SetLoops(-1);
        floatSequence.Play();
    }

    void RandomChangeSpriteColor()
    {
        if (!isPsychoFruit)
            return;

        float _duration = .25f;
        int _chosenColorIndex = Random.Range(0, psychoFruitColors.Length);

        mySpriteRenderer.DOColor(psychoFruitColors[_chosenColorIndex], _duration)
            .SetLoops(1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                RandomChangeSpriteColor();
            });
    }
}
