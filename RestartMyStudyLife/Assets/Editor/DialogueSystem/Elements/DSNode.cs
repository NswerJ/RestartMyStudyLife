using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

namespace DS.Elements
{
    using Enumerations;

    public class DSNode : Node
    {
        public string DialorueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }

        public void Init()
        {
            DialorueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue text";
        }

        public void Draw()
        {
            /*Title Container*/
            TextField dialogueNameTextField = new TextField()
            {
                value = DialorueName
            };
            titleContainer.Insert(0, dialogueNameTextField);



            /*Input Container*/
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            inputPort.name = "Dialogue Connection";

            inputContainer.Add(inputPort);

            /*Extension Container*/

            VisualElement customDataContainer = new VisualElement() ;

            Foldout textFoldout = new Foldout()
            {
                text = "Dialogue Text"

            };

            TextField textField = new TextField()
            {
                value = Text
            };

            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }
    }
}




