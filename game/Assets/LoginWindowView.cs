﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Mirror;

#if GOOGLEGAMES
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class LoginWindowView : MonoBehaviour
{
    public bool ClearPlayerPrefs;

    public TextMeshProUGUI emailField;
    public TextMeshProUGUI passwordField;
    public TextMeshProUGUI confirmPasswordField;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI registerInfoText;

    public BasicAuth basicAuth;

    public Button loginButton;
    public Button loginWithGoogle;
    public Button registerButton;
    public Button cancelRegisterButton;
    public Toggle rememberMe;

    public GameObject registerPanel;
    public GameObject mainPanel;

    public GetPlayerCombinedInfoRequestParams InfoRequestParams;

    // Reference to our Authentication service
    private PlayFabAuthService _AuthService = PlayFabAuthService.Instance;

    public void Awake()
    {

#if GOOGLEGAMES
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .RequestServerAuthCode(false)
            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();
#endif


        if (ClearPlayerPrefs)
        {
            _AuthService.UnlinkSilentAuth();
            _AuthService.ClearRememberMe();
            _AuthService.AuthType = Authtypes.None;
        }

        //Set our remember me button to our remembered state.
        rememberMe.isOn = _AuthService.RememberMe;

        //Subscribe to our Remember Me toggle
        rememberMe.onValueChanged.AddListener((toggle) =>
        {
            _AuthService.RememberMe = toggle;
        });
    }

    void Start()
    {
        mainPanel.SetActive(true);
        registerPanel.SetActive(false);

        // subscribe to events that happen after we authenticate
        // we are telling the delegate in PlayFabAuthService to call this method
        PlayFabAuthService.OnDisplayAuthentication += OnDisplayAuthentication;
        PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;
        PlayFabAuthService.OnPlayFabError += OnPlayFaberror;

        loginButton.onClick.AddListener(OnLoginClicked);
        loginWithGoogle.onClick.AddListener(OnLoginWithGoogleClicked);
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        cancelRegisterButton.onClick.AddListener(OnCancelRegisterButtonClicked);

        _AuthService.InfoRequestParams = InfoRequestParams;

        _AuthService.Authenticate();
    }

    private void OnCancelRegisterButtonClicked()
    {
        //Reset all forms
        emailField.text = string.Empty;
        passwordField.text = string.Empty;
        confirmPasswordField.text = string.Empty;
        //Show panels
        registerPanel.SetActive(false);
    }

    private void OnRegisterButtonClicked()
    {
        if (passwordField.text != confirmPasswordField.text)
        {
            infoText.text = "Passwords do not match.";
            return;
        }

        //_AuthService.displayName = usernameField.text;
        _AuthService.Email = emailField.text;
        _AuthService.Password = passwordField.text;
        _AuthService.Authenticate(Authtypes.RegisterPlayFabAccount);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.LogFormat("Logged In as: {0}", result.PlayFabId);

        //SceneLoader.Load(SceneLoader.Scene.Menu);

        basicAuth.username = result.PlayFabId;
        basicAuth.password = result.SessionTicket;
        NetworkManager.singleton.StartClient();
    }

    private void OnPlayFaberror(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
            case PlayFabErrorCode.InvalidPassword:
            case PlayFabErrorCode.InvalidEmailOrPassword:
                Debug.Log("Invalid email or password");
                break;
            case PlayFabErrorCode.AccountNotFound:
                registerPanel.SetActive(true);
                return;
            default:
                Debug.Log(error.GenerateErrorReport());
                break;

        }

        Debug.Log(error.Error);
    }

    private void OnLoginWithGoogleClicked()
    {
        // using unity social platform
        Social.localUser.Authenticate((success) =>
        {
            if (success)
            {
                // will get the sever auth code
                // need to take it and send it to playfab to auth
#if GOOGLEGAMES
                var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                _AuthService.AuthTicket = serverAuthCode;
                _AuthService.Authenticate(Authtypes.Google);
#endif
            }
        });
    }

    private void OnLoginClicked()
    {
        infoText.text = string.Format("Logging in as {0} ...", emailField.text);

        _AuthService.Email = emailField.text;
        _AuthService.Password = passwordField.text;
        _AuthService.Authenticate(Authtypes.EmailAndPassword);
    }

    /// <summary>
    /// Choose to display the Auth UI or any other action.
    /// </summary>
    private void OnDisplayAuthentication()
    {

//#if FACEBOOK
//        if (FB.IsInitialized)
//        {
//            Debug.LogFormat("FB is Init: AccessToken:{0} IsLoggedIn:{1}",AccessToken.CurrentAccessToken.TokenString, FB.IsLoggedIn);
//            if (AccessToken.CurrentAccessToken == null || !FB.IsLoggedIn)
//            {
//                Panel.SetActive(true);
//            }
//        }
//        else
//        {
//            Panel.SetActive(true);
//            Debug.Log("FB Not Init");
//        }
//#else
        //Here we have choses what to do when AuthType is None.
        mainPanel.SetActive(true);
//#endif
        /*
         * Optionally we could Not do the above and force login silently
         * 
         * _AuthService.Authenticate(Authtypes.Silent);
         * 
         * This example, would auto log them in by device ID and they would
         * never see any UI for Authentication.
         * 
         */
    }

    /// <summary>
    /// Play As a guest, which means they are going to silently authenticate
    /// by device ID or Custom ID
    /// </summary>

}
