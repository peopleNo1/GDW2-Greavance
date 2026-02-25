using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ability : MonoBehaviour
{
    public abstract IEnumerator Execute();

    public virtual bool CanUse()
    {
        return true;
    }
}