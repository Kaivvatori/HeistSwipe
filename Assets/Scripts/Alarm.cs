using UnityEngine;

public class Alarm : MonoBehaviour
{
    public bool isAlarm { get; private set; }
    public bool isChase { get; private set; }
    private float chaseCooldown = 0f;
    [SerializeField] private float initialCooldown = 30f;
    private float cooldown = 0f;
    private Color lerpedColor;
    private Color defaultColor;
    [SerializeField] private GameObject blackBackground;
    [SerializeField] private GameObject electricityBox;
    private ElectricityBox electricityBoxScript;
    private SpriteRenderer blackSprite;

    private GameObject[] Securities;
    private GameObject Security;

    public void SoundAlarm(Vector3 Pos)
    {
        isAlarm = true;
        cooldown = initialCooldown;
        Security = GetClosestHelp(Securities);
        Security.GetComponent<SecurityMovement>().CallForAlarm(Pos);
    }
    public void EndAlarm()
    {
        isAlarm = false;
    }
    void Start()
    {
        Securities = GameObject.FindGameObjectsWithTag("Security");
        electricityBoxScript = electricityBox.GetComponent<ElectricityBox>();
        cooldown = 0f;
        isAlarm = false;
        blackSprite = blackBackground.GetComponent<SpriteRenderer>();
        defaultColor = blackSprite.color;
    }
    void Update()
    {
        CheckChase();
        CheckCooldown();
        
    }
    void CheckChase()
    {
        if(chaseCooldown > 0)
        {
            lerpedColor = Color.Lerp(defaultColor, new Color(0.48f,0.147f,0.147f,0.85f), Mathf.PingPong(Time.time, 1));
            blackSprite.color = lerpedColor;
            chaseCooldown -= Time.deltaTime;
        }
        else if(chaseCooldown <= 0)
        {
            blackSprite.color = defaultColor;
            EndChase();
        }
    }
    public void ChaseStart()
    {
        chaseCooldown = 15f;
        isChase = true;
    }
    public void EndChase()
    {
        isChase = false;
    }
    void CheckCooldown()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else if(cooldown <= 0 || !electricityBoxScript.GetIsElectricBoxActive())
        {
            if(isAlarm)
            {
                EndAlarm();
            }
        }
    }
    GameObject GetClosestHelp(GameObject[] Securities)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in Securities)
        {
        float dist = Vector3.Distance(t.transform.position, currentPos);
        if (dist < minDist)
        {
            tMin = t;
            minDist = dist;
        }
        }
        return tMin;
    }
}
