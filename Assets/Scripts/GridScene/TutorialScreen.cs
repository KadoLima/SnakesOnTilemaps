using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] float fadeTime = .15f;
    [SerializeField] float showingTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.isFirstOpen == false)
            screen.SetActive(false);
        else
            StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        screen.SetActive(true);
        tutorialText.DOFade(0, 0);
        tutorialText.GetComponent<RectTransform>().DOScale(0, 0f);
        yield return new WaitUntil(() => GameController.instance.player.IsReady);
        tutorialText.DOFade(.8f, fadeTime);
        tutorialText.GetComponent<RectTransform>().DOScale(1, fadeTime);
        yield return new WaitForSeconds(showingTime);
        tutorialText.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        screen.SetActive(false);
    }

}
