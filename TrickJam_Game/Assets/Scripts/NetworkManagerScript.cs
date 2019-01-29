using UnityEngine;


public enum ClientType
{
    NONE,
    HOST,
    USER
}


public class NetworkManagerScript : MonoBehaviour
{

    public static NetworkManagerScript Instance;

    private ClientType m_ClientType = ClientType.NONE;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
