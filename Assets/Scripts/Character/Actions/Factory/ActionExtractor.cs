using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActionExtractor<T> : MonoBehaviour
{ }

public abstract class ActionExtractor<T> : BaseActionExtractor<T>
{
    public abstract IManagedAction GetManagedAction(T target);
}

public abstract class CancellableActionExtractor<T> : BaseActionExtractor<T>
{
    public abstract IManagedAction InitializeManagedAction(T target, OnActionEnded fallback);
}
