using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Battleship.API;

/// <summary>
/// Define the raw config received from the server
/// </summary>
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

/// <summary>
/// Define the raw config element of a ship, as received from the server
/// </summary>
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

/// <summary>
/// Define a ship, as will be used in the game config
/// </summary>
public record struct ConfigShip(uint Size, string Name);

/// <summary>
/// Define the global config, as will be used in the game config 
/// </summary>
public class GridConfig {
    # region Constants
    private const string     API_ROOT = "https://api-lprgi.natono.biz/api";
    public static GridConfig Singleton { get; } = new GridConfig();
    #endregion

    #region Properties
    public (uint X, uint Y) Size  { get; }
    public ConfigShip[ ]    Ships { get; }
    #endregion

    #region Constructors
    private GridConfig() {
        // API is dead, this is a template to make the game work
        // TODO: Make a random config generator

        // Extract info
        Size  = (10, 10);
        Ships = new[ ] {
            new ConfigShip(5, "Porte-avion"),
            new ConfigShip(4, "Croiseur"),
            new ConfigShip(3, "Contre-torpilleur"),
            new ConfigShip(3, "Contre-torpilleur"),
            new ConfigShip(2, "Torpilleur"),
        };
    }
    #endregion
}