using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using System.IO;
using System;
using RL.Dev.AI;



namespace RL.Core.Editors
{
    internal class RMSL_BehaviorTreeEditorWindow : RMSL_GraphBaseWindow<RMSL_BehaviorTreeEditorWindow.RMSL_BehaviorTreeGraphView>
    {
        private RMSL_BehaviorTreeSaveData saveData;
        private RMSL_VisualWindow inspactor;
        private VisualElement graphRoot;

        [MenuItem("RMSL/AI/BehaviorTree")]
        private static void OpenEditor()
        {

            var window = CreateWindow<RMSL_BehaviorTreeEditorWindow>();
            window.titleContent.text = "BehaviorTreeEditor";

            window.Show();
            window.maximized = true;

        }

        private void SetUpGraph()
        {

            graphView.SetDrag();
            graphView.SetGrid();
            graphView.SetZoom();
            graphView.style.position = Position.Relative;
            graphView.SetMiniMap(new Rect(10, 10, 300, 300));

        }

        private void SetUpInspacter()
        {

            inspactor = new RMSL_VisualWindow("Inspactor", Position.Relative, new Color(0.3f, 0.3f, 0.3f));

        }

        private void SetUpSplit()
        {

            var split = new TwoPaneSplitView(1, 300, TwoPaneSplitViewOrientation.Horizontal);
            split.contentContainer.Add(graphView);
            split.contentContainer.Add(inspactor);
            graphRoot.Add(split);

        }

        private void SetUpToolBar()
        {

            var saveBtn = new Button(HandleSave);
            saveBtn.text = "SaveFile";

            var loadBtn = new Button(HandleLoad);
            loadBtn.text = "LoadFile";

            toolbar.Add(saveBtn);
            toolbar.Add(loadBtn);

        }

        private void HandleSave()
        {

            if (saveData == null)
            {

                var path = EditorUtility.SaveFilePanelInProject("Save", "NewBehaviorTree", "asset", "Save");

                saveData = ScriptableObject.CreateInstance<RMSL_BehaviorTreeSaveData>();
                var bt = ScriptableObject.CreateInstance<RMSL_BehaviorTree>();
                bt.name = "AI";

                AssetDatabase.CreateAsset(saveData, path);
                AssetDatabase.AddObjectToAsset(bt, saveData);

                AssetDatabase.SaveAssets();

                graphView.AddGraphNode(new RMSL_BehaviorRootNode());
                saveData = AssetDatabase.LoadAssetAtPath<RMSL_BehaviorTreeSaveData>(path);
                saveData.behaviorTree = bt;

            }

            var nodeList = graphView.Query<RMSL_BehaviorTreeBaseNode>().ToList();

            foreach (var node in nodeList)
            {

                node.nodeObject.editorPos = node.localBound;

                var com = node.nodeObject as RMSL_CompositeNode;

                if (com != null)
                {

                    com.childrens.Clear();

                }

                if (saveData.behaviorTree.nodes.Contains(node.nodeObject)) continue;

                saveData.behaviorTree.nodes.Add(node.nodeObject);
                AssetDatabase.AddObjectToAsset(node.nodeObject, saveData.behaviorTree);
                AssetDatabase.SaveAssets();

            }

            SaveConnect();

            saveData.behaviorTree.SettingRootNode();

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(saveData.behaviorTree);

        }

        private void HandleLoad()
        {

            saveData = null;

            graphView.nodes.ToList().ForEach(node =>
            {

                graphView.RemoveElement(node);

            });

            graphView.edges.ToList().ForEach(edge =>
            {

                graphView.RemoveElement(edge);

            });

            var path = EditorUtility.OpenFilePanel("Open", Application.dataPath, "asset");
            path = path.Replace(Application.dataPath, "");
            path = path.Insert(0, "Assets");

            saveData = AssetDatabase.LoadAssetAtPath<RMSL_BehaviorTreeSaveData>(path);

            if (saveData == null) return;

            saveData.behaviorTree.nodes.ForEach((node) =>
            {

                if (node as RMSL_ActionNode)
                {

                    var obj = new RMSL_BehaviorTreeBaseNode(node.GetType(), node.GetType().Name, "ActionNode");
                    obj.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    obj.nodeObject = node;
                    obj.guid = node.guid;
                    obj.SetPosition(node.editorPos);
                    obj.OnSelectEvent += inspactor.HandleCreateInspactor;
                    graphView.AddElement(obj);

                }
                else if (node as RMSL_CompositeNode)
                {

                    var obj = new RMSL_BehaviorChildNode(node.GetType(), Port.Capacity.Multi, node.GetType().Name, "CompositeNode");
                    obj.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    obj.nodeObject = node;
                    obj.guid = node.guid;
                    obj.SetPosition(node.editorPos);
                    obj.OnSelectEvent += inspactor.HandleCreateInspactor;
                    graphView.AddElement(obj);

                }
                else if (node as RMSL_DecoratorNode)
                {

                    var obj = new RMSL_BehaviorChildNode(node.GetType(), Port.Capacity.Single, node.GetType().Name, "DecoratorNode");
                    obj.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    obj.nodeObject = node;
                    obj.guid = node.guid;
                    obj.SetPosition(node.editorPos);
                    obj.OnSelectEvent += inspactor.HandleCreateInspactor;
                    graphView.AddElement(obj);

                }
                else if (node as RMSL_RootNode)
                {

                    var obj = new RMSL_BehaviorChildNode(node.GetType(), Port.Capacity.Single, "StartPoint", "Root");
                    obj.nodeObject = node;
                    obj.guid = node.guid;
                    obj.SetPosition(node.editorPos);
                    obj.OnSelectEvent += inspactor.HandleCreateInspactor;
                    graphView.AddElement(obj);


                }

            });

            saveData.connectData.ForEach((x) =>
            {

                var inputNode = graphView.Query<RMSL_BehaviorTreeBaseNode>().ToList().Find(xx => xx.guid.ToString() == x.inputGuid);
                var outputNode = graphView.Query<RMSL_BehaviorTreeBaseNode>().ToList().Find(xx => xx.guid.ToString() == x.outputGuid);

                var edge = inputNode.inputContainer.Q<Port>().ConnectTo(outputNode.outputContainer.Q<Port>());

                graphView.AddElement(edge);

            });

        }

        private void SaveConnect()
        {

            var edgeLs = graphView.edges.ToList();
            saveData.connectData.Clear();

            foreach (var edge in edgeLs)
            {

                var inputNode = edge.input.node as RMSL_BehaviorTreeBaseNode;
                var outputNode = edge.output.node as RMSL_BehaviorTreeBaseNode;

                saveData.connectData.Add(new RMSL_ConnectData
                {

                    inputGuid = inputNode.guid.ToString(),
                    outputGuid = outputNode.guid.ToString()

                });

                ConnectNode(inputNode, outputNode);

            }

        }

        private void ConnectNode(RMSL_BehaviorTreeBaseNode input, RMSL_BehaviorTreeBaseNode output)
        {

            if (output.nodeObject as RMSL_CompositeNode != null)
            {

                var obj = output.nodeObject as RMSL_CompositeNode;

                obj.childrens.Add(input.nodeObject);
                obj.childrens = obj.childrens.OrderBy(x => x.editorPos.x).ToList();

            }
            else if (output.nodeObject as RMSL_DecoratorNode != null)
            {

                var obj = output.nodeObject as RMSL_DecoratorNode;
                obj.children = input.nodeObject;

            }
            else if (output.nodeObject as RMSL_RootNode != null)
            {


                var obj = output.nodeObject as RMSL_RootNode;
                obj.children = input.nodeObject;

            }

        }

        protected override void OnEnable()
        {

            base.OnEnable();
            AddToolBar();

            graphRoot = new VisualElement();
            graphRoot.style.flexGrow = 1;
            rootVisualElement.Add(graphRoot);

            SetUpGraph();
            SetUpInspacter();
            SetUpSplit();
            SetUpToolBar();

            graphView.Init(inspactor, HandleGraphViewChanged);

        }

        private GraphViewChange HandleGraphViewChanged(GraphViewChange graphViewChange)
        {

            if (graphViewChange.elementsToRemove != null)
            {

                foreach (var elem in graphViewChange.elementsToRemove)
                {

                    var node = elem as RMSL_BehaviorTreeBaseNode;

                    if (node != null && saveData != null)
                    {

                        if (saveData.behaviorTree.nodes.Contains(node.nodeObject))
                        {

                            saveData.behaviorTree.nodes.Remove(node.nodeObject);
                            AssetDatabase.RemoveObjectFromAsset(node.nodeObject);

                        }

                    }

                }

            }

            return graphViewChange;

        }

        #region 그래프 관련

        /// <summary>
        /// 그래프뷰
        /// </summary>
        internal class RMSL_BehaviorTreeGraphView : RMSL_BaseGraphView
        {

            private RMSL_BehaviorTreeSearchWindow searchWindow;
            private RMSL_VisualWindow inspactor;

            public RMSL_BehaviorTreeGraphView() : base()
            {

                SetSearchWindow();

            }

            private void SetSearchWindow()
            {

                searchWindow = ScriptableObject.CreateInstance<RMSL_BehaviorTreeSearchWindow>();
                searchWindow.Init(this);
                nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);

            }

            public void Init(RMSL_VisualWindow inspactor, GraphViewChanged changed)
            {

                this.inspactor = inspactor;
                graphViewChanged += changed;

            }

            public void AddGraphNode(RMSL_BehaviorTreeBaseNode node)
            {

                node.OnSelectEvent += inspactor.HandleCreateInspactor;

                AddElement(node);

            }

            public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
            {

                return ports.ToList().Where(x => x.direction != startPort.direction && x.node != startPort.node).ToList();

            }


        }

        /// <summary>
        /// 그래프 노드 생성창
        /// </summary>
        internal class RMSL_BehaviorTreeSearchWindow : ScriptableObject, ISearchWindowProvider
        {

            private RMSL_BehaviorTreeGraphView graphView;

            public void Init(RMSL_BehaviorTreeGraphView graphView)
            {

                this.graphView = graphView;

            }

            public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
            {

                var tree = new List<SearchTreeEntry>()
                {

                    new SearchTreeGroupEntry(new GUIContent("Create Behavior"), 0),
                    new SearchTreeGroupEntry(new GUIContent("Create Node"), 1),

                };

                var types = TypeCache.GetTypesDerivedFrom<RMSL_Node>();

                foreach (var t in types)
                {

                    if (t == typeof(RMSL_RootNode) || t.IsAbstract) continue;

                    tree.Add(new SearchTreeEntry(new GUIContent(t.Name))
                    {

                        userData = t,
                        level = 2

                    });

                }



                return tree;

            }

            public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
            {

                if (SearchTreeEntry.userData as Type != null)
                {

                    var type = SearchTreeEntry.userData as Type;

                    if (type.IsSubclassOf(typeof(RMSL_ActionNode)))
                    {

                        var node = new RMSL_BehaviorTreeBaseNode(type, type.Name, "ActionNode");
                        node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                        node.transform.position = graphView.ChangeCoordinatesTo(graphView.contentContainer, context.screenMousePosition);

                        graphView.AddGraphNode(node);

                    }
                    else if (type.IsSubclassOf(typeof(RMSL_CompositeNode)))
                    {

                        var node = new RMSL_BehaviorChildNode(type, Port.Capacity.Multi, type.Name, "CompositeNode");
                        node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                        node.transform.position = context.screenMousePosition;

                        graphView.AddGraphNode(node);

                    }
                    else if (type.IsSubclassOf(typeof(RMSL_DecoratorNode)))
                    {


                        var node = new RMSL_BehaviorChildNode(type, Port.Capacity.Single, type.Name, "CompositeNode");
                        node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                        node.transform.position = context.screenMousePosition;

                        graphView.AddGraphNode(node);

                    }

                    return true;

                }

                return false;

            }

        }

        #endregion

        #region Node 관련

        internal class RMSL_BehaviorTreeBaseNode : RMSL_BaseNode
        {

            public event Action<RMSL_BehaviorTreeBaseNode> OnSelectEvent;
            public RMSL_Node nodeObject;
            public Label descriptionLabel;

            internal RMSL_BehaviorTreeBaseNode(Type classType, string title, string message) : base(AssetDatabase.GetAssetPath(Resources.Load<VisualTreeAsset>("BehaviorNode")))
            {

                nodeObject = ScriptableObject.CreateInstance(classType) as RMSL_Node;

                nodeObject.guid = guid;
                nodeObject.name = classType.Name;

                styleSheets.Clear();
                styleSheets.Add(Resources.Load<StyleSheet>("BehaviorNodeStyle"));
                descriptionLabel = mainContainer.Q<Label>("description");
                this.title = title;
                descriptionLabel.text = message;

                RefreshAll();

            }

            public override void OnSelected()
            {

                base.OnSelected();

                OnSelectEvent?.Invoke(this);

            }

        }

        internal class RMSL_BehaviorChildNode : RMSL_BehaviorTreeBaseNode
        {

            public List<RMSL_BehaviorTreeBaseNode> childrens;

            internal RMSL_BehaviorChildNode(Type classType, Port.Capacity capacity, string title, string message) : base(classType, title, message)
            {

                AddPort(Orientation.Vertical, Direction.Output, capacity);

            }

        }

        internal class RMSL_BehaviorRootNode : RMSL_BehaviorChildNode
        {

            internal RMSL_BehaviorRootNode() : base(typeof(RMSL_RootNode), Port.Capacity.Single, "StartPoint", "Root")
            {


            }

        }

        #endregion

        #region 잡것들

        /// <summary>
        /// uxml작성할줄 몰라서 만든 윈도우
        /// </summary>
        internal class RMSL_VisualWindow : VisualElement
        {

            private Editor editor;

            public VisualElement titleContainer { get; protected set; }
            public Label titleLabel { get; protected set; }
            public VisualElement guiContainer { get; protected set; }

            public RMSL_VisualWindow(string text, Position position, Color backGroundColor)
            {

                style.backgroundColor = backGroundColor;
                style.position = position;
                style.flexGrow = 1;

                CreateTitleContainer();

                titleContainer = new VisualElement();
                titleContainer.style.position = Position.Relative;
                titleContainer.style.backgroundColor = Color.black;
                titleContainer.style.flexShrink = 0;

                titleLabel = new Label(text);
                titleLabel.style.position = Position.Relative;
                titleLabel.style.fontSize = 24;

                titleContainer.Add(titleLabel);

                guiContainer = new VisualElement();
                guiContainer.style.position = Position.Relative;
                guiContainer.style.flexGrow = 1;

                Add(titleContainer);
                Add(guiContainer);


            }

            private void CreateTitleContainer()
            {

                titleContainer = new VisualElement();
                titleContainer.style.position = Position.Relative;
                titleContainer.style.backgroundColor = Color.black;

                Add(titleContainer);

            }

            public void HandleCreateInspactor(RMSL_BehaviorTreeBaseNode node)
            {

                UnityEngine.Object.DestroyImmediate(editor);
                editor = Editor.CreateEditor(node.nodeObject);
                guiContainer.Clear();

                var imgui = new IMGUIContainer(() =>
                {

                    if (editor.target)
                    {

                        editor.OnInspectorGUI();

                    }

                });

                imgui.style.flexGrow = 1;

                guiContainer.Add(imgui);


            }

        }
        #endregion

    }

}


