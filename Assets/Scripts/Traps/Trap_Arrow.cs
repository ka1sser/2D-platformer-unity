using Unity.VisualScripting;
using UnityEngine;

public class Trap_Arrow : Trap_Trampoline
{
    [Header("Additional info")]
    [SerializeField] private float cooldown;
    [SerializeField] private float rotationSpeed = 120;
    [SerializeField] private bool rotationRight;
    private int direction = -1;

    [Space]
    [SerializeField] private float scaleUpSpeed = 10;
    [SerializeField] private Vector3 targetScale;

    private void Start()
    {
        transform.localScale = new Vector3(.3f, .3f, .3f);
    }

    private void Update()
    {
        HandleScale();
        HandleRotation();
    }

    private void HandleScale()
    {
        if (transform.localScale.x < targetScale.x)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleUpSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        direction = rotationRight ? -1 : 1;

        transform.Rotate(0, 0, rotationSpeed * direction * Time.deltaTime);
    }

    private void DestroyMe()
    {
        GameObject arrowPrefab = GameManager.instance.arrowPrefab;
        GameManager.instance.CreateObject(arrowPrefab, transform, cooldown);

        Destroy(gameObject);
    }
}
