using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRPSable
{
    Sprite GetMatchSprite();
    void ShowRPSResult();
    RPS_CHOISE GetRPSChoice();
    void EnterRPSMode();
    void ExitRPSMode();
}
