using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Battleship;

public record RawConfig {
    [JsonPropertyName("nbLignes")]
    public int NumLines { get; }

    [JsonPropertyName("nbColonnes")]
    public int NumColumns { get; }

    [JsonPropertyName("bateaux")]
    public RawShipConfig[ ] Ships { get; }

    [JsonConstructor]
    public RawConfig(int numLines, int numColumns, RawShipConfig[ ] ships) {
        NumLines   = numLines;
        NumColumns = numColumns;
        Ships      = ships;
    }

    public override string ToString() {
        return $"Size: ({NumLines}, {NumColumns})\nShips:\n{string.Join<RawShipConfig>("\n", Ships)}";
    }
}

public record RawShipConfig {
    [JsonPropertyName("taille")]
    public int Size { get; }

    [JsonPropertyName("nom")]
    public string Name { get; }

    [JsonConstructor]
    public RawShipConfig(int size, string name) {
        Size = size;
        Name = name;
    }

    public override string ToString() {
        return $"- {Name}: {Size} cells";
    }
}

public class GridConfig {
    # region Constants
    private const string API_ROOT = "https://api-lprgi.natono.biz/api";
    #endregion

    #region Properties
    public (int, int) Size { get; }
    #endregion

    #region Constructors
    public GridConfig() {
        // Init request
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-functions-key", "lprgi_api_key_2023");

        // Do and parse request
        RawConfig rawConfig = client.GetFromJsonAsync<RawConfig>($"{API_ROOT}/GetConfig").Result!;

        // Extract info
        Size = (rawConfig.NumLines, rawConfig.NumColumns);
    }
    #endregion
}