using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandScript : MonoBehaviour
{
[SerializeField] public Sprite playerHandIdle;
[SerializeField] public Sprite playerHandGrab;
[SerializeField] private GameObject Flashlight;
private FlashlightScript flashLightScript;
private DiamondScript diamondScript;
[SerializeField] private GameObject Player;
private SpriteRenderer spriteRenderer;
public bool isHoldingSomething = false;
    private Color defaultcolor;
    private bool collectedDiamond;

private string collisionTag;
private string collisionName;
private string holdingItemTag;

public List<String> interractableTags = new List<string>();

    void Start()
    {
        flashLightScript = Flashlight.GetComponent<FlashlightScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultcolor = spriteRenderer.color;
        isHoldingSomething = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        collisionTag = collision.gameObject.tag;
        if(collisionTag == "Player" && isHoldingSomething)
        {
            spriteRenderer.color = Color.red;
            // DROPPING
            if(Input.GetMouseButtonDown(0))
            {
                if(holdingItemTag == "Flashlight")
                {
                    flashLightScript.Drop(Player.transform);
                    spriteRenderer.sprite = playerHandIdle;
                    spriteRenderer.color = defaultcolor;
                    isHoldingSomething = false;
                }
                if(holdingItemTag == "Diamond" && diamondScript)
                {
                    diamondScript.Drop(Player.transform);
                    collectedDiamond = true;
                    spriteRenderer.sprite = playerHandIdle;
                    spriteRenderer.color = defaultcolor;
                    isHoldingSomething = false;
                }
            }
        }
        if(interractableTags.Contains(collisionTag) && !isHoldingSomething)
        {// GRABBING
            spriteRenderer.color = Color.green;
            if(Input.GetMouseButtonDown(0))
            {
                switch (collisionTag)
                {
                    case "Flashlight":
                        flashLightScript.Grab(gameObject.transform);
                        spriteRenderer.sprite = playerHandGrab;
                        spriteRenderer.color = defaultcolor;
                        isHoldingSomething = true;
                        holdingItemTag = "Flashlight"; 
                    break;
                    case "Diamond":
                        if (collectedDiamond)
                        {
                            break;
                        }
                        diamondScript = collision.gameObject.GetComponent<DiamondScript>();
                        diamondScript.Grab(gameObject.transform);
                        spriteRenderer.sprite = playerHandGrab;
                        spriteRenderer.color = defaultcolor;
                        isHoldingSomething = true;
                        holdingItemTag = "Diamond";  
                    break;
                    case "SecurityCamera":
                        collision.gameObject.GetComponent<SecurityCameraBehaviour>().TurnOnOff();
                    break;
                    case "ElectricityBox":
                        collision.gameObject.GetComponent<ElectricityBox>().TurnOnOff();
                    break;
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        spriteRenderer.color = defaultcolor;
    }


}
