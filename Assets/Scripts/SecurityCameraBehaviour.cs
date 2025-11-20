using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityCameraBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private float turnAmount;
    private bool Switch = true;
    [SerializeField] private GameObject SCFov;
    [SerializeField] private GameObject SCFovPos;
    private FieldOfView cameraFieldOfView;
    private SpriteRenderer spriteRenderer;
    private GameObject electricityBox;
    private ElectricityBox electricityBoxScript;
    private bool isElectricBoxActive;
    private bool isActive = true;
    private float default_z;
    private Color defaultcolor;

    [SerializeField] private float viewDistance;
    [SerializeField] private float viewAngle;
    private LayerMask layerMask;
    private GameObject alarmObject;
    private Alarm alarmScript;
    void Start()
    {
        layerMask = LayerMask.GetMask("Object", "Player");
        alarmObject = GameObject.FindGameObjectWithTag("Alarm");
        alarmScript = alarmObject.GetComponent<Alarm>();
        cameraFieldOfView = SCFov.GetComponent<FieldOfView>();
        cameraFieldOfView.SetValues(viewDistance, viewAngle);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultcolor = spriteRenderer.color;
        electricityBox = GameObject.FindGameObjectWithTag("ElectricityBox");
        electricityBoxScript = electricityBox.GetComponent<ElectricityBox>();
        default_z = transform.rotation.eulerAngles.z;
        Debug.Log(default_z);

    }
    void Update() {
        isElectricBoxActive = electricityBoxScript.GetIsElectricBoxActive();
        cameraFieldOfView.SetOrigin(SCFovPos.transform.position);
        cameraFieldOfView.SetAimDirection((SCFovPos.transform.position - transform.position).normalized);
        if(isActive)
        {

                Quaternion targetRotation = Quaternion.Euler(0,0,default_z + turnAmount);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.005f);
                if (Quaternion.Angle(transform.rotation, targetRotation) <= Math.Abs(turnAmount/6f))
                {
                turnAmount = -turnAmount;
                }
            FindTargetPlayer();
        }
        


        if(Switch)
        {
            if(isElectricBoxActive && !isActive)
            {
                isActive = true;
                SCFov.SetActive(true);
                spriteRenderer.color = defaultcolor;
            }
            else if(!isElectricBoxActive && isActive)
            {
                isActive = false;
                SCFov.SetActive(false);
                spriteRenderer.color = Color.gray;
            }
        }
        else if (!Switch && isActive)
        {
            isActive = false;
            SCFov.SetActive(false);
            spriteRenderer.color = Color.gray;
        }
    }
    public void TurnOnOff()
    {
        Switch = !Switch;
    }
    private void FindTargetPlayer()
    {
        if(Vector3.Distance(transform.position,Player.transform.position)<viewDistance)
        {
            Vector3 dirToPlayer = (Player.transform.position-transform.position).normalized;
            if(Vector3.Angle((SCFovPos.transform.position - transform.position).normalized,dirToPlayer) < viewAngle / 2f)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance, layerMask);
                if(raycastHit2D.collider != null)
                {
                    Debug.Log(raycastHit2D.collider.gameObject.name);
                    if(raycastHit2D.collider.gameObject.GetComponent<PlayerMovement>() != null)
                    {
                        alarmScript.SoundAlarm(raycastHit2D.collider.transform.position);
                    }
                }
            }
        }
    }
}
