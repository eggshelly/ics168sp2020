using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Crossable
{
    bool IsOpen();

    void ChangeCrossableState();
}
