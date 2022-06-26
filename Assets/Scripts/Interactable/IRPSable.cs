using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRPSable
{
    Sprite GetMatchSprite();
    RPS_CHOISE GetRPSChoice();
    int GetCoins();
    void ShowRPSResult();
    void EnterRPSMode();
    void ExitRPSMode();
}
