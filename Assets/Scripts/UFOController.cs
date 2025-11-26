using UnityEngine;

public class UFOController : MonoBehaviour
{
    [Header("Configuración UFO")]
    public float normalSpeed = 3f;
    public float magnetAttractionSpeed = 8f;
    
    private Rigidbody2D rb;
    private Transform magnetTarget;
    private bool isAttracted = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Movimiento inicial de izquierda a derecha
        rb.linearVelocity = new Vector2(-normalSpeed, 0);
    }
    
    void Update()
    {
        if (isAttracted && magnetTarget != null)
        {
            MoveTowardsMagnet();
        }
    }
    
    public void SetAttractedToMagnet(Transform magnet)
    {
        if (!isAttracted)
        {
            magnetTarget = magnet;
            isAttracted = true;
            Debug.Log("✓ UFO siendo atraído hacia el imán");
        }
    }
    
    private void MoveTowardsMagnet()
    {
        Vector2 direction = (magnetTarget.position - transform.position).normalized;
        rb.linearVelocity = direction * magnetAttractionSpeed;
    }
    
    public bool IsAttracted()
    {
        return isAttracted;
    }
    
    private void OnDestroy()
    {
        // Limpiar al ser destruido
        isAttracted = false;
        magnetTarget = null;
    }
}