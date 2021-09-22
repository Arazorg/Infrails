using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateable 
{
    void Tick(); //I would call it update, but this way it doesn't collide with the unity message
}
