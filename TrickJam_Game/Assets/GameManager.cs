using UnityEngine;


public enum GameState
{
    OUTISDE,
    ROOM,
    PLAY

}


public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private GameState m_CurrentCanvasState;



    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);


        DontDestroyOnLoad(this);


        m_CurrentCanvasState = GameState.OUTISDE;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
