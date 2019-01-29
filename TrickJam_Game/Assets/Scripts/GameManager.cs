using System;
using UnityEngine;


public enum GameStates
{
    OUTISDE,
    ROOM,
    PLAY

}


public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private GameStates _mCurrentCanvasStates;



    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);


        DontDestroyOnLoad(this);


        _mCurrentCanvasStates = GameStates.OUTISDE;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateGameState(GameStates i_State)
    {
        switch (i_State)
        {
            case GameStates.OUTISDE:
                break;
            case GameStates.ROOM:
                break;
            case GameStates.PLAY:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(i_State), i_State, null);
        }
    }

    public void StartGame()
    {
        Debug.Log("Game Manager Start GAme");
        UpdateGameState(GameStates.PLAY);
    }
}
