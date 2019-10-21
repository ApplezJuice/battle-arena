using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public struct ChatMessage
{
    public string sender;
    public string messsage;
}

class SnycListChatMessages : SyncList<ChatMessage> { }
public class ChatManager : NetworkBehaviour
{
    public Transform chatContent;
    public GameObject chatMessagePrefab;
    public InputField inputField;

    public GameObject chatPanel;

    public Button send;

    public Text chatText;

    readonly SnycListChatMessages chatMessages = new SnycListChatMessages();

    [Command]
    public void CmdNewMessage(NetworkIdentity networkIdentity, string msg)
    {
        ChatMessage message = new ChatMessage
        {
            sender = networkIdentity.ToString(),
            messsage = msg
        };


        //chatMessages.Add(message);
        OnAddChatMessage(message);
        //RpcChatMessage(message);
    }

    [ClientRpc]
    void RpcUpdateChat()
    {
        chatText.text = "";
        foreach (ChatMessage msg in chatMessages)
        {
            chatText.text += "[" + msg.sender + "] - " + msg.messsage + "\n";
        }
    }

    [Command]
    void CmdCallUpdateChat()
    {
        RpcUpdateChat();
    }

    [Server]
    void OnAddChatMessage(ChatMessage message)
    {
        chatMessages.Add(message);
    }

    private void Start()
    {
        //if (!isLocalPlayer && !isServer)
        //{
        //    return;
        //}
        chatMessages.Callback += OnUpdateChat;
        //InitializeLobbyCanvas();
    }

    private void OnUpdateChat(SyncList<ChatMessage>.Operation op, int itemIndex, ChatMessage item)
    {
        switch (op)
        {
            case SnycListChatMessages.Operation.OP_ADD:
                // index is where it got added in the list
                // item is the new item


                //GameObject newMessage = Instantiate(chatMessagePrefab, chatContent);
                //Text content = newMessage.GetComponent<Text>();

                //content.text = string.Format(content.text, item.sender, item.messsage);
                Debug.Log("Message from: " + item.sender + " | Message: " + item.messsage);
                chatText.text = "";
                foreach (ChatMessage msg in chatMessages)
                {
                    chatText.text += "[" + msg.sender + "] - " + msg.messsage + "\n";
                }
                //CmdCallUpdateChat();

                break;
            case SnycListChatMessages.Operation.OP_CLEAR:
                // list got cleared
                break;
            case SnycListChatMessages.Operation.OP_INSERT:
                // index is where it got added in the list
                // item is the new item
                break;
            case SnycListChatMessages.Operation.OP_REMOVE:
                // index is where it got removed in the list
                // item is the item that was removed
                break;
            case SnycListChatMessages.Operation.OP_REMOVEAT:
                // index is where it got removed in the list
                // item is the item that was removed
                break;
            case SnycListChatMessages.Operation.OP_SET:
                // index is the index of the item that was updated
                // item is the previous item
                break;
            case SnycListChatMessages.Operation.OP_DIRTY:
                // index is the index of the item that was updated
                // item is the previous item
                break;
        }
    }

    //public void InitializeLobbyCanvas()
    //{
    //    chatContent = GameObject.Find("Content").transform;
    //    inputField = GameObject.Find("ChatInput").GetComponent<InputField>();
    //    chatPanel = GameObject.Find("Chat");
    //    send = GameObject.Find("Send").GetComponent<Button>();
    //    chatText = GameObject.Find("ChatText").GetComponent<Text>();

    //    send.onClick.AddListener(ChatSendButtonPressed);

    //    chatText.text = "";
    //    foreach (ChatMessage msg in chatMessages)
    //    {
    //        chatText.text += "[" + msg.sender + "] - " + msg.messsage + "\n";
    //    }
    //    //chatPanel.SetActive(false);
    //    //GameObject.Find("SceneManager").GetComponent<SceneButtons>().HideChat();
    //}

    public void ChatSendButtonPressed()
    {
        string msg = inputField.text;
        if (msg != "" || msg != null || msg != " ")
        {
            NetworkIdentity identity = NetworkClient.connection.identity;
            CmdNewMessage(identity, msg);
        }
        inputField.text = "";
    }

    [ClientRpc]
    void RpcChatMessage(ChatMessage message)
    {
        GameObject newMessage = Instantiate(chatMessagePrefab, chatContent);
        Text content = newMessage.GetComponent<Text>();

        content.text = string.Format(content.text, message.sender, message.messsage);
    }
}
