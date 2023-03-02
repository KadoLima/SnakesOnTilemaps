using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameEffects : MonoBehaviour
{

    public static GameEffects instance;

    [Header("Screen shake parameters")]
    [SerializeField] float duration;
    [SerializeField] float strength = 1;
    [SerializeField] int vibrato = 10;
    float randomness = 90;

    Camera mainCamera;

    private void Awake()
    {
        instance = this;

        mainCamera = Camera.main;
    }

    public void CameraShake()
    {
        mainCamera.transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
    }

}
