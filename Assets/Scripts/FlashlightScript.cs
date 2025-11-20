using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    
    [SerializeField] private GameObject Fov;
    private FieldOfView fieldOfView;
    private MeshRenderer meshRenderer;
    [SerializeField] private GameObject PlayerHand;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject FlashlightHolder;
    [SerializeField] private float viewDistance;
    [SerializeField] private float viewAngle;

    private void Start() {
        fieldOfView = Fov.GetComponent<FieldOfView>();
        meshRenderer = Fov.GetComponent<MeshRenderer>();
        fieldOfView.SetValues(viewDistance, viewAngle);
        fieldOfView.enabled = false;
    }

    void Update()
    {
        if(meshRenderer.enabled){
        Vector3 playerPos = Player.transform.position;
        Vector3 playerHandPos = PlayerHand.transform.position;
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection((playerHandPos - playerPos).normalized);
        }
    }

    public void TurnOnOff(bool val){
        meshRenderer.enabled = val;
        fieldOfView.enabled = val;
    }

    public void Grab(Transform playerHand){
        transform.parent = playerHand;
        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
        transform.localScale = FlashlightHolder.transform.localScale;
        TurnOnOff(true);
    }
    public void Drop(Transform Player){
        transform.parent = Player;
        transform.position = FlashlightHolder.transform.position;
        transform.rotation = FlashlightHolder.transform.rotation;
        transform.localScale = FlashlightHolder.transform.localScale;
        TurnOnOff(false);
    }


}
