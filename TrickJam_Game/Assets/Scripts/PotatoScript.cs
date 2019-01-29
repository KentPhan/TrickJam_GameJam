using UnityEngine;

public class PotatoScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



    }


    private bool AlreadyTrigered = false;

    public void OnTriggerEnter2D(Collider2D i_collider)
    {
        if (i_collider.CompareTag("PotatoTriggerZone") && !AlreadyTrigered)
        {
            AlreadyTrigered = true;
            GameManager.Instance.LockPotato();
            GameManager.Instance.SendPotato();

            //Debug.Log(i_collider.gameObject.tag);
        }

    }

    public void OnTriggerExit2D(Collider2D i_collider)
    {
        if (i_collider.CompareTag("PotatoTriggerZone"))
        {
            AlreadyTrigered = false;
            //Debug.Log(i_collider.gameObject.tag);
        }

    }
}
