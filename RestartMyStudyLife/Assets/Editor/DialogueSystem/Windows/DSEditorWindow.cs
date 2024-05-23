using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("DS/Dialogue Graph")]
        public static void ShowExample()
        {
           GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
          

            AddGraphView();

            AddStyles();
        }

       

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSVariables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }

        private void AddGraphView()
        {

            DSGraphView graphView = new DSGraphView();
            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
    }
}

