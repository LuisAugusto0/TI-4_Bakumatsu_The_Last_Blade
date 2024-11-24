using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBaseActionPrefabFactory<T> : MonoBehaviour
{}

public abstract class IActionPrefabFactory<T> : IBaseActionPrefabFactory<T>
{
    public abstract IManagedAction GetManagedAction(T target);
}

public abstract class ICancellableActionPrefabFactory<T> : IBaseActionPrefabFactory<T>
{
    public abstract IManagedAction GetManagedAction(T target, OnActionEnded fallback);
}
