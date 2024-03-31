using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Avatar : MonoBehaviour
{
    public GameObject avatar;
    bool active = false;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            OutlineTag[] outlines = FindObjectsOfType<OutlineTag>(true);
            
            for (int i = 0; i < outlines.Length; i++)
            {
                outlines[i].gameObject.SetActive(false);
                if (AccountSystem.instance.avatars[i] == this)
                {
                    AccountSystem.instance.SetAvatarValue(i);
                    AccountSystem.instance.avatarSprite = GetComponent<Image>().sprite;
                }
            }
            active = !active;
            avatar.SetActive(active);
        });
    }

    public void SelectAvatar()
    {
        OutlineTag[] outlines = FindObjectsOfType<OutlineTag>(true);
        
        for (int i = 0; i < outlines.Length; i++)
        {
            outlines[i].gameObject.SetActive(false);
        }
        active = !active;
        avatar.SetActive(active);
        AccountSystem.instance.avatarSprite = GetComponent<Image>().sprite;
    }
}
