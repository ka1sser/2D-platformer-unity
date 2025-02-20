using UnityEngine;

public class Trap_Fire_Button : MonoBehaviour
{
    private Animator anim;
    private Trap_Fire trapFire;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        trapFire = GetComponentInParent<Trap_Fire>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            anim.SetTrigger("activate");
            trapFire.SwitchOffFire();
        }
    }
}
