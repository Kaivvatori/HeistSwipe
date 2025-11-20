using UnityEngine;

public class ExitScript : MonoBehaviour
{
    [SerializeField] private GameObject UIHandler;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponentInChildren<DiamondScript>())
            {
                UIHandler.GetComponent<MainMenuController>().GameWinMenu(true);
                collision.GetComponent<PlayerMovement>().StopMoving();
            }
        }
    }
}
