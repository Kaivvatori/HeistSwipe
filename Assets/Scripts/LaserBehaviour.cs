using System.Collections;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField]
    private float laserFrequency = 2f;
    [SerializeField]
    private float delay = 0;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private GameObject electricityBox;
    private ElectricityBox electricityBoxScript;
    [SerializeField]
    private float multipliedFrequency;
    private float defaultFrequency;
    private float Multiplier;
    private bool isElectricBoxActive;
    private bool isGeneratorActive;

    Coroutine lastCoroutine = null;
    void Start()
    { 
        electricityBox = GameObject.FindGameObjectWithTag("ElectricityBox");
        electricityBoxScript = electricityBox.GetComponent<ElectricityBox>();
        defaultFrequency = laserFrequency;
        multipliedFrequency = laserFrequency/Multiplier;
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        isElectricBoxActive = electricityBoxScript.GetIsElectricBoxActive();
        if(isElectricBoxActive && !isActive)
        {
            isActive = true;
            lastCoroutine = StartCoroutine(TurnOnOff());
        }
        if(!isElectricBoxActive && isActive)
        {
            isActive = false;
            if(lastCoroutine != null){
            StopCoroutine(lastCoroutine);
            }
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
        }
    }

    private IEnumerator TurnOnOff(){
        yield return new WaitForSeconds(delay);
        while(isActive)
        {
            laserFrequency = defaultFrequency;
            yield return new WaitForSeconds(laserFrequency);
            boxCollider2D.enabled = !boxCollider2D.enabled;
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
        
    }
}
