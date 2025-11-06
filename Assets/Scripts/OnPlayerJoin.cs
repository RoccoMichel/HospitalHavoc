using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnPlayerJoin : MonoBehaviour
{
    public static OnPlayerJoin intance;

    public List<PlayerController> players;

    public void PlayerJoined(PlayerInput obj)
    {
        obj.gameObject.GetComponent<PlayerController>().playerInt = obj.playerIndex + 1;
        obj.gameObject.GetComponent<PlayerController>().SetPlayerModel();

        players.Add(obj.gameObject.GetComponent<PlayerController>());
    }

    public void OnPlayerLeave(PlayerInput obj)
    {
        for (int i = obj.playerIndex + 1; i < players.Count; i--)
        {
            players[i].playerInt--;
        }

        players.RemoveAt(obj.playerIndex);
    }

    void Awake()
    {
        intance = this;
    }
}