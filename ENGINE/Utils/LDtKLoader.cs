using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kai_Engine.ENGINE.Utils
{
    internal class LDtKLoader
    {
        public Root Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            string jsonContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Root>(jsonContent);
        }
    }

    public class CustomFields
    {
        [JsonExtensionData]
        public Dictionary<string, object> Fields { get; set; }
    }

    public class Entities
    {
        [JsonProperty("entities")]
        public List<Entity> AllEntities { get; set; }
    }

    public class Entity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("iid")]
        public string Iid { get; set; }

        [JsonProperty("layer")]
        public string Layer { get; set; }

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("color")]
        public int Color { get; set; }

        [JsonProperty("customFields")]
        public CustomFields CustomFields { get; set; }
    }

    public class GridTile
    {
        [JsonProperty("px")]
        public List<int> Px { get; set; } // Position in pixels (e.g., [32, 64])

        [JsonProperty("src")]
        public List<int> Src { get; set; } // Source position in the tile sheet

        [JsonProperty("f")]
        public int F { get; set; } // Flip bits

        [JsonProperty("t")]
        public int T { get; set; } // Tile ID

        [JsonProperty("d")]
        public int D { get; set; } // Internal data
    }

    public class Layer
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } // Could be "IntGrid", "Entities", etc.

        [JsonProperty("gridTiles")]
        public List<GridTile> GridTiles { get; set; }

        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; } // Adjust for other entity types.

        [JsonProperty("autoLayerTiles")]
        public List<GridTile> AutoLayerTiles { get; set; }

        [JsonProperty("layerDefUid")]
        public int LayerDefUid { get; set; }
    }

    public class Root
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("uniqueIdentifer")]
        public string UniqueIdentifer { get; set; }

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("bgColor")]
        public string BgColor { get; set; }

        [JsonProperty("neighbourLevels")]
        public List<object> NeighbourLevels { get; set; }

        [JsonProperty("customFields")]
        public CustomFields CustomFields { get; set; }

        [JsonProperty("layers")]
        public List<Layer> Layers { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }
    }
}
