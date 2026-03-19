using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ability : MonoBehaviour
{
    protected PlayerController _playerController;

    protected virtual void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    
    public abstract IEnumerator Execute();

    public virtual bool CanUse()
    {
        return true;
    }
}