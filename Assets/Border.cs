using UnityEngine;

public class Border : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        Player.OnPlayerChoiceEnding += SetColliderToTrigger;
    }

    private void OnDisable()
    {
        Player.OnPlayerChoiceEnding -= SetColliderToTrigger;
    }

    private void SetColliderToTrigger()
    {
        _collider.isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player _))
            Player.RaiseBadEnding();
    }
}
