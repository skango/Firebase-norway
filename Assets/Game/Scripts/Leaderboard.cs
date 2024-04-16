using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    
    private List<Player> players = new List<Player>();
    public List<Text> PlayerTexts = new List<Text>();
    public List<Sprite> avatarsprites = new List<Sprite>();
    public List<Image> userProfiles = new List<Image>();

    void Start()
    {        
        LoadPlayersFromDatabase();
    }

    async void LoadPlayersFromDatabase()
    {
       
       var task = await AccountSystem.instance.FetchTopPlayersAsync();
       for (int i = 0; i < task.Count; i++)
       {
            PlayerTexts[i].text = task[i].Username;
            userProfiles[i].sprite = avatarsprites[i];
            Debug.Log(task[i].Username);
       }
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
