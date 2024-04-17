using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settiings : MonoBehaviour
{
    private int Question1Rate,Question2Rate;
    private int StarRate;
    public List<Image> Stars = new List<Image>();
    public TMP_InputField comment1, comment2;

    public void rateq1(int rating)
    {
        Question1Rate = rating;
    }

    public void rateq2(int rating)
    {
        Question2Rate = rating;
    }

    public void RateStars(int rate)
    {
        for (int i = 0; i < Stars.Count; i++)
        {
            if (i <= rate)
            {
                Stars[i].color = Color.yellow;
            }else
            {
                Stars[i].color = Color.white;
            }
        }
        StarRate = rate;
    }

    public void RatePage1()
    {
        AccountSystem.instance.SetRating1(Question1Rate);
        AccountSystem.instance.SetRating2(Question2Rate);
        AccountSystem.instance.SetRatingStar(StarRate);
    }

    public void RatePage2()
    {
        AccountSystem.instance.SetComment1(comment1.text);
        AccountSystem.instance.SetComment2(comment2.text);
    }
}
