using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordField : MonoBehaviour
{
    private TMP_InputField field;
    
    void Start()
    {
        field = GetComponent<TMP_InputField>();
    }

    public void UpdateVisibility(bool visible)
    {
        field.contentType = visible ? TMP_InputField.ContentType.Standard 
            : TMP_InputField.ContentType.Password;
        field.ForceLabelUpdate();
    }
}
