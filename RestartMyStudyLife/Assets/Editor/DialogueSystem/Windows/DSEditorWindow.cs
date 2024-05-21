using UnityEditor;
using UnityEngine;
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

       
    }
}

