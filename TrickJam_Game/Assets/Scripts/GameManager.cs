using System;
using UnityEngine;


public enum GameStates
{
    OUTISDE,
    ROOM,
    PLAY

}

public enum PotatoState
{
    LIMBO,
    HERE
}


public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private GameStates m_CurrentCanvasStates;

    public string m_CurrentPotatoLocaiton;

    public GameObject PotatoLocker;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);


        DontDestroyOnLoad(this);


        m_CurrentCanvasStates = GameStates.OUTISDE;
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
                CanvasManager.Instance.UpdateGameState(i_State);
                break;
            case GameStates.ROOM:
                CanvasManager.Instance.UpdateGameState(i_State);
                break;
            case GameStates.PLAY:
                CanvasManager.Instance.UpdateGameState(i_State);
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

    public void DropPotato()
    {
        PotatoLocker.SetActive(false);
    }
    public void LockPotato()
    {
        PotatoLocker.SetActive(true);
    }
}
