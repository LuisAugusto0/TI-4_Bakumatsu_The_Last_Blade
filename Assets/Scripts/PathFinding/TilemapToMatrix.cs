using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tile;

public class TilemapToMatrix : MonoBehaviour
{
    public Tilemap tilemap;  // Refer�ncia ao Tilemap

    private CellInfo[,] walkableMatrix;  // Matriz de caminhabilidade

    [System.Serializable]
    public struct CellInfo
    {
        public bool isWalkable;  // Indica se � caminh�vel
        public Vector3 worldPosition;  // Posi��o no mundo

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

    void Awake()
    {
        CreateBooleanMatrix();
        PrintMatrix();  // Imprimir a matriz ap�s a cria��o
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
                Vector3 worldPosition = tilemap.GetCellCenterWorld(cellPosition);  // Obter a posi��o mundial

                walkableMatrix[x - bounds.xMin, y - bounds.yMin] = new CellInfo(isWalkable, worldPosition);
            }
        }

        Debug.Log("Matriz de caminhabilidade criada com sucesso!");
    }

    bool IsTileWalkable(TileBase tile)
    {
        // Verifica se o tile tem algum tipo de colis�o
        if (tile is Tile tileWithCollider)
        {
            return tileWithCollider.colliderType == ColliderType.None;
        }
        return true;  // Se n�o for um Tile com colliderType, assume que � caminh�vel
    }

    public Vector2 WalkableMatrixToMatrix(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);  // Converter posi��o mundial para posi��o de c�lula
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
