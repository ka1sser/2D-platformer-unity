using UnityEngine;

public class Trap_Trampoline : MonoBehaviour
{
    [SerializeField] private float duration = .5f;
    [SerializeField] private float pushPower;

    protected Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.Push(transform.up * pushPower, duration);
            anim.SetTrigger("activate");
        }
    }
}
