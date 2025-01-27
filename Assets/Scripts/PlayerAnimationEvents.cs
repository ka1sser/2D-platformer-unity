using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void FinishedRespawn() => player.RespawnFinish(true);
}
