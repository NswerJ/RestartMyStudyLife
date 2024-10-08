using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RL.Core.Editors
{
    public class RMSL_GraphBaseWindow<T> : EditorWindow where T : RMSL_BaseGraphView, new()
    {
        protected T graphView;
        protected Toolbar toolbar;

        protected virtual void CreateGraph()
        {

            graphView = new T();
            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);

        }

        protected Toolbar AddToolBar()
        {

            var toolbar = new Toolbar();
            rootVisualElement.Add(toolbar);

            this.toolbar = toolbar;

            return toolbar;

        }

        protected virtual void OnEnable()
        {

            CreateGraph();

        }

    }

}
