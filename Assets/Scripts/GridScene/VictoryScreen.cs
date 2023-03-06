using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] RectTransform panel;
    public static bool beatGame = false;
    [SerializeField] GameObject restartButton;
    [SerializeField] TextMeshProUGUI fruitAmountText;
    EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        beatGame = false;
        screen.SetActive(false);
        panel.DOScale(0, 0f);
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowVictoryScreen();
        }
    }

    public void ShowVictoryScreen()
    {
        fruitAmountText.text = GameController.instance.FruitsToBeatGame + " FRUITS";
        //Debug.LogWarning("VICTORY!");
        beatGame = true;
        screen.SetActive(true);
        panel.DOScale(1, .25f).SetEase(Ease.OutBack);
        eventSystem.SetSelectedGameObject(restartButton);
    }
}
