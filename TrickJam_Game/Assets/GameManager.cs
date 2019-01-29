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
}
