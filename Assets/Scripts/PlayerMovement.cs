using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    private Sprite playerHandIdle;
    private Sprite playerHandGrab;
    [SerializeField] private GameObject PlayerFov;
    private FieldOfView playerFieldOfView;
    [SerializeField] private GameObject PlayerHand;
    [SerializeField] private float PlayerHandRadius = 2;
    [SerializeField] private Camera mainCam;
    private bool isDragging = false;
    private bool isHoldingSomething;
    private Rigidbody2D rb2D;
    private PlayerHandScript playerHandScript;

    [SerializeField] private float viewDistance;
    [SerializeField] private float viewAngle;

    [SerializeField]
    private float distance;
    private bool stopMovement;

    [SerializeField] private GameObject UIHandler;
    private MainMenuController mainMenuController;


    void Start()
    {
        playerFieldOfView = PlayerFov.GetComponent<FieldOfView>();
        playerHandScript = PlayerHand.GetComponent<PlayerHandScript>();
        playerHandIdle = playerHandScript.playerHandIdle;
        playerHandGrab = playerHandScript.playerHandGrab;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        playerFieldOfView.SetValues(viewDistance, viewAngle);
        mainMenuController = UIHandler.GetComponent<MainMenuController>();
    }


    void Update()
    {
        if (stopMovement)
        {
            return;
        }
        Vector3 playerPos = transform.position;
        Vector3 playerHandPos = PlayerHand.transform.position;
        playerFieldOfView.SetOrigin(transform.position);
        playerFieldOfView.SetAimDirection((playerHandPos - transform.position).normalized);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool _isPaused = mainMenuController.isPaused;
            if (_isPaused)
            {
                mainMenuController.ResumeGame();
            }
            else
            {
                mainMenuController.PauseGame();
            }
        }
        isHoldingSomething = playerHandScript.isHoldingSomething;
        float dist = Vector2.Distance(playerPos, playerHandPos);
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        if (!isHoldingSomething)
        {
            if (Input.GetMouseButtonDown(0) && !isDragging && dist >= distance)
            {
                PlayerHand.GetComponent<SpriteRenderer>().sprite = playerHandGrab;
                isDragging = true;
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                PlayerHand.GetComponent<SpriteRenderer>().sprite = playerHandIdle;
                isDragging = false;
            }
        }
        float speed = 0.05f;
        if (dist < distance) speed = 0f;
        if (!isDragging)
        {
            PlayerHand.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - playerHandPos);
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, playerHandPos - playerPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed);
        }
    }

    void FixedUpdate()
    {
        if (stopMovement)
        {
            return;
        }
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        float dist = Vector2.Distance(mousePos, transform.position);

        if (!isDragging)
        {
            Vector2 directionVector = (mousePos - playerPos).normalized * PlayerHandRadius;
            Vector2 newPlayerHandPos = directionVector + playerPos;

            if (dist <= PlayerHandRadius)
            {
                PlayerHand.transform.position = Vector2.Lerp(PlayerHand.transform.position, mousePos, 0.1f);
            }
            else
            {
                PlayerHand.transform.position = Vector2.Lerp(PlayerHand.transform.position, newPlayerHandPos, 0.1f);
            }
        }


        else if (isDragging)
        {
            Vector2 playerHandPos = PlayerHand.transform.position;
            Vector2 mouseDelta = playerHandPos - mousePos;
            Vector2 directionVector = mouseDelta.normalized * PlayerHandRadius;
            dist = Vector2.Distance(PlayerHand.transform.position, transform.position);
            if (dist < PlayerHandRadius)
            {
                //rb2D.MovePosition(rb2D.position + mouseDelta * Time.fixedDeltaTime);
                rb2D.AddForce(mouseDelta * Time.fixedDeltaTime * 150);
            }
            else
            {
                Vector2 newPlayerPos = directionVector + playerHandPos;
                Vector2 newPos = newPlayerPos - rb2D.position;
                //rb2D.MovePosition(rb2D.position + newPos * Time.fixedDeltaTime);
                rb2D.AddForce(newPos * Time.fixedDeltaTime * 150);
            }
        }
    }
    public void GotCaught()
    {
        UIHandler.GetComponent<MainMenuController>().GameOverMenu(true);
        StopMoving();
    }
    public void StopMoving()
    {
        stopMovement = true;
    }



}
