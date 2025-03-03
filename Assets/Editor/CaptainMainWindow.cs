using System;
using System.Threading.Tasks;
using Assets.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

using static Assets.Editor.CaptainMainElements;
using Image = UnityEngine.UIElements.Image;
enum MessageSender
{
    OPENAI, USER
}

public class CaptainMainWindow : EditorWindow
{
    // file patching
    private string fileContent;
    private string fileTitle;
    private Label fileNameLabel;
    private Image fileStatusIcon;

    // chat view
    private ScrollView historyChatView;

    // chat send
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

        ScrollView chatInputScrollView = new ScrollView();
        chatInputScrollView.style.flexGrow = 1;
        chatInputScrollView.style.height = 50;
        chatInputContainer.Add(chatInputScrollView);

        chatInputField = new TextField();
        chatInputField.style.flexGrow = 1;
        chatInputField.style.minHeight = 82;
        chatInputField.multiline = true;
        chatInputScrollView.Add(chatInputField);

        VisualElement buttonContainer = new VisualElement();
        buttonContainer.style.flexDirection = FlexDirection.Row;
        buttonContainer.style.justifyContent = Justify.FlexStart;
        buttonContainer.style.alignItems = Align.Center;
        chatInputContainer.Add(buttonContainer);

        sendButton = new UnityEngine.UIElements.Button();
        sendButton.text = "Send";
        sendButton.style.flexGrow = 0;
        sendButton.style.maxWidth = 120;
        sendButton.style.paddingLeft = 15;
        sendButton.style.paddingRight = 15;
        sendButton.clicked += OnSendButtonClicked;
        buttonContainer.Add(sendButton);

        UnityEngine.UIElements.Button filePickerButton = new UnityEngine.UIElements.Button();
        filePickerButton.text = "Provide a schema file";
        filePickerButton.style.flexGrow = 0;
        filePickerButton.style.maxWidth = 150;
        filePickerButton.style.paddingLeft = 15;
        filePickerButton.style.paddingRight = 15;
        filePickerButton.clicked += OnFilePickerButtonClicked;
        buttonContainer.Add(filePickerButton);

        fileStatusIcon = new Image();
        fileStatusIcon.image = LoadIcon("Assets/Editor/Icons/gray-dot.png");
        fileStatusIcon.style.width = 6;
        fileStatusIcon.style.height = 6;
        fileStatusIcon.style.marginLeft = 4;
        fileStatusIcon.style.marginTop = 2;
        fileStatusIcon.style.marginRight = 2;
        fileStatusIcon.style.marginBottom = 2;
        buttonContainer.Add(fileStatusIcon);

        fileNameLabel = new Label();
        fileNameLabel.text = "no file selected";
        fileNameLabel.style.opacity = 0.5f;
        buttonContainer.Add(fileNameLabel);
    }

    private async void OnSendButtonClicked()
    {
        string message = chatInputField.value;
        if (!string.IsNullOrEmpty(message))
        {
            sendButton.SetEnabled(false);
            sendButton.text = "Please wait...";
            AddMessage(message, MessageSender.USER);

            if (fileContent != null)
            {
                var invokeMessage = InvokeFormat.MergeSchemaMessage(fileContent, message);
                var response = await CaptainProxy.InvokeCaptainAskAsync(invokeMessage);
                AddMessage(response.Content, MessageSender.OPENAI);
            }
            else
            {
                var response = await CaptainProxy.InvokeCaptainAskAsync(message);
                AddMessage(response.Content, MessageSender.OPENAI);
            }

            sendButton.SetEnabled(true);
            sendButton.text = "Send";
        };
    }
    private void OnFilePickerButtonClicked()
    {
        string path = EditorUtility.OpenFilePanel("Select File", "", "txt");
        if (!string.IsNullOrEmpty(path))
        {
            fileTitle = System.IO.Path.GetFileName(path);
            fileContent = System.IO.File.ReadAllText(path);

            if (fileContent.Length > 3000)
            {
                Debug.Log("the schema file provided was too large, please check if selected file is correct.");
                return;
            }

            fileNameLabel.text = fileTitle;
            fileNameLabel.style.opacity = 1;
            fileStatusIcon.image = LoadIcon("Assets/Editor/Icons/green-dot.png");
        }
    }

    private async void AddMessage(string msg, MessageSender sender)
    {
        VisualElement lineContainer = new VisualElement();
        lineContainer.style.flexDirection = FlexDirection.Row;
        lineContainer.style.justifyContent = sender == MessageSender.OPENAI ? Justify.FlexStart : Justify.FlexEnd;
        historyChatView.Add(lineContainer);

        VisualElement Bobble = CaptainMainElements.CreateBobble(sender);

        var userBadge = CaptainMainElements.CreateUserBadge(sender);
        var msgLabel = CaptainMainElements.CreateMsgLabel(msg);

        Bobble.Add(userBadge);
        Bobble.Add(msgLabel);
        lineContainer.Add(Bobble);
        chatInputField.value = string.Empty;

        // label animate
        await Task.Delay(33);
        if (sender == MessageSender.OPENAI)
        {
            Bobble.experimental.animation.Start(new StyleValues { opacity = 1, marginLeft = 5 }, 500);
        }
        else
        {
            Bobble.experimental.animation.Start(new StyleValues { opacity = 1, marginRight = 5 }, 500);
        }
    }

    private static Texture2D LoadIcon(string path)
    {
        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
