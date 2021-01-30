using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LinkPoint : MonoBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void Disable()
    {
        _collider.enabled = false;
    }
    
    public void Enable()
    {
        _collider.enabled = true;
    }
}
