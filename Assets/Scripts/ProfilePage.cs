using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePage : MonoBehaviour
{
    public TMP_Text UsernameText, UsernameText2,Score;
    public Image avatar;

    void Start()
    {
        Score.text = PlayerPrefs.GetInt("Score").ToString();
        AccountSystem.instance.StartCoroutine(AccountSystem.instance.GetAvatar());
        UsernameText.text = UsernameText2.text = AccountSystem.instance.GetUsername();       
    }


    public void UpdateUsernames(string name)
    {
        UsernameText.text = UsernameText2.text = name;
    }
    private void Update()
    {
        if (AccountSystem.instance.avatarSprite == null)
        {
            return;
        }
        avatar.sprite = AccountSystem.instance.avatarSprite;
    }
}
