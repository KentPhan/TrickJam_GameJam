using System;
using UnityEngine;


public enum GameStates
{
    OUTISDE,
    ROOM,
    PLAY,
    LOSE,
    WIN
}

public enum PotatoState
{
    LIMBO,
    HERE
}


public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public float m_ExplosionTimer = 5.0f;
    private float m_CurrentExplosionTimer;

    private GameStates m_CurrentGameStates;
    private PotatoState m_PotatoState;


    public string m_CurrentPotatoLocation;

    public GameObject PotatoLocker;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);


        DontDestroyOnLoad(this);


        m_CurrentGameStates = GameStates.OUTISDE;
        m_PotatoState = PotatoState.LIMBO;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentExplosionTimer = m_ExplosionTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Instance.GetClientType() == ClientType.HOST && m_CurrentGameStates == GameStates.PLAY)
        {
            if (m_CurrentExplosionTimer <= 0)
            {
                if (IsPotatoHere())
                {
                    NetworkManager.Instance.SendPotato();

                    if (!ImDead())
                        UpdateGameState(GameStates.LOSE);
                }
                else
                {
                    Debug.Log("Sent Explosion");
                    NetworkManager.Instance.SendExplosion();
                }


                m_CurrentExplosionTimer = m_ExplosionTimer;
            }


            m_CurrentExplosionTimer -= Time.deltaTime;
        }
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
            case GameStates.LOSE:
                CanvasManager.Instance.UpdateGameState(i_State);
                break;
            case GameStates.WIN:
                CanvasManager.Instance.UpdateGameState(i_State);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(i_State), i_State, null);
        }

        m_CurrentGameStates = i_State;
    }

    public void StartGame()
    {
        Debug.Log("Game Manager Start GAme");
        DropPotato();
        UpdateGameState(GameStates.PLAY);
        NetworkManager.Instance.SendState(m_CurrentGameStates);
    }

    public void DropPotato()
    {
        Debug.Log("Dropped Potato");
        PotatoLocker.SetActive(false);
        m_PotatoState = PotatoState.HERE;
    }

    public void SendPotato()
    {
        Debug.Log("Send Potato");
        m_PotatoState = PotatoState.LIMBO;
        NetworkManager.Instance.SendPotato();
    }

    public void LockPotato()
    {
        PotatoLocker.SetActive(true);
    }

    public bool IsPotatoHere()
    {
        return m_PotatoState == PotatoState.HERE;
    }

    public bool ImDead()
    {
        return m_CurrentGameStates == GameStates.LOSE;
    }
}
