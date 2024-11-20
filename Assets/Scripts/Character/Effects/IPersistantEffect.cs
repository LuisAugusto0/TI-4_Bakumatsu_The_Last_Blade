using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/* Defines effect that persists until End() is called
 *
 * Start() must be safeguarded to not allow consecutive adds
 * Increase in values must only be done using Update from UpdatableEffect<T>
 */
public interface IPersistantEffect : IEffect {
    public void Start();
    public void End();
    public bool IsActive();
}


