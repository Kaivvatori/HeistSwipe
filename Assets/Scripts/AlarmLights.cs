using UnityEngine;

public class AlarmLights : MonoBehaviour
{
    private GameObject alarmObject;
    private GameObject electricityBox;
    private ElectricityBox electricityBoxScript;
    private Alarm alarmScript;
    private Color defaultColor;
    private SpriteRenderer spriteRenderer;
    private bool isElectricBoxActive;

    private Color lerpedcolor;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        electricityBox = GameObject.FindGameObjectWithTag("ElectricityBox");
        electricityBoxScript = electricityBox.GetComponent<ElectricityBox>();
        alarmObject = GameObject.FindGameObjectWithTag("Alarm");
        alarmScript = alarmObject.GetComponent<Alarm>();
        defaultColor = spriteRenderer.color;
    }
    void Update()
    {
        isElectricBoxActive = electricityBoxScript.GetIsElectricBoxActive();
        if (isElectricBoxActive)
        {
            if(alarmScript.isAlarm)
            {
                lerpedcolor = Color.Lerp(defaultColor, Color.red, Mathf.PingPong(Time.time, 1));
                spriteRenderer.color = lerpedcolor;
            }
            else 
            {
                lerpedcolor = Color.Lerp(defaultColor, Color.gray, Mathf.PingPong(Time.time/4f, 1));
                spriteRenderer.color = lerpedcolor;
            }
        }
        else if(!isElectricBoxActive)
        {
            spriteRenderer.color = Color.black;
        }
    }
}
