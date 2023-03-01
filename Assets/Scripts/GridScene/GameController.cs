using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public WallTilesEffector wallTilesEffector;
    public Player player;

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
        return player.IsReady;
        //return wallTilesEffector.IsDone && player.IsReady;
    }
}
