using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    private Dictionary<string, Text> leaderboardTexts = new Dictionary<string, Text>(); 
    private List<Player> players = new List<Player>();

    void Start()
    {
        
        LoadPlayersFromDatabase();
    }

    void LoadPlayersFromDatabase()
    {
       
    }

   
}

public class Player
{
    public string Name { get; set; }
    public int Score { get; set; }

    public Player(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
