using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBalance : MonoBehaviour
{
    public static GameBalance instance;

    public GameBalance()
    {
        if (instance == null)
            instance = this;
        else throw new System.Exception("More than 1 GameBalance instance");
    }

    [Header("Общий вес")]
    public float MaxWeight = 30;
    public float appleWeight = 0.25f;
    public float cranberryWeight = 0.25f;
    public float blueberryWeight = 0.25f;
    public float coinWeight = 1f;
    public float capWeight = 2f;
    public float featherWeight = 1f;
}
