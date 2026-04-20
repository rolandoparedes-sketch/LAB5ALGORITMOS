using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Enemy> enemigos;
    public static GameManager instance;
    public TextMeshProUGUI turnoText;
    public CustomDoubleLinkedList snapshotSystem = new();

    public Player player;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        enemigos = new List<Enemy>(FindObjectsByType<Enemy>(FindObjectsSortMode.None));
        SaveTurn();
        snapshotSystem.ResetPointer();
        LoadTurn();
    }
    [Button]
    public void SaveTurn()
    {
        snapshotSystem.SaveTurn();
        Debug.Log("Saving turn: " + snapshotSystem.Count);
    }
    //[Button]
    public void LoadTurn()
    {
        snapshotSystem.LoadTurn(player);
        MoverEnemigos();
    }
    [Button]
    public void NextTurn()
    {
        snapshotSystem.MoveForward();
        LoadTurn();
    }
    [Button]
    public void PrevTurn()
    {
        snapshotSystem.MoveBackwards();
        LoadTurn();
    }

    [Button]
    public void ExitWithoutChanges()
    {
        snapshotSystem.GoToLast();   
        snapshotSystem.LoadTurn(player); 
    }
    public void MoverEnemigos()
    {
        foreach (var e in enemigos)
        {
            e.Actuar(player.transform);
        }
    }
}