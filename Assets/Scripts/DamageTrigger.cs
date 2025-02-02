using Unity.VisualScripting;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.KnockBack(transform.position.x);
        }
    }
}
