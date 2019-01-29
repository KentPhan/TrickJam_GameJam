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


    public void OnTriggerEnter2D(Collider2D i_collider)
    {
        if (i_collider.CompareTag("PotatoTriggerZone"))
        {
            GameManager.Instance.LockPotato();
            //Debug.Log(i_collider.gameObject.tag);
        }

    }
}
