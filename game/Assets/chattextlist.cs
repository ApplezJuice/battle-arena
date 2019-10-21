using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

public class chattextlist : NetworkBehaviour
{
    public GameObject chatMessagePrefab;
    public Transform chatContent;

    readonly SnycListChatMessages chatMessages = new SnycListChatMessages();

    private void Start()
    {
        chatMessages.Callback += OnUpdateChat;
    }

    private void OnUpdateChat(SyncList<ChatMessage>.Operation op, int itemIndex, ChatMessage item)
    {
        GameObject newMessage = Instantiate(chatMessagePrefab, chatContent);
        Text content = newMessage.GetComponent<Text>();

        content.text = string.Format(content.text, item.sender, item.messsage);
    }
}
