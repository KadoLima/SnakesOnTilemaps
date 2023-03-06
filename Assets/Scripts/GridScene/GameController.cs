using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [Header("References")]
    public Player player;
    public VictoryScreen victoryScreen;
    [Header("Settings")]
    [SerializeField] bool hideCursor;
    [SerializeField] int fruitsToBeatGame = 100;
    public int FruitsToBeatGame => fruitsToBeatGame;


    public static bool isFirstOpen = true;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
       if (hideCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public bool IsPlayable()
    {
        return player.IsReady && GameEffects.instance.IntroDone;
        //return wallTilesEffector.IsDone && player.IsReady;
    }


}
