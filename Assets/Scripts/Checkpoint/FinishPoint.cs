using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
 
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            anim.SetTrigger("activated");
            Debug.Log("You completed the level!!!");
        }
    }
}
