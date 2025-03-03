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
        public static VisualElement CreateBobble(MessageSender sender)
        {
            VisualElement Bobble = new VisualElement();
            Bobble.style.flexDirection = FlexDirection.Column;

            Bobble.style.borderTopLeftRadius = 5;
            Bobble.style.borderTopRightRadius = 5;
            Bobble.style.borderBottomLeftRadius = 5;
            Bobble.style.borderBottomRightRadius = 5;
            Bobble.style.borderTopWidth = 1;
            Bobble.style.borderRightWidth = 1;
            Bobble.style.borderBottomWidth = 1;
            Bobble.style.borderLeftWidth = 1;

            Color borderColor = sender == MessageSender.OPENAI ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.5f, 0.5f, 0.5f);
            Bobble.style.borderTopColor = borderColor;
            Bobble.style.borderRightColor = borderColor;
            Bobble.style.borderBottomColor = borderColor;
            Bobble.style.borderLeftColor = borderColor;
            Bobble.style.paddingTop = 5;
            Bobble.style.paddingBottom = 5;
            Bobble.style.paddingLeft = 10;
            Bobble.style.paddingRight = 10;
            Bobble.style.maxWidth = new StyleLength(new Length(70, LengthUnit.Percent));
            Bobble.style.marginTop = 5;
            Bobble.style.marginBottom = 5;
            Bobble.style.marginLeft = 5;
            Bobble.style.marginRight = 5;
            Bobble.style.backgroundColor = sender == MessageSender.OPENAI ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.3f, 0.3f, 0.3f);
            Bobble.style.opacity = 0;
            if (sender == MessageSender.OPENAI)
            {
                Bobble.style.marginLeft = 80;
            }
            else
            {
                Bobble.style.marginRight = 80;
            }

            return Bobble;
        }
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
