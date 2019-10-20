using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    public delegate void ChatSendEvent(string msg);
    public static event ChatSendEvent OnChatSend;

    public string message;
    public Button sendButton;
    public InputField chatInputBox;

    public void SendButtonPressed()
    {
        if (chatInputBox.text != null)
        {
            // has a message
            message = chatInputBox.text;
            OnChatSend.Invoke(message);
        }
    }
}
