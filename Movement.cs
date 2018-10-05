using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour {

    private int lockCount;
    private bool _isLocked;
    public bool isLocked
    {
        get
        {
            return _isLocked;
        }
    }
    
    //Pass a duration to automatically unlock after a certain amount of time
    //If you dont pass a duration, you should call removeLock manually
    public void addLock(float duration = 0)
    {
        lockCount++;
        _isLocked = true;

        if (duration > 0)
            Invoke("removeLock", duration);
    }

    public void removeLock()
    {

        if (--lockCount == 0)
        {
            _isLocked = false;
        }
    }
}
