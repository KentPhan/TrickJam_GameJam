using System;
using TMPro;
using UnityEngine;
using WebSocketSharp;


public class CanvasManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI m_ConnectionText;


    [SerializeField] private TextMeshProUGUI m_RoomNameValue;


    public static CanvasManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);


        DontDestroyOnLoad(this);

        //m_CurrentCanvasState = CanvasState.LOBBY;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetConnectionText(string i_text)
    {
        m_ConnectionText.text = i_text;
    }


    public void UpdateGameState(GameStates i_newstate)
    {
        switch (i_newstate)
        {
            case GameStates.OUTISDE:
                break;
            case GameStates.ROOM:
                break;
            case GameStates.PLAY:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(i_newstate), i_newstate, null);
        }
    }

    public void CreateRoom()
    {
        if (m_RoomNameValue.text.Trim().IsNullOrEmpty())
        {
            Debug.Log("Empty String Not Accepted");
            return;
        }

        m_RoomNameValue.text = m_RoomNameValue.text.Trim().ToUpper();

        if (NetworkManagerScript.Instance.CreateRoom(m_RoomNameValue.text))
        {
            Debug.Log("Create Room Button" + m_RoomNameValue.text);
            return;
        }

        Debug.Log("Failed To Create Room Button" + m_RoomNameValue.text);
        return;
    }

    public void JoinRoom()
    {
        if (m_RoomNameValue.text.Trim().IsNullOrEmpty())
        {
            Debug.Log("Empty String Not Accepted");
            return;
        }

        m_RoomNameValue.text = m_RoomNameValue.text.Trim().ToUpper();

        if (NetworkManagerScript.Instance.JoinRoom(m_RoomNameValue.text))
        {
            Debug.Log("Join Room Button" + m_RoomNameValue.text);
            return;
        }

        Debug.Log("Failed To Join Room Button" + m_RoomNameValue.text);
        return;
    }


}
