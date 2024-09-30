using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TileType;

public class Dual_Grid_Script : MonoBehaviour {
    protected static Vector3Int[] NEIGHBOURS = new Vector3Int[] {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 1, 0), 
        new Vector3Int(1, 1, 0)
    };

    protected static Dictionary<Tuple<TileType, TileType, TileType, TileType>, Tile> neighbourTupleToTile;
    int Steps;
    int RandGrass;
    System.Random rand;

    public Camera mainCamera;

    // Referencia para cada Tilemap
    public Tilemap ConfigTilemap;
    public Tilemap DisplayTilemap;

    // Referencia para os tiles utilizados como place holders
    public Tile grassPlaceholderTile;
    public Tile dirtPlaceholderTile;

    // Provide the 16 tiles in the inspector
    public Tile[] tiles;

    void Start() {
        // This dictionary stores the "rules", each 4-neighbour configuration corresponds to a tile
        // |_1_|_2_|
        // |_3_|_4_|
        neighbourTupleToTile = new() {
            {new (Grass, Grass, Grass, Grass), tiles[6]},
            {new (Dirt, Dirt, Dirt, Grass), tiles[13]}, // OUTER_BOTTOM_RIGHT
            {new (Dirt, Dirt, Grass, Dirt), tiles[0]}, // OUTER_BOTTOM_LEFT
            {new (Dirt, Grass, Dirt, Dirt), tiles[8]}, // OUTER_TOP_RIGHT
            {new (Grass, Dirt, Dirt, Dirt), tiles[15]}, // OUTER_TOP_LEFT
            {new (Dirt, Grass, Dirt, Grass), tiles[1]}, // EDGE_RIGHT
            {new (Grass, Dirt, Grass, Dirt), tiles[11]}, // EDGE_LEFT
            {new (Dirt, Dirt, Grass, Grass), tiles[3]}, // EDGE_BOTTOM
            {new (Grass, Grass, Dirt, Dirt), tiles[9]}, // EDGE_TOP
            {new (Dirt, Grass, Grass, Grass), tiles[5]}, // INNER_BOTTOM_RIGHT
            {new (Grass, Dirt, Grass, Grass), tiles[2]}, // INNER_BOTTOM_LEFT
            {new (Grass, Grass, Dirt, Grass), tiles[10]}, // INNER_TOP_RIGHT
            {new (Grass, Grass, Grass, Dirt), tiles[7]}, // INNER_TOP_LEFT
            {new (Dirt, Grass, Grass, Dirt), tiles[14]}, // DUAL_UP_RIGHT
            {new (Grass, Dirt, Dirt, Grass), tiles[4]}, // DUAL_DOWN_RIGHT
            {new (Dirt, Dirt, Dirt, Dirt), tiles[12]},
        };
        RefreshDisplayTilemap();
    }

    void Update()
    {
        
    }

    public void SetCell(Vector3Int coords, Tile tile) {
        ConfigTilemap.SetTile(coords, tile);
        setDisplayTile(coords);
    }

    private TileType getPlaceholderTileTypeAt(Vector3Int coords) {
        if (ConfigTilemap.GetTile(coords) == grassPlaceholderTile)
            return Grass;
        else
            return Dirt;
    }

    protected Tile calculateDisplayTile(Vector3Int coords) {
        // 4 neighbours
        TileType topRight = getPlaceholderTileTypeAt(coords - NEIGHBOURS[0]);
        TileType topLeft = getPlaceholderTileTypeAt(coords - NEIGHBOURS[1]);
        TileType botRight = getPlaceholderTileTypeAt(coords - NEIGHBOURS[2]);
        TileType botLeft = getPlaceholderTileTypeAt(coords - NEIGHBOURS[3]);

        Tuple<TileType, TileType, TileType, TileType> neighbourTuple = new(topLeft, topRight, botLeft, botRight);

        return neighbourTupleToTile[neighbourTuple];
    }

    protected void setDisplayTile(Vector3Int pos) {
        for (int i = 0; i < NEIGHBOURS.Length; i++) {
            Vector3Int newPos = pos + NEIGHBOURS[i];
            
            if(Steps==RandGrass){
                if (calculateDisplayTile(newPos) == tiles[6]){
                    DisplayTilemap.SetTile(newPos, tiles[16]);
                } else {
                    DisplayTilemap.SetTile(newPos, calculateDisplayTile(newPos));
                }
                Steps = 0;
                RandGrass = rand.Next(8, 20);
            } else {
                DisplayTilemap.SetTile(newPos, calculateDisplayTile(newPos));
            }
            Steps++;
        }
    }

    // The tiles on the display tilemap will recalculate themselves based on the placeholder tilemap
    public void RefreshDisplayTilemap() {
        rand = new System.Random(Seed:0);
        Steps = 0;
        RandGrass = rand.Next(8, 20);

        Rect rect = mainCamera.pixelRect;
        Vector2 pos = rect.position;

        Debug.Log("\nCamera x: " + rect.x);

        
        //Atualizar 50 pixel acima e 50 abaixo da câmeta quando refresh display for chamado
        for (int i = ((int)Math.Ceiling(pos.x))-50; i < ((int)Math.Ceiling(pos.x)) + 50; i++) {
            for (int j = ((int)pos.y)-10; j < ((int)pos.y) + 10; j++) {
                setDisplayTile(new Vector3Int(i, j, 0));
            }
        }
    }


}

public enum TileType {
    None,
    Grass,
    Dirt
}


