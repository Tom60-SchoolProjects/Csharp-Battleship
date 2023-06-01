using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Battleship.API;

file class RawConfig {
    [JsonPropertyName("nbLignes")]
    public uint NumLines { get; }

    [JsonPropertyName("nbColonnes")]
    public uint NumColumns { get; }

    [JsonPropertyName("bateaux")]
    public RawShipConfig[ ] Ships { get; }

    [JsonConstructor]
    public RawConfig(uint numLines, uint numColumns, RawShipConfig[ ] ships) {
        NumLines   = numLines;
        NumColumns = numColumns;
        Ships      = ships;
    }

    public override string ToString() {
        return $"Size: ({NumLines}, {NumColumns})\nShips:\n{string.Join<RawShipConfig>("\n", Ships)}";
    }
}

file struct RawShipConfig {
    [JsonPropertyName("taille")]
    public uint Size { get; }

    [JsonPropertyName("nom")]
    public string Name { get; }

    [JsonConstructor]
    public RawShipConfig(uint size, string name) {
        Size = size;
        Name = name;
    }

    public static ConfigShip ToShip(RawShipConfig rawShip)
        => new ConfigShip(rawShip.Size, rawShip.Name);
}

public record struct ConfigShip(uint Size, string Name);

public class GridConfig {
    # region Constants
    private const string API_ROOT = "https://api-lprgi.natono.biz/api";
    #endregion

    #region Properties
    public (uint X, uint Y) Size  { get; }
    public ConfigShip[ ]    Ships { get; }
    #endregion

    #region Constructors
    public GridConfig() {
        // Init request
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-functions-key", "lprgi_api_key_2023");

        // Do and parse request
        RawConfig rawConfig = client.GetFromJsonAsync<RawConfig>($"{API_ROOT}/GetConfig").Result!;

        // Extract info
        Size  = (rawConfig.NumLines, rawConfig.NumColumns);
        Ships = rawConfig.Ships.Select(RawShipConfig.ToShip).ToArray();
    }
    #endregion
}