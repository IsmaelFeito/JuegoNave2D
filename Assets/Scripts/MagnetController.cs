using UnityEngine;

public class MagnetController : MonoBehaviour
{
    [Header("Configuración del Imán")]
    public float magnetStrength = 8f;
    public float magnetRadius = 3f;
    public bool magnetActive = false;
    
    private CircleCollider2D magnetCollider;
    
    void Start()
    {
        magnetCollider = GetComponent<CircleCollider2D>();
        if (magnetCollider != null)
        {
            magnetCollider.radius = magnetRadius;
            magnetCollider.isTrigger = true;
        }
        SetMagnetActive(false);
    }
    
    void Update()
    {
        // Seguir a la nave (padre)
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
        }
    }
    
    public void SetMagnetActive(bool active)
    {
        magnetActive = active;
        if (magnetCollider != null)
        {
            magnetCollider.enabled = active;
        }
        Debug.Log($"Imán {(active ? "ACTIVADO" : "DESACTIVADO")}. Collider: {magnetCollider.enabled}");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!magnetActive) return;
        
        Debug.Log($"Imán detectó: {other.gameObject.name} con tag: {other.tag}");
        
        if (other.CompareTag("UFO"))
        {
            UFOController ufo = other.GetComponent<UFOController>();
            if (ufo != null)
            {
                ufo.SetAttractedToMagnet(transform);
                Debug.Log("✓ UFO atrapado por el imán!");
            }
            else
            {
                Debug.LogError("✗ El UFO no tiene componente UFOController");
            }
        }
    }
}