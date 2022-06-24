using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Please make sure "Main" is excuted before every custom script but after "GameManager"
public class Main : MonoBehaviour
{
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    protected virtual void Awake(){
        GameManager.mainCam = Camera.main;
        GameManager.player1 = player1;
        GameManager.player2 = player2;
    }
}
