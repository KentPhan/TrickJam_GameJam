using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public enum ClientType
{
    NONE,
    HOST,
    USER
}


public enum PhotonEventCodes
{
    GAMESTATE = 0,
    POTATOLOCATION = 1,
    POTATOEXPLOSION = 2,
    NEWHOST = 3
}


public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{

    public static NetworkManager Instance;

    private ClientType m_ClientType = ClientType.NONE;

    public float PollRate = 2.0f;
    public float m_CurrentPollRate;


    private string m_RoomName;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);

        ConnectToNetwork();
        m_CurrentPollRate = PollRate;
    }

    // Update is called once per frame
    void Update()
    {
        CanvasManager.Instance.SetConnectionText(PhotonNetwork.NetworkClientState.ToString());
        CanvasManager.Instance.SetUserIDText(PhotonNetwork.LocalPlayer.ActorNumber.ToString());


        if (PhotonNetwork.IsConnected)
        {
            if (GameManager.Instance.GameIsGoing())
            {
                if (m_CurrentPollRate <= 0)
                {
                    if (PhotonNetwork.PlayerListOthers.Length == 0)
                    {
                        GameManager.Instance.UpdateGameState(GameStates.WIN);
                    }
                    m_CurrentPollRate = PollRate;
                }


                m_CurrentPollRate -= Time.deltaTime;
            }
        }
    }

    public bool ConnectToNetwork()
    {
        Debug.Log("Connecting to Network...");
        return PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //PhotonNetwork.JoinOrCreateRoom("One", null, null);
        Debug.Log($"Connected to Master Rooms:{PhotonNetwork.CountOfRooms.ToString()}");

        //PhotonNetwork.JoinOrCreateRoom(m_RoomName, null, null);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }


    public bool CreateRoom(string i_RoomName)
    {
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToNetwork();
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            m_RoomName = i_RoomName;
            return PhotonNetwork.CreateRoom(m_RoomName);
        }

        return false;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        if (m_ClientType != ClientType.USER)
            m_ClientType = ClientType.HOST;

        GameManager.Instance.UpdateGameState(GameStates.ROOM);
        base.OnCreatedRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"Failed to Create Room {message}");
        base.OnCreateRoomFailed(returnCode, message);
    }

    public bool JoinRoom(string i_RoomName)
    {
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToNetwork();
        }


        if (PhotonNetwork.IsConnectedAndReady)
        {
            m_RoomName = i_RoomName;
            return PhotonNetwork.JoinRoom(m_RoomName);
        }

        return false;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        if (m_ClientType != ClientType.HOST)
            m_ClientType = ClientType.USER;
        GameManager.Instance.UpdateGameState(GameStates.ROOM);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Put logic for disconnecting or disconnecting
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Put logic for disconnecting or disconnecting
        //PhotonNetwork.LeaveRoom();
        //MobileGameManager.Instance.UpdateGameState(GameStates.NOT_IN_ROOM);
        base.OnPlayerLeftRoom(otherPlayer);
    }

    private void OnFailedToConnectToPhoton()
    {
        Debug.Log("Disconnected from Network...");
    }

    public override void OnDisconnected(DisconnectCause i_cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}.", i_cause);
        m_ClientType = ClientType.NONE;
        GameManager.Instance.UpdateGameState(GameStates.LOSE);
        //PhotonNetwork.ReconnectAndRejoin();
    }


    #region Events



    public void OnEvent(EventData photonEvent)
    {
        PhotonEventCodes l_Code = (PhotonEventCodes)photonEvent.Code;
        switch (l_Code)
        {
            case PhotonEventCodes.GAMESTATE:
                OnReceiveGameState(photonEvent);
                break;
            case PhotonEventCodes.POTATOLOCATION:
                OnReceivePotato(photonEvent);
                break;
            case PhotonEventCodes.POTATOEXPLOSION:
                OnReceiveExplosion(photonEvent);
                break;
            case PhotonEventCodes.NEWHOST:
                OnReceiveNewHost(photonEvent);
                break;
        }
    }

    public void OnReceiveGameState(EventData i_photonEvent)
    {
        object[] l_data = (object[])i_photonEvent.CustomData;
        GameStates l_state = (GameStates)l_data[0];
        GameManager.Instance.UpdateGameState(l_state);
    }

    public void OnReceivePotato(EventData i_photonEvent)
    {
        object[] l_data = (object[])i_photonEvent.CustomData;
        int l_actorNumber = (int)l_data[0];

        Debug.Log("Player " + l_actorNumber + " Recevied Potato");

        // If this Player
        if (l_actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (GameManager.Instance.ImDead())
            {
                SendPotato();
                return;
            }
            else
            {
                // Drop Potato
                GameManager.Instance.DropPotato();
            }
        }// Else
        else
        {
            // Do Nothing
        }

        GameManager.Instance.m_CurrentPotatoLocation = l_actorNumber;

        //MobileCanvasManager.Instance.UpdatePowerBar(l_dataRatio);
    }

    public void OnReceiveExplosion(EventData i_photonEvent)
    {
        if (GameManager.Instance.IsPotatoHere())
        {
            GameManager.Instance.UpdateGameState(GameStates.LOSE);

            if (m_ClientType == ClientType.USER)
            {

            }
            else
            {
                SendNewHost();
            }
            SendPotato();
            PhotonNetwork.LeaveRoom();
        }
        else
        {

        }
    }

    public void OnReceiveNewHost(EventData i_photonEvent)
    {
        object[] l_data = (object[])i_photonEvent.CustomData;
        int l_playerID = (int)l_data[0];

        // If this Player
        if (l_playerID == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            m_ClientType = ClientType.HOST;
        }// Else
        else
        {
            // Do Nothing
        }
    }

    #endregion


    #region RaiseEvents

    public void SendState(GameStates i_State)
    {
        object[] l_content = new object[] { i_State };
        RaiseEventOptions l_eventOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        SendOptions l_sendOptions = new SendOptions() { Reliability = true };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.GAMESTATE, l_content, l_eventOptions, l_sendOptions);
    }

    public void SendPotato()
    {
        Player[] l_Players = PhotonNetwork.PlayerListOthers;
        if (l_Players.Length <= 0)
        {
            if (PhotonNetwork.IsConnected)
                GameManager.Instance.UpdateGameState(GameStates.WIN);
            else
                GameManager.Instance.UpdateGameState(GameStates.LOSE);
            return;
        }


        Player l_Selection = l_Players[UnityEngine.Random.Range(0, l_Players.Length - 1)];
        object[] l_content = new object[] { l_Selection.ActorNumber };

        RaiseEventOptions l_eventOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        SendOptions l_sendOptions = new SendOptions() { Reliability = true };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.POTATOLOCATION, l_content, l_eventOptions, l_sendOptions);
    }

    public void SendExplosion()
    {
        object[] l_content = new object[] { };
        RaiseEventOptions l_eventOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        SendOptions l_sendOptions = new SendOptions() { Reliability = true };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.POTATOEXPLOSION, l_content, l_eventOptions, l_sendOptions);
    }

    public void SendNewHost()
    {
        Player[] l_Players = PhotonNetwork.PlayerListOthers;
        if (l_Players.Length <= 0)
        {
            if (PhotonNetwork.IsConnected)
                GameManager.Instance.UpdateGameState(GameStates.WIN);
            else
                GameManager.Instance.UpdateGameState(GameStates.LOSE);
            return;
        }


        Player l_Selection = l_Players[UnityEngine.Random.Range(0, l_Players.Length - 1)];
        object[] l_content = new object[] { l_Selection.ActorNumber };
        RaiseEventOptions l_eventOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        SendOptions l_sendOptions = new SendOptions() { Reliability = true };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.NEWHOST, l_content, l_eventOptions, l_sendOptions);
    }

    #endregion


    #region Accessors


    public ClientType GetClientType()
    {
        return m_ClientType;
    }

    #endregion
}


