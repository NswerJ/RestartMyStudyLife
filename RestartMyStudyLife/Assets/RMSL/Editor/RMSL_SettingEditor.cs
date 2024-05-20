using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace RL.Core.Editors
{

    public class RMSL_SettingEditor : EditorWindow
    {

        [MenuItem("RMSL/Setting")]
        public static void CreateSettingWindow()
        {

            var window = GetWindow<RMSL_SettingEditor>();
            window.titleContent.text = "RMSL_Setting";
            window.maxSize = new Vector2(300, 500);
            window.minSize = new Vector2(300, 500);
            window.SettingWindow();
            window.Show();

        }



        private void SettingWindow()
        {
            var res = Resources.Load<RL.Core.RMSL_SettingSO>("RMSL/SettingSO");

            if (res == null)
            {
                if (!Directory.Exists(Application.dataPath + "/Resources"))
                {

                    Directory.CreateDirectory(Application.dataPath + "/Resources");

                }

                if (!Directory.Exists(Application.dataPath + "/Resources/RMSL"))
                {

                    Directory.CreateDirectory(Application.dataPath + "/Resources/RMSL");

                }

                var obj = CreateInstance<RMSL_SettingSO>();
                AssetDatabase.CreateAsset(obj, "Assets/Resources/RMSL/SettingSO.Asset");
                res = Resources.Load<RMSL_SettingSO>("RMSL/SettingSO");

                AssetDatabase.Refresh();

            }

            var image = new Image();

            Texture2D texture = Resources.Load<Texture2D>("RMSL_Logo");

            image.image = texture;
            image.style.flexShrink = 100;
            image.style.flexGrow = 0.3f;

            Label label = new Label("RMSL_Setting");
            Toggle toggle = new Toggle("UsePooling");
            Button settingCompleteButton = new Button(() =>
            {

                var so = Resources.Load<RMSL_SettingSO>("RMSL/SettingSO");

                so.usePooling = toggle.value;

                if (so.usePooling && Resources.Load<RMSL_PoolingSO>("RMSL/PoolingSO") == null)
                {

                    var obj = CreateInstance<RMSL_PoolingSO>();
                    AssetDatabase.CreateAsset(obj, "Assets/Resources/RMSL/PoolingSO.Asset");
                    res.poolingSO = Resources.Load<RMSL_PoolingSO>("RMSL/PoolingSO");

                }

                Close();

            });

            settingCompleteButton.text = "SettingComplete";

            toggle.value = res.usePooling;

            rootVisualElement.Add(image);
            rootVisualElement.Add(label);
            rootVisualElement.Add(toggle);
            rootVisualElement.Add(settingCompleteButton);

        }

    }
}
