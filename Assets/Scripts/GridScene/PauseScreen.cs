using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] CanvasGroup buttonsCanvasGroup;
    public static bool isPaused;
    float origScreenY;

    EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        origScreenY = buttonsCanvasGroup.GetComponent<RectTransform>().anchoredPosition.y;
        ResumeGame();

        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    void Pause()
    {
        if (VictoryScreen.beatGame)
            return;

        if (isPaused)
        {
            ResumeGame();
            return;
        }

        isPaused = true;
        screen.SetActive(true);
        eventSystem.SetSelectedGameObject(buttonsCanvasGroup.transform.GetChild(0).gameObject);

        float _animTime = 0.15f;
        buttonsCanvasGroup.DOFade(0, 0);
        buttonsCanvasGroup.GetComponent<RectTransform>().DOAnchorPosY(0, _animTime);
        buttonsCanvasGroup.DOFade(1, _animTime);
    }

    public void ResumeGame()
    {
        buttonsCanvasGroup.GetComponent<RectTransform>().DOAnchorPosY(origScreenY, 0);
        isPaused = false;
        screen.SetActive(false);
    }

    public void Restart()
    {
        GameController.isFirstOpen = true;
        Debug.LogWarning("Restarting game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
