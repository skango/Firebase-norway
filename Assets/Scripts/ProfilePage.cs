using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePage : MonoBehaviour
{
    public TMP_Text UsernameText,UsernameText2;
    public Image avatar;

    void Start()
    {
        UsernameText.text = UsernameText2.text = AccountSystem.instance.GetUsername();
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
