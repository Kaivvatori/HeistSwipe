using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class ElectricityBox : MonoBehaviour
{

   private bool isActive = true;
   private SpriteRenderer spriteRenderer;
   private Color defaultcolor;
   private GameObject[] Securities;
   private GameObject Security;
   private int count;
   [SerializeField] private Camera cameraObj;
    public void SetPipeline(int index)
    {
        if (index >= 0 && index <= 2)
        {
            var x = cameraObj.GetUniversalAdditionalCameraData();
            x.SetRenderer(index);
        }
    }
   public bool GetIsElectricBoxActive()
   {
    return isActive;
   }
    void Start()
    {
        Securities = GameObject.FindGameObjectsWithTag("Security");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultcolor = spriteRenderer.color;
        isActive = true;
        count = 0;
    }
    public void TurnOnOff()
   {
        isActive = !isActive;
        if(!isActive)
        {
            count++;
            spriteRenderer.color = Color.red;
            CallSecurity();
            SetPipeline(1); // SET FULL_DARK
        }
        else if (isActive)
        {
            spriteRenderer.color = defaultcolor;
            if(count <= 5)
            {
            Security.GetComponent<SecurityMovement>().EndHelp();
            }
            SetPipeline(0); // SET SEMI_DARK
        }
   }
   private void CallSecurity()
   {
    Security = GetClosestHelp(Securities);
    Security.GetComponent<SecurityMovement>().CallForElectricityHelp();
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
