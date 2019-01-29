using UnityEngine;

public enum InputType
{
    MOUSE,
    TOUCH
}

public enum PotatoGrabState
{
    YOUAINTGOTSHIT,
    GOTIT,
}

public class PlayerScript : MonoBehaviour
{


    [SerializeField]
    private InputType m_InputType = InputType.MOUSE;
    [SerializeField]
    private float m_MinDragSpeed = 10.0f;
    [SerializeField]
    private float m_MaxDragSpeed = 30.0f;
    [SerializeField]
    private float m_DragDistanceLimit = 100.0f;

    private PotatoGrabState m_CurrentPotatoGrabState = PotatoGrabState.YOUAINTGOTSHIT;
    private GameObject m_PotatoLocked;

    // Start is called before the first frame update
    void Start()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {


        if (m_InputType == InputType.MOUSE)
        {
            Vector3 l_point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(l_point.x, l_point.y, transform.position.z);


            if (m_CurrentPotatoGrabState == PotatoGrabState.YOUAINTGOTSHIT)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject l_Target = Raycast(Input.mousePosition);
                    if (l_Target)
                    {
                        m_CurrentPotatoGrabState = PotatoGrabState.GOTIT;
                        m_PotatoLocked = l_Target;
                    }
                }
            }
            else if (m_CurrentPotatoGrabState == PotatoGrabState.GOTIT)
            {
                if (!Input.GetMouseButton(0))
                {
                    m_CurrentPotatoGrabState = PotatoGrabState.YOUAINTGOTSHIT;
                }
                else
                {
                    //GameObject l_Target = Raycast(Input.mousePosition);

                    Vector2 l_DistanceVector = (transform.position - m_PotatoLocked.transform.position);
                    Vector2 l_Direction = l_DistanceVector.normalized;
                    float l_Ratio = Mathf.Floor(Mathf.Clamp(l_DistanceVector.magnitude, 0.0f, m_DragDistanceLimit)) / m_DragDistanceLimit;

                    float l_Speed = Mathf.Floor(Mathf.Lerp(m_MinDragSpeed, m_MaxDragSpeed, l_Ratio));
                    //Debug.Log(l_Ratio + " " + l_DistanceVector.magnitude + " " + l_Speed);
                    m_PotatoLocked.GetComponent<Rigidbody2D>().velocity = l_Direction * l_Speed;
                }
            }
        }
        else if (m_InputType == InputType.TOUCH)
        {
            if (Input.touchCount <= 0)
                return;


            Touch l_touch = Input.GetTouch(1);

            if (l_touch.phase == TouchPhase.Began || l_touch.phase == TouchPhase.Moved)
            {
                Vector3 l_point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                transform.position = new Vector3(l_point.x, l_point.y, transform.position.z);
                //Raycast(Input.GetTouch(0).position);
            }

            // GARARGEAREGARGE
            if (m_CurrentPotatoGrabState == PotatoGrabState.YOUAINTGOTSHIT)
            {
                if (l_touch.phase == TouchPhase.Began)
                {
                    GameObject l_Target = Raycast(Input.GetTouch(0).position);
                    if (l_Target)
                    {
                        m_CurrentPotatoGrabState = PotatoGrabState.GOTIT;
                        m_PotatoLocked = l_Target;
                    }
                }
            }
            else if (m_CurrentPotatoGrabState == PotatoGrabState.GOTIT)
            {
                if (l_touch.phase == TouchPhase.Ended || l_touch.phase == TouchPhase.Canceled)
                {
                    m_CurrentPotatoGrabState = PotatoGrabState.YOUAINTGOTSHIT;
                }
                else if (l_touch.phase == TouchPhase.Moved)
                {
                    //GameObject l_Target = Raycast(Input.mousePosition);

                    Vector2 l_DistanceVector = (transform.position - m_PotatoLocked.transform.position);
                    Vector2 l_Direction = l_DistanceVector.normalized;
                    float l_Ratio = Mathf.Floor(Mathf.Clamp(l_DistanceVector.magnitude, 0.0f, m_DragDistanceLimit)) / m_DragDistanceLimit;

                    float l_Speed = Mathf.Floor(Mathf.Lerp(m_MinDragSpeed, m_MaxDragSpeed, l_Ratio));
                    //Debug.Log(l_Ratio + " " + l_DistanceVector.magnitude + " " + l_Speed);
                    m_PotatoLocked.GetComponent<Rigidbody2D>().velocity = l_Direction * l_Speed;
                }
            }
        }
    }


    private GameObject Raycast(Vector2 i_ScreenCoordinates)
    {
        RaycastHit2D hitInfo =
            Physics2D.Raycast(Camera.main.ScreenToWorldPoint(i_ScreenCoordinates), Vector2.zero);
        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.gameObject.name);
            return hitInfo.transform.gameObject;
        }
        return null;
    }
}
