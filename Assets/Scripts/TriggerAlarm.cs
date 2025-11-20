using UnityEngine;

public class TriggerAlarm : MonoBehaviour
{
    private GameObject alarmObject;
    private Alarm alarmScript;
    void Start()
    {
        alarmObject = GameObject.FindGameObjectWithTag("Alarm");
        alarmScript = alarmObject.GetComponent<Alarm>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Player"))
        {
            alarmScript.SoundAlarm(transform.position);
        }
    }
}
