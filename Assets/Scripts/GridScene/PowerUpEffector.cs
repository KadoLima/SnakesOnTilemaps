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


    private void Start()
    {
        Float();

        mySpriteRenderer = GetComponent<SpriteRenderer>();

        RandomChangeSpriteColor();
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

    void RandomChangeSpriteColor()
    {
        if (!isPsychoFruit)
            return;

        float _duration = .25f;
        int _chosenColorIndex = Random.Range(0, psychoFruitColors.Length);

        //Debug.LogWarning("chosen color is: " + _chosenColorIndex);

        mySpriteRenderer.DOColor(psychoFruitColors[_chosenColorIndex], _duration)
            .SetLoops(1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                RandomChangeSpriteColor();
            });
    }
}
