using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Xml;

namespace YandexAdsEditor
{
    public class PostBuildProcessor
    {
        private const string XMLFilePath = "Assets/Editor/YandexAds/skad_ids.xml";

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
        {
            if (target == BuildTarget.iOS)
            {
                ModifyPlist(pathToBuildProject);
            }
        }

        private static void ModifyPlist(string pathToBuildProject)
        {
            string plistPath = Path.Combine(pathToBuildProject, "Info.plist");
            if (!File.Exists(plistPath))
            {
                Debug.LogError("Info.plist not found at: " + plistPath);
                return;
            }
            

            if (File.Exists(XMLFilePath))
            {
                List<string> skadIds = new List<string>();
                XmlDocument doc = new XmlDocument();
                doc.Load(XMLFilePath);
                XmlNodeList idNodes = doc.GetElementsByTagName("id");
                foreach (XmlNode node in idNodes)
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                        skadIds.Add(node.InnerText.Trim());
                }

                
            }
            else
            {
                Debug.LogWarning("XML file with SKAdNetwork IDs not found at: " + XMLFilePath);
            }
        }
    }
}
