using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Editor
{
    internal static class CaptainMainElements
    {
        public static VisualElement CreateUserBadge(MessageSender sender)
        {
            VisualElement elem = new VisualElement();
            elem.style.flexDirection = FlexDirection.Row;
            elem.style.alignItems = Align.FlexEnd;

            Image usrIcon = new Image();
            usrIcon.image = LoadIcon("Assets/Editor/Icons/user.png");
            usrIcon.style.width = 10;
            usrIcon.style.height = 10;
            usrIcon.style.marginRight = 2;
            usrIcon.style.marginBottom = 2;
            elem.Add(usrIcon);

            Label usrLabel = new Label();
            usrLabel.text = sender == MessageSender.OPENAI ? "Captain" : "User";
            usrLabel.style.color = new Color(0.8f, 0.8f, 0.8f);
            usrLabel.style.fontSize = 10;

            elem.Add(usrLabel);
            elem.style.opacity = 0.5f;

            return elem;
        }

        public static Label CreateMsgLabel(string msg)
        {
            Label l = new Label(msg);
            l.style.whiteSpace = WhiteSpace.Normal;
            return l;
        }

        private static Texture2D LoadIcon(string path)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
    }
}
