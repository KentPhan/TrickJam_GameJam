using System;
using TMPro;
using UnityEngine;


public class CanvasManager : MonoBehaviour
{
    [SerializeField] private RectTransform m_OutsideScreen;
    [SerializeField] private RectTransform m_LobbyScreen;
    [SerializeField] private RectTransform m_HostScreen;
    [SerializeField] private RectTransform m_UserScreen;

    [SerializeField] private RectTransform m_GameScreen;

    [SerializeField] private RectTransform m_LoseScreen;
    [SerializeField] private RectTransform m_WinScreen;


    [SerializeField] private TextMeshProUGUI m_ConnectionText;
    [SerializeField] private TextMeshProUGUI m_UserIDText;


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
        UpdateGameState(GameStates.OUTISDE);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetConnectionText(string i_text)
    {
        m_ConnectionText.text = i_text;
    }

    public void SetUserIDText(string i_text)
    {
        m_UserIDText.text = i_text;
    }


    public void UpdateGameState(GameStates i_newstate)
    {
        switch (i_newstate)
        {
            case GameStates.OUTISDE:
                m_OutsideScreen.gameObject.SetActive(true);
                m_LobbyScreen.gameObject.SetActive(false);
                m_GameScreen.gameObject.SetActive(false);

                m_HostScreen.gameObject.SetActive(false);
                m_UserScreen.gameObject.SetActive(false);

                m_LoseScreen.gameObject.SetActive(false);
                m_WinScreen.gameObject.SetActive(false);
                break;
            case GameStates.ROOM:
                m_OutsideScreen.gameObject.SetActive(false);
                m_LobbyScreen.gameObject.SetActive(true);
                m_GameScreen.gameObject.SetActive(false);


                switch (NetworkManager.Instance.GetClientType())
                {
                    case ClientType.NONE:
                        break;
                    case ClientType.HOST:
                        m_HostScreen.gameObject.SetActive(true);
                        m_UserScreen.gameObject.SetActive(false);
                        break;
                    case ClientType.USER:
                        m_HostScreen.gameObject.SetActive(false);
                        m_UserScreen.gameObject.SetActive(true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                m_LoseScreen.gameObject.SetActive(false);
                m_WinScreen.gameObject.SetActive(false);

                break;
            case GameStates.PLAY:
                m_OutsideScreen.gameObject.SetActive(false);
                m_LobbyScreen.gameObject.SetActive(false);
                m_GameScreen.gameObject.SetActive(true);

                m_HostScreen.gameObject.SetActive(false);
                m_UserScreen.gameObject.SetActive(false);
                m_LoseScreen.gameObject.SetActive(false);
                m_WinScreen.gameObject.SetActive(false);
                break;
            case GameStates.LOSE:
                m_OutsideScreen.gameObject.SetActive(false);
                m_LobbyScreen.gameObject.SetActive(false);
                m_GameScreen.gameObject.SetActive(false);

                m_HostScreen.gameObject.SetActive(false);
                m_UserScreen.gameObject.SetActive(false);
                m_LoseScreen.gameObject.SetActive(true);
                m_WinScreen.gameObject.SetActive(false);
                break;
            case GameStates.WIN:
                m_OutsideScreen.gameObject.SetActive(false);
                m_LobbyScreen.gameObject.SetActive(false);
                m_GameScreen.gameObject.SetActive(false);

                m_HostScreen.gameObject.SetActive(false);
                m_UserScreen.gameObject.SetActive(false);
                m_LoseScreen.gameObject.SetActive(false);
                m_WinScreen.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(i_newstate), i_newstate, null);
        }
    }

    public void CreateRoom()
    {
        if (m_RoomNameValue.text.Trim() == null || m_RoomNameValue.text.Trim() == string.Empty)
        {
            Debug.Log("Empty String Not Accepted");
            return;
        }

        m_RoomNameValue.text = m_RoomNameValue.text.Trim().ToUpper();

        if (NetworkManager.Instance.CreateRoom(m_RoomNameValue.text))
        {
            Debug.Log("Create Room Button" + m_RoomNameValue.text);
            return;
        }

        Debug.Log("Failed To Create Room Button" + m_RoomNameValue.text);
        return;
    }

    public void JoinRoom()
    {
        if (m_RoomNameValue.text.Trim() == null || m_RoomNameValue.text.Trim() == string.Empty)
        {
            Debug.Log("Empty String Not Accepted");
            return;
        }

        m_RoomNameValue.text = m_RoomNameValue.text.Trim().ToUpper();

        if (NetworkManager.Instance.JoinRoom(m_RoomNameValue.text))
        {
            Debug.Log("Join Room Button" + m_RoomNameValue.text);
            return;
        }

        Debug.Log("Failed To Join Room Button" + m_RoomNameValue.text);
        return;
    }
}
