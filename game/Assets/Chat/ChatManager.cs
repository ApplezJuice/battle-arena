using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class ChatManager : NetworkBehaviour
{
    public Transform chatContent;
    public GameObject chatMessagePrefab;
    public InputField inputField;

    public GameObject chatPanel;

    public Button send;

    private void Start()
    {
        InitializeLobbyCanvas();
        //SceneManager.sceneLoaded += OnSceneLoaded;

        //SceneButtons.OnOpenChatBoxDelegate += InitializeLobbyCanvas;
    }

    //private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    //{
    //    if (arg0.buildIndex == 1)
    //    {
    //        InitializeLobbyCanvas();
    //    }
    //}

    private void Update()
    {
        //if (chatPanel == null || send == null || chatContent == null)
        //{
        //    InitializeLobbyCanvas();
        //}
    }

    private void InitializeLobbyCanvas()
    {
        chatContent = GameObject.Find("Content").transform;
        inputField = GameObject.Find("ChatInput").GetComponent<InputField>();
        chatPanel = GameObject.Find("Chat");
        send = GameObject.Find("Send").GetComponent<Button>();

        send.onClick.AddListener(ChatSendButtonPressed);

        //chatPanel.SetActive(false);
        GameObject.Find("SceneManager").GetComponent<SceneButtons>().HideChat();
    }

    public void ValueChanged()
    {
        if (inputField.text.Contains("\n"))
        {
            UserSendingChat(inputField.text);
            Debug.Log(inputField.text);
        }
    }

    public void ChatSendButtonPressed()
    {
        string msg = inputField.text;
        Debug.Log("Message: " + msg);
        CmdSendChatMessage("sendbutton", msg);
    }

    private void UserSendingChat(string msg)
    {
        if (!isServer)
        {
            CmdSendChatMessage("ApplezTest", "test");
        }
    }

    [Command]
    void CmdSendChatMessage(string username, string msg)
    {
        RpcChatMessage("ApplezTest", msg);
    }

    [ClientRpc]
    void RpcChatMessage(string username, string msg)
    {
        if (string.IsNullOrEmpty(msg))
        {
            // The message was empty, so do not display the message.
            return;
        }

        GameObject newMessage = Instantiate(chatMessagePrefab, chatContent);
        Text content = newMessage.GetComponent<Text>();

        content.text = string.Format(content.text, username, msg);
    }
}
