using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitchable
{
    public Transform thecurrentChar { get; }

    public void OnSwitch(bool isSwitched);

    public void IsSeperated();

    public void IsTogether();
    
}
