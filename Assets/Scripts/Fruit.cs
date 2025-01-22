using UnityEngine;

public enum FruitType {Apple, Banana, Cherry, Kiwi, Melon, Orange, Pineapple, Strawberry}

public class Fruit : MonoBehaviour
{
    private GameManager gameManager;
    private Animator anim;

    [SerializeField] private FruitType fruitType;
    [SerializeField] private GameObject pickupVFX;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        SetRandomLookIfNeeded();    
    }

    private void SetRandomLookIfNeeded()
    {
        if(gameManager.FruitsHaveRandomLook() == false)
        {
            UpdateFruitVisuals();
            return;
        }

        int randomIndex = Random.Range(0,8);
        anim.SetFloat("fruitIndex", randomIndex);
    }

    private void UpdateFruitVisuals()
    {
        anim.SetFloat("fruitIndex", (int)fruitType);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            gameManager.AddFruit();
            Destroy(gameObject);

            GameObject newFx = Instantiate(pickupVFX, transform.position, Quaternion.identity);
            Destroy(newFx, 0.5f);
        }
    }
}
