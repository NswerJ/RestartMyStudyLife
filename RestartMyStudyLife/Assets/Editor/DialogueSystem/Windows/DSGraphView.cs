using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddGridBackGround();
            AddStyles();
        }

       

        private void AddGridBackGround()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyle.uss");

            styleSheets.Add(styleSheet);
        }

    }
}


