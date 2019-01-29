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
    GOTPOTATO = 1
}


public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{

    public static NetworkManager Instance;

    private ClientType m_ClientType = ClientType.NONE;


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
    }

    // Update is called once per frame
    void Update()
    {
        CanvasManager.Instance.SetConnectionText(PhotonNetwork.NetworkClientState.ToString());
        CanvasManager.Instance.SetUserIDText(PhotonNetwork.LocalPlayer.UserId);
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
        //PhotonNetwork.ReconnectAndRejoin();
    }


    #region Events



    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {

        }
    }

    public void OnReceivePotato(EventData i_photonEvent)
    {
        object[] l_data = (object[])i_photonEvent.CustomData;
        int l_dataRatio = (int)l_data[0];
        //MobileCanvasManager.Instance.UpdatePowerBar(l_dataRatio);
    }

    public void OnReceiveGameState(EventData i_photonEvent)
    {
        object[] l_data = (object[])i_photonEvent.CustomData;
        int l_dataRatio = (int)l_data[0];
        //MobileCanvasManager.Instance.UpdatePowerBar(l_dataRatio);
    }

    #endregion


    #region RaiseEvents

    public void SendPotato(int i_ID)
    {
        object[] l_content = new object[] { i_ID };
        RaiseEventOptions l_eventOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        SendOptions l_sendOptions = new SendOptions() { Reliability = true };
        //PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.FLASH_LIGHT_TOGGLE, l_content, l_eventOptions, l_sendOptions);
    }

    #endregion


    #region Accessors


    public ClientType GetClientType()
    {
        return m_ClientType;
    }

    #endregion
}


