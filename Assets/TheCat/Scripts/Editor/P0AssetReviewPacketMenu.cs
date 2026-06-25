using System;
using System.IO;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0AssetReviewPacketMenu
    {
        private const string MenuPath = "TheCat/P0/Write P0 Asset Review Packet";
        private const string OutputPath = "design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md";

        [MenuItem(MenuPath, false, 93)]
        private static void WriteP0AssetReviewPacket()
        {
            try
            {
                P0AssetReviewPacketReport report = P0AssetReviewPacket.EvaluateP0Packet();
                string outputPath = ToProjectPath(OutputPath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                File.WriteAllText(outputPath, report.BuildMarkdown());

                string log = "[TheCat] P0 Asset Review Packet written to " + outputPath + Environment.NewLine + report.BuildDetailedSummary();
                if (report.IsReady)
                {
                    Debug.Log(log);
                }
                else
                {
                    Debug.LogError(log);
                }

                EditorUtility.DisplayDialog(
                    report.IsReady ? "P0 Asset Review Packet Ready" : "P0 Asset Review Packet Needs Attention",
                    report.BuildSummary(),
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Asset Review Packet Failed",
                    exception.GetType().Name + ": " + exception.Message,
                "OK");
            }
        }

        private static string ToProjectPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            return Path.Combine(projectRoot, path.Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
