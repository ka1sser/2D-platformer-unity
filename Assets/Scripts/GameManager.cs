using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Makes this a singleton

    public Player player; // reference to the Player

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
