using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System;
//using UnityEngine.Rendering.PostProcessing;


public class GameEffects : MonoBehaviour
{
    public static GameEffects instance;

    [Header("Psycho effect parameters")]
    [SerializeField] Volume volume;
    private ColorAdjustments colorAdj;
    [SerializeField] float psychoEffectDuration = 5f;

    [Header("Screen shake parameters")]
    [SerializeField] float duration;
    [SerializeField] float strength = 1;
    [SerializeField] int vibrato = 10;
    float randomness = 90;


    [Header("Intro effect references")]
    [SerializeField] Image blackScreen;
    [SerializeField] Transform torchesParent;
    [SerializeField] float torchEffectTime =.2f;
    [SerializeField] float torchLightIntensity = 1.85f;
    bool introDone;

    public bool IntroDone => introDone;

    Camera mainCamera;

    private void Awake()
    {
        instance = this;

        mainCamera = Camera.main;
    }

    private void Start()
    {
        //postProcessVolume.profile.TryGetSettings(out colorAdj);
        volume.profile.TryGet(out colorAdj);
        colorAdj.hueShift.value = 0;

        IntroEffect();
    }

    public void CameraShake()
    {
        mainCamera.transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
    }

    public void IntroEffect()
    {
        StartCoroutine(IntroEffect_Coroutine());
    }

    IEnumerator IntroEffect_Coroutine()
    {
        if (GameController.isFirstOpen)
        {
            blackScreen.gameObject.SetActive(true);
            blackScreen.DOFade(1, 0);
            blackScreen.DOFade(0, 2f);


            foreach (Transform child in torchesParent)
            {
                child.gameObject.SetActive(false);
                child.GetComponentInChildren<Light2D>().intensity = 0;
            }

            float _tweenTime = 0.05f;

            foreach (Transform child in torchesParent)
            {
                Vector2 _origScale = child.localScale;
                child.gameObject.SetActive(true);
                child.DOScale(0, 0);
                child.DOScale(new Vector2(_origScale.x+1,_origScale.y+1), torchEffectTime);

                while (child.GetComponentInChildren<Light2D>().intensity < torchLightIntensity)
                {
                    child.GetComponentInChildren<Light2D>().intensity += Time.deltaTime * 20;
                    yield return null;
                }
                child.GetComponentInChildren<Light2D>().intensity = torchLightIntensity;
                //yield return new WaitForSeconds(0.02f);
                //child.DOScale(_origScale , torchEffectTime);
                yield return new WaitForSeconds(_tweenTime);
                //_tweenTime /= 1.2f;
            }
            introDone = true;
            blackScreen.gameObject.SetActive(false);
        }
        else
        {
            foreach (Transform child in torchesParent)
            {
                child.gameObject.SetActive(true);
                //child.DOScale(1, 0);
            }

            introDone = true;
            blackScreen.gameObject.SetActive(false);
        }
    }

    public void PlayPsychoEffect()
    {
        StartCoroutine(PlayPsychoEffect_Coroutine());
    }

    IEnumerator PlayPsychoEffect_Coroutine()
    {
        float t = 0f;
        float hueShiftTarget = -180f;

        float _halfEffectDuration = psychoEffectDuration / 2;

        while (t < (_halfEffectDuration))
        {
            colorAdj.hueShift.value = Mathf.Lerp(0, hueShiftTarget, t / _halfEffectDuration);
            yield return null;
            t += Time.deltaTime;
        }

        while (t < (psychoEffectDuration))
        {
            colorAdj.hueShift.value = Mathf.Lerp(hueShiftTarget, 0, (t - (_halfEffectDuration)) / (_halfEffectDuration));
            yield return null;
            t += Time.deltaTime;
        }

        colorAdj.hueShift.value = 0;
    }
}
