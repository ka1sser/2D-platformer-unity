using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Makes this a singleton

    public Player player; // reference to the Player
    

    [Header("Fruits Management")]
    public bool fruitsHaveRandomLook;
    public int fruitsCollected;

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

    public void AddFruit() => fruitsCollected++;

    public bool FruitsHaveRandomLook() => fruitsHaveRandomLook;
}
