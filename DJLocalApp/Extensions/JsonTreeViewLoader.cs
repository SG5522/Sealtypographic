using System.Text.Json;
using static System.Text.Json.JsonElement;

namespace DJLocalApp.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonTreeViewLoader
    {
        public static void LoadJson(this System.Windows.Forms.TreeView treeView, string jsonString, bool isClean, bool isExpand)
        {
            if(isClean)
            {
                treeView.Nodes.Clear();
            }

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return;
            }

            JsonDocument jsonDocumentObject = JsonDocument.Parse(jsonString);
            AddObjectNodes(jsonDocumentObject, "JSON", treeView.Nodes);

            if(isExpand)
            {
                treeView.ExpandAll();
            }
        }

        public static void AddObjectNodes(JsonDocument jsonDocumentObject, string name, TreeNodeCollection parent)
        {
            var node = new TreeNode(name);
            parent.Add(node);

            JsonElement root = jsonDocumentObject.RootElement;
            AddTokenNodes(root, node.Nodes);
        }

        /// <summary>
        /// Adds the token nodes.(未完待補)
        /// </summary>
        /// <param name="jsonElement">The json element.</param>
        /// <param name="parent">The parent.</param>
        private static void AddTokenNodes(JsonElement jsonElement, TreeNodeCollection parent)
        {
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                parent.Add(new TreeNode(string.Format("{0}: {1}", jsonElement.GetString(), jsonElement.GetString())));
            }
            else if (jsonElement.ValueKind == JsonValueKind.True || jsonElement.ValueKind == JsonValueKind.False)
            {
                parent.Add(new TreeNode(string.Format("{0}: {1}", jsonElement.GetBoolean(), jsonElement.GetBoolean())));
            }
            else if (jsonElement.ValueKind == JsonValueKind.Number)
            {
                parent.Add(new TreeNode(string.Format("{0}: {1}", jsonElement.GetInt32(), jsonElement.GetInt32())));
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                AddArrayNodes(jsonElement, parent);
            }
            else if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                ObjectEnumerator props = jsonElement.EnumerateObject();

                while (props.MoveNext())
                {
                    JsonProperty prop = props.Current;
                    parent.Add(new TreeNode(string.Format("{0}: {1}", prop.Name, prop.Value)));
                }
            }
        }

        private static void AddArrayNodes(JsonElement jsonElement, TreeNodeCollection parent)
        {
            ObjectEnumerator props = jsonElement.EnumerateObject();
            JsonProperty prop = props.Current;
            TreeNode node = new(string.Format("{0}: {1}", prop.Name, prop.Value));
            parent.Add(node);

            ArrayEnumerator arrayEnumerator = jsonElement.EnumerateArray();
            while(arrayEnumerator.MoveNext())
            {
                JsonElement jsonElement2 = arrayEnumerator.Current;
                AddTokenNodes(jsonElement2, node.Nodes);
            }
        }
    }
}
