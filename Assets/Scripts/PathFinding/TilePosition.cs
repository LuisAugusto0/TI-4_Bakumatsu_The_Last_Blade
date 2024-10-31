using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSeparator : MonoBehaviour
{
    public Tilemap originalTilemap; // Tilemap original de referência
    public Tilemap collisionTilemap; // Tilemap para tiles com colisão (Z = -2)
    public Tilemap noCollisionTilemap; // Tilemap para tiles sem colisão (Z = -3)

    void Start()
    {
        // Percorre todas as posições do Tilemap original
        foreach (var pos in originalTilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = originalTilemap.GetTile(pos);

            if (tile != null)
            {
                // Pega o tipo de colisão do tile
                Tile.ColliderType colliderType = originalTilemap.GetColliderType(pos);

                // Separa os tiles nos Tilemaps corretos
                if (colliderType == Tile.ColliderType.None)
                {
                    // Move o tile para o Tilemap sem colisão (Z = -3)
                    noCollisionTilemap.SetTile(pos, tile);
                    noCollisionTilemap.SetTransformMatrix(pos, Matrix4x4.TRS(new Vector3(pos.x, pos.y, -3), Quaternion.identity, Vector3.one));
                }
                else
                {
                    // Move o tile para o Tilemap com colisão (Z = -2)
                    collisionTilemap.SetTile(pos, tile);
                    collisionTilemap.SetTransformMatrix(pos, Matrix4x4.TRS(new Vector3(pos.x, pos.y, -2), Quaternion.identity, Vector3.one));
                }

                // Remove o tile do Tilemap original
                originalTilemap.SetTile(pos, null);
            }
        }
    }
}
