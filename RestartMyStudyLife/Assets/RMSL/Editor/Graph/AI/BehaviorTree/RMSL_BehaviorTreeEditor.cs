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
    internal class RMSL_BehaviorTreeBaseEditor : RMSL_GraphBaseWindow<RMSL_BehaviorTreeGraph>
    {

        private VisualElement graphRoot;
        private RMSL_BehaviorTree behaviorTree;

        [MenuItem("RMSL/AI/BehaviorTree")]
        private static void Open()
        {

            var window = CreateWindow<RMSL_BehaviorTreeBaseEditor>();
            window.titleContent.text = "BehaviorTree Editor";
            window.maximized = true;
            window.Show();

        }

        protected override void OnEnable()
        {

            behaviorTree = ScriptableObject.CreateInstance<RMSL_BehaviorTree>();

            AddToolBar();
            SetUpToolbar();
            CreateGraphRoot();
            SetUpGraphView();
            CreateSpliteView();

            Save();

            var root = new RMSL_BehaviorRootNode(graphView);

            root.nodeObject.name = "RootNode";

            graphView.AddAssets(root);
            graphView.AddElement(root);

            root.RefreshAll();

        }

        private void CreateGraphRoot()
        {

            graphRoot = new VisualElement();
            graphRoot.style.position = Position.Relative;
            graphRoot.style.flexDirection = FlexDirection.Row;
            graphRoot.style.flexGrow = 1;

            rootVisualElement.Add(graphRoot);

        }

        private void CreateSpliteView()
        {

            TwoPaneSplitView splitView = new TwoPaneSplitView(1, 300, TwoPaneSplitViewOrientation.Horizontal);

            splitView.contentContainer.Add(graphView);
            splitView.contentContainer.Add(new RMSL_VisualWindow("Insp", Position.Relative, new Color(0.2f, 0.2f, 0.2f)));
            graphRoot.Add(splitView);
        }

        private void SetUpGraphView()
        {

            graphView = new RMSL_BehaviorTreeGraph();

            graphView.SetDrag();
            graphView.SetMiniMap(new Rect(10, 30, 300, 300));
            graphView.SetGrid();
            graphView.SetZoom();

            graphView.style.position = Position.Relative;
            graphView.style.width = 1600;
            graphView.style.flexGrow = 1;

            graphView.Init(behaviorTree);

        }

        private void SetUpToolbar()
        {

            var saveBtn = new Button(Save);
            saveBtn.text = "SaveFile";
            var loadBtn = new Button(Load);
            loadBtn.text = "LoadFile";

            toolbar.Add(saveBtn);
            toolbar.Add(loadBtn);

        }

        private void Load()
        {



        }
        /// <summary>
        /// 
        /// </summary>
        private void Save()
        {

            if (!behaviorTree.isMemory)
            {

                var path = EditorUtility.SaveFilePanelInProject("SaveBehaviorTree", "NewBehaviorTree", "asset", "Save");


                AssetDatabase.CreateAsset(behaviorTree, path);

                behaviorTree.isMemory = true;

            }

            var nodeLs = graphView.edges.ToList();

            nodeLs.ForEach((x) =>
            {

                var baseNode = behaviorTree.nodes.Find(node => node.guid == (x.output.node as RMSL_BaseNode).guid);
                var childNode = behaviorTree.nodes.Find(node => node.guid == (x.input.node as RMSL_BaseNode).guid);

                if (baseNode as RMSL_ControlFlowNode != null)
                {

                    var baseControllNode = baseNode as RMSL_ControlFlowNode;
                    baseControllNode.childrens.Add(childNode);

                }
                else if (baseNode as RMSL_DecoratorNode != null)
                {

                    var baseDecoNode = baseNode as RMSL_DecoratorNode;
                    baseDecoNode.children = childNode;

                }
                else if (baseNode as RMSL_RootNode != null)
                {

                    var baseDecoNode = baseNode as RMSL_RootNode;
                    baseDecoNode.children = childNode;

                }



            });

            AssetDatabase.SaveAssets();

        }


    }

    internal class RMSL_BehaviorTreeGraph : RMSL_BaseGraphView
    {

        private RMSL_BehaviorTreeSearchWindow searchWindow;
        private RMSL_BehaviorTree behaviorTree;

        public RMSL_BehaviorTreeGraph()
        {

            AddSearchWindow();

        }

        public void Init(RMSL_BehaviorTree behaviorTree)
        {

            this.behaviorTree = behaviorTree;

        }

        private void AddSearchWindow()
        {

            searchWindow = ScriptableObject.CreateInstance<RMSL_BehaviorTreeSearchWindow>();
            searchWindow.Init(this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);

        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {

            return ports.ToList().Where(x => x.direction != startPort.direction && x.node != startPort.node).ToList();

        }

        public void RemovePort(RMSL_BaseNode node, Port port)
        {

            var targetEdge = edges.ToList().Where(x => x.output.node == port.node && x.output.portName == port.portName);

            if (targetEdge.Any())
            {

                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());

            }

            node.outputContainer.Remove(port);

            node.RefreshAll();

            var item = node.outputContainer.Query<Port>().ToList().OrderBy(x => int.Parse(x.portName)).ToList();

            for (int i = 0; i < item.Count; i++)
            {

                item[i].portName = (i + 1).ToString();

            }

            node.RefreshAll();

        }

        public void AddAssets(RMSL_BehaviorTreeNode node)
        {

            AssetDatabase.AddObjectToAsset(node.nodeObject, behaviorTree);
            behaviorTree.nodes.Add(node.nodeObject);
            //AssetDatabase.SaveAssets();

        }

    }

    internal class RMSL_BehaviorTreeSearchWindow : ScriptableObject, ISearchWindowProvider
    {

        private RMSL_BehaviorTreeGraph graph;

        public void Init(RMSL_BehaviorTreeGraph graph)
        {

            this.graph = graph;

        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {

            var tree = new List<SearchTreeEntry>()
            {

                new SearchTreeGroupEntry(new GUIContent("Create Behavior"), 0),
                new SearchTreeGroupEntry(new GUIContent("Node"), 1),

            };

            var types = TypeCache.GetTypesDerivedFrom<RMSL_Node>();

            var nodes = types.Where(x => x.IsAbstract == false && x.Name != "RMSL_RootNode").ToList();

            for (int i = 0; i < nodes.Count; i++)
            {

                tree.Add(new SearchTreeEntry(new GUIContent(nodes[i].Name)) { userData = nodes[i], level = 2 });

            }

            return tree;

        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {


            if (searchTreeEntry.userData is Type)
            {

                var t = searchTreeEntry.userData as Type;

                if (t.IsSubclassOf(typeof(RMSL_ControlFlowNode)))
                {

                    var node = new RMSL_BehaviorChildNode(graph, t.Name, "ControlFlow", t);
                    node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    node.AddPort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi);
                    node.transform.position = context.screenMousePosition;

                    graph.AddAssets(node);
                    graph.AddElement(node);

                }
                else if (t.IsSubclassOf(typeof(RMSL_DecoratorNode)))
                {

                    var node = new RMSL_BehaviorChildNode(graph, t.Name, "Decorator", t);
                    node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    node.AddPort(Orientation.Vertical, Direction.Output, Port.Capacity.Single);
                    node.transform.position = context.screenMousePosition;

                    graph.AddAssets(node);
                    graph.AddElement(node);

                }
                else
                {

                    var node = new RMSL_BehaviorActionNode(t.Name, "Behavior", t);
                    node.AddPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single);
                    node.transform.position = context.screenMousePosition;

                    graph.AddAssets(node);
                    graph.AddElement(node);


                }

            }

            return false;

        }

    }

    internal class RMSL_BehaviorTreeNode : RMSL_BaseNode
    {

        public RMSL_Node nodeObject;

        public RMSL_BehaviorTreeNode(Type classType) : base(AssetDatabase.GetAssetPath(Resources.Load<VisualTreeAsset>("BehaviorNode")))
        {

            var style = Resources.Load<StyleSheet>("BehaviorNodeStyle");

            nodeObject = ScriptableObject.CreateInstance(classType) as RMSL_Node;
            nodeObject.guid = guid;
            styleSheets.Add(style);
            AddToClassList("node");

            RefreshAll();

        }

    }

    internal class RMSL_BehaviorActionNode : RMSL_BehaviorTreeNode
    {

        public RMSL_BehaviorActionNode(string className, string message, Type classType) : base(classType)
        {

            title = className;
            mainContainer.Q<Label>("description").text = message;

        }

    }

    internal class RMSL_BehaviorRootNode : RMSL_BehaviorChildNode
    {

        public RMSL_BehaviorRootNode(RMSL_BehaviorTreeGraph graph) : base(graph, "StartPoint", "", typeof(RMSL_RootNode))
        {

            name = "RootNode";
            AddPort(Orientation.Vertical, Direction.Output, Port.Capacity.Single);

        }

    }

    internal class RMSL_BehaviorChildNode : RMSL_BehaviorTreeNode
    {

        protected RMSL_BehaviorTreeGraph graph;

        public RMSL_BehaviorChildNode(RMSL_BehaviorTreeGraph graph, string title, string message, Type classType) : base(classType)
        {

            this.title = title;
            mainContainer.Q<Label>("description").text = message;
            this.graph = graph;

        }

        public List<GUID> ConnectNode()
        {

            var targetEdge = graph.edges.ToList().Where(x => x.output.node == this).Select(x => x.input.node).ToList();

            return targetEdge.OrderBy(x => x.transform.position.x).ToList().ConvertAll(x => (x as RMSL_BehaviorChildNode).guid);

        }

    }

    internal class RMSL_VisualWindow : VisualElement
    {

        public VisualElement titleContainer { get; protected set; }
        public Label titleLabel { get; protected set; }

        public RMSL_VisualWindow(string text, Position position, Color backGroundColor)
        {

            style.backgroundColor = backGroundColor;
            style.position = position;

            CreateTitleContainer();

            titleLabel = new Label(text);


            titleContainer.Add(titleLabel);

        }

        private void CreateTitleContainer()
        {

            titleContainer = new VisualElement();
            titleContainer.style.position = Position.Relative;
            titleContainer.style.backgroundColor = Color.black;
            titleContainer.style.flexShrink = 0;

            Add(titleContainer);

        }

    }
}

    
