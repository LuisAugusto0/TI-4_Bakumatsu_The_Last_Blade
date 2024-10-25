using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tile;

public class TilemapToMatrix : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap

    private CellInfo[,] walkableMatrix;  // Matriz de caminhabilidade

    [System.Serializable]
    public struct CellInfo
    {
        public bool isWalkable;  // Indica se é caminhável
        public Vector3 worldPosition;  // Posição no mundo

        public CellInfo(bool isWalkable, Vector3 worldPosition)
        {
            this.isWalkable = isWalkable;
            this.worldPosition = worldPosition;
        }
    }

    public CellInfo[,] WalkableMatrix
    {
        get { return walkableMatrix; }
    }

    void Start()
    {
        CreateBooleanMatrix();
        PrintMatrix();  // Imprimir a matriz após a criação
    }

    void CreateBooleanMatrix()
    {
        BoundsInt bounds = tilemap.cellBounds;
        walkableMatrix = new CellInfo[bounds.size.x, bounds.size.y];

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPosition);

                bool isWalkable = IsTileWalkable(tile);
                Vector3 worldPosition = tilemap.GetCellCenterWorld(cellPosition);  // Obter a posição mundial

                walkableMatrix[x - bounds.xMin, y - bounds.yMin] = new CellInfo(isWalkable, worldPosition);
            }
        }

        Debug.Log("Matriz de caminhabilidade criada com sucesso!");
    }

    bool IsTileWalkable(TileBase tile)
    {
        // Verifica se o tile tem algum tipo de colisão
        if (tile is Tile tileWithCollider)
        {
            return tileWithCollider.colliderType == ColliderType.None;
        }
        return true;  // Se não for um Tile com colliderType, assume que é caminhável
    }

    public Vector2 WalkableMatrixToMatrix(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);  // Converter posição mundial para posição de célula
        return new Vector2(cellPosition.x - tilemap.cellBounds.xMin, cellPosition.y - tilemap.cellBounds.yMin);
    }

    void PrintMatrix()
    {
        string matrixString = "";

        for (int y = walkableMatrix.GetLength(1) - 1; y >= 0; y--)  // Imprimir de cima para baixo
        {
            for (int x = 0; x < walkableMatrix.GetLength(0); x++)
            {
                matrixString += walkableMatrix[x, y].isWalkable ? "1 " : "0 ";
            }
            matrixString += "\n";
        }

        Debug.Log(matrixString);
    }
}
