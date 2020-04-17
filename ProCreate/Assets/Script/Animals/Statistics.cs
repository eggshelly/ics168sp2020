using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface used to see if the object has statistics to display over the gameobject in the scene
//Used on AnimalStatistics
public interface Statistics
{
    void DisplayStatistics();

    void HideStatistics();
}
