using UnityEngine;

public class DiamondScript : MonoBehaviour
{
[SerializeField] private GameObject Holder;
   public void Grab(Transform playerHand)
   {
        transform.parent = playerHand;
        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
        transform.localScale = Holder.transform.localScale;
   }
   public void Drop(Transform Player)
   {
        transform.parent = Player;
        transform.position = Holder.transform.position;
        transform.rotation = Holder.transform.rotation;
        transform.localScale = Holder.transform.localScale;
   }
}
