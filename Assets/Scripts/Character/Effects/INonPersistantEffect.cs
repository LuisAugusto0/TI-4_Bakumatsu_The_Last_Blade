using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

/* Effects that contain a defined end timestamp (IStatusEffect<T>) 
 * or that are instanteneous
 *
 * Can always be used as an inner effect by other effects:
 *   Effects with defined end timestamp but that cannot be used as inner
 *   effects should not implement this interface
 */
public interface INonPersistantEffect : IEffect 
{
    public void Start();
}





