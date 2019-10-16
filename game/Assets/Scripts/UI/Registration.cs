using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class Registration : MonoBehaviour
{
    public TextMeshProUGUI firstNameField;
    public TextMeshProUGUI lastNameField;
    public TextMeshProUGUI usernameField;
    public TextMeshProUGUI passwordField;
    public TextMeshProUGUI emailField;

    public Button registerButton;

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        // make a call to a url
        
        UnityWebRequest www = new UnityWebRequest("http://localhost/sqlconnect/register.php");

        // wait until you get the info back
        yield return www;

        if (!www.isNetworkError && !www.isHttpError)
        {
            // no errors and it worked
            Debug.Log("User created successfully.");

        }
        else
        {
            // there were errors
            Debug.Log("User creation failed. Error #" + www.error);
        }
    }

    public void VerifyInputs()
    {
        registerButton.interactable = (firstNameField.text.Length >= 2 && passwordField.text.Length >= 8);
    }

}
