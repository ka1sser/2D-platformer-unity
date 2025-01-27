using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Makes this a singleton
    
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay;
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

    public void RespawnPlayer() => StartCoroutine(RespawnCoroutine());

    private IEnumerator RespawnCoroutine() 
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
        
    }

    public void AddFruit() => fruitsCollected++;

    public bool FruitsHaveRandomLook() => fruitsHaveRandomLook;
}
