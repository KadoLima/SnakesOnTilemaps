using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Player player;


    public static bool isFirstOpen = true;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public bool IsPlayable()
    {
        return player.IsReady && GameEffects.instance.IntroDone;
        //return wallTilesEffector.IsDone && player.IsReady;
    }


}
