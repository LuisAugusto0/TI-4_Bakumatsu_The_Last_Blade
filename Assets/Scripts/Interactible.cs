using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))] // as trigger
public class OnTriggerInteractible : MonoBehaviour
{
    [Serializable]
    public class InteractEvent : UnityEvent<GameObject> { }
    public InteractEvent onInteractEvent;
    bool destroyOnInteract = true;

    public LayerMask layerMask;

    [Tooltip("If Tag is None, use layer filtering; otherwise use tag filtering")]
    public Tag expectedTag = Tag.None;

    void OnTriggerEnter2D(Collider2D collider)
    {
        int layer = collider.gameObject.layer;

        if (expectedTag == Tag.None)
        {
            if ((layerMask & (1 << layer)) != 0)
            {
                Interact(collider.gameObject);
            }
        }
        else if (collider.gameObject.tag == expectedTag.ToString())
        {
            Interact(collider.gameObject);
        }
    }

    void Interact(GameObject target)
    {
        onInteractEvent.Invoke(target);

        if (destroyOnInteract)
        {
            Destroy(this.gameObject);
        }
    }
}
