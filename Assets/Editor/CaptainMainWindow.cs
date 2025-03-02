using System.Threading.Tasks;
using Assets.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

using static Assets.Editor.CaptainMainElements;
enum MessageSender
{
    OPENAI, USER
}

public class CaptainMainWindow : EditorWindow
{
    private ScrollView historyChatView;
    private TextField chatInputField;
    private UnityEngine.UIElements.Button sendButton;

    [MenuItem("Tools/Unity Captain Toolset")]
    public static void ShowCaptainToolset()
    {
        CaptainMainWindow wnd = GetWindow<CaptainMainWindow>();
        wnd.titleContent = new GUIContent(" Unity Captain", LoadIcon("Assets/Editor/Icons/captain-icon.png"));
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        CreateChatView();
        CreateChatInputContainer();
    }

    void CreateChatView()
    {
        historyChatView = new ScrollView();
        historyChatView.style.flexGrow = 3;
        rootVisualElement.Add(historyChatView);
    }

    void CreateChatInputContainer()
    {
        VisualElement chatInputContainer = new VisualElement();
        chatInputContainer.style.flexDirection = FlexDirection.Column;
        chatInputContainer.style.flexGrow = 0;
        chatInputContainer.style.height = 120;
        chatInputContainer.style.paddingTop = 5;
        chatInputContainer.style.paddingBottom = 5;
        rootVisualElement.Add(chatInputContainer);

        chatInputField = new TextField();
        chatInputField.style.flexGrow = 1;
        chatInputField.style.minHeight = 50;
        chatInputField.style.maxHeight = 100;
        chatInputField.multiline = true;
        chatInputContainer.Add(chatInputField);

        sendButton = new UnityEngine.UIElements.Button();
        sendButton.text = "Send to captain!";
        sendButton.style.flexGrow = 0;
        sendButton.style.maxWidth = 120;
        sendButton.style.paddingLeft = 15;
        sendButton.style.paddingRight = 15;
        sendButton.clicked += OnSendButtonClicked;
        chatInputContainer.Add(sendButton);
    }

    private async void OnSendButtonClicked()
    {
        string message = chatInputField.value;
        if (!string.IsNullOrEmpty(message))
        {
            sendButton.SetEnabled(false);
            sendButton.text = "Please wait...";
            AddMessage(message, MessageSender.USER);

            var response = await CaptainProxy.InvokeCaptainAskAsync(message);
            AddMessage(response.Content, MessageSender.OPENAI);

            sendButton.SetEnabled(true);
            sendButton.text = "Send to captain!";
        };
    }

    private async void AddMessage(string msg, MessageSender sender)
    {
        VisualElement lineContainer = new VisualElement();
        lineContainer.style.flexDirection = FlexDirection.Row;
        lineContainer.style.justifyContent = sender == MessageSender.OPENAI ? Justify.FlexStart : Justify.FlexEnd;
        historyChatView.Add(lineContainer);

        VisualElement lineContent = new VisualElement();
        lineContent.style.flexDirection = FlexDirection.Column;
        
        lineContent.style.borderTopLeftRadius = 5;
        lineContent.style.borderTopRightRadius = 5;
        lineContent.style.borderBottomLeftRadius = 5;
        lineContent.style.borderBottomRightRadius = 5;
        lineContent.style.borderTopWidth = 1;
        lineContent.style.borderRightWidth = 1;
        lineContent.style.borderBottomWidth = 1;
        lineContent.style.borderLeftWidth = 1;

        Color borderColor = sender == MessageSender.OPENAI ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.5f, 0.5f, 0.5f);
        lineContent.style.borderTopColor = borderColor;
        lineContent.style.borderRightColor = borderColor;
        lineContent.style.borderBottomColor = borderColor;
        lineContent.style.borderLeftColor = borderColor;
        lineContent.style.paddingTop = 5;
        lineContent.style.paddingBottom = 5;
        lineContent.style.paddingLeft = 10;
        lineContent.style.paddingRight = 10;
        lineContent.style.maxWidth = new StyleLength(new Length(70, LengthUnit.Percent));
        lineContent.style.marginTop = 5;
        lineContent.style.marginBottom = 5;
        lineContent.style.marginLeft = 5;
        lineContent.style.marginRight = 5;
        lineContent.style.backgroundColor = sender == MessageSender.OPENAI ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.3f, 0.3f, 0.3f);
        lineContent.style.opacity = 0;
        if (sender == MessageSender.OPENAI)
        {
            lineContent.style.marginLeft = 80;
        }
        else
        {
            lineContent.style.marginRight = 80;
        }

        var userBadge = CaptainMainElements.CreateUserBadge(sender);
        var msgLabel = CaptainMainElements.CreateMsgLabel(msg);

        lineContent.Add(userBadge);
        lineContent.Add(msgLabel);
        lineContainer.Add(lineContent);
        chatInputField.value = string.Empty;

        // label animate
        await Task.Delay(33);
        if (sender == MessageSender.OPENAI)
        {
            lineContent.experimental.animation.Start(new StyleValues { opacity = 1, marginLeft = 5 }, 500);
        }
        else
        {
            lineContent.experimental.animation.Start(new StyleValues { opacity = 1, marginRight = 5 }, 500);
        }
    }


    private static Texture2D LoadIcon(string path)
    {
        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
