using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public struct ChatMessageNew
{
    public string sender;
    public string messsage;
}

class SnycListChatMessagesNew : SyncList<ChatMessageNew> { }
public class ChatHandler : NetworkBehaviour
{
    const short chatMsg = 1000;
    NetworkClient _client;

    [SerializeField]
    private Text chatline;

    readonly SnycListChatMessagesNew chatMessages = new SnycListChatMessagesNew();

    [SerializeField]
    private InputField input;

    public void SendMessage()
    {
        ChatMessageNew message = new ChatMessageNew
        {
            sender = NetworkClient.connection.identity.netId.ToString(),
            messsage = input.text
        };

        //OnServerPostChatMessage(message);
        CmdSendMessageToServer(message);
        //chatMessages.Add(message);
    }

    public override void OnStartClient()
    {
        chatMessages.Callback += OnChatUpdated;
    }

    private void OnChatUpdated(SyncList<ChatMessageNew>.Operation op, int itemIndex, ChatMessageNew item)
    {
        chatline.text += "[" + item.sender + "] - " + item.messsage + "\n";
    }

    [Client]
    public void PostChatMessage(string message)
    {
        if (message.Length == 0) return;
        var msg = new StringMessage(message);

        input.text = "";
        input.ActivateInputField();
        input.Select();
    }

    [Command]
    void CmdSendMessageToServer(ChatMessageNew message)
    {
        OnServerPostChatMessage(message);
    }

    [Server]
    void OnServerPostChatMessage(ChatMessageNew message)
    {
        chatMessages.Add(message);

    }

}
