using System.Collections;
using System.Collections.Generic;
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

    private PotatoGrabState m_CurrentPotatoGrabState = PotatoGrabState.YOUAINTGOTSHIT;

    // Start is called before the first frame update
    void Start()
    {

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
                        l_Target.transform.SetParent(this.transform);
                    }
                }
            }
            else if (m_CurrentPotatoGrabState == PotatoGrabState.GOTIT)
            {
                if (!Input.GetMouseButton(0))
                {
                    Debug.Log("Running");
                    m_CurrentPotatoGrabState = PotatoGrabState.YOUAINTGOTSHIT;
                    if (this.transform.childCount > 0)
                    {
                        this.transform.DetachChildren();
                    }
                }
            }
        }
        else if (m_InputType == InputType.TOUCH)
        {
            if (Input.touchCount <= 0)
                return;


            Touch l_touch = Input.GetTouch(1);

            if (l_touch.phase == TouchPhase.Began)
            {
                Raycast(Input.GetTouch(0).position);
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
