using System;
using TMPro;
using UnityEngine;




public class CanvasManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI m_ConnectionText;




    public static CanvasManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);


        DontDestroyOnLoad(this);

        m_CurrentCanvasState = CanvasState.LOBBY;
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


    public void UpdateCanvasState(CanvasState i_newstate)
    {
        switch (i_newstate)
        {
            case CanvasState.LOBBY:
                break;
            case CanvasState.INGAME:

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(i_newstate), i_newstate, null);
        }

        m_CurrentCanvasState = i_newstate;
    }
}
