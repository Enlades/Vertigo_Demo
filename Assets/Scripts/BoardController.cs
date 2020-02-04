using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public const float HEX_TILE_WIDTH = 1.27f;
    public const float HEX_TILE_HEIGTH = 1.12f;
    private HexTile _hexTilePrefab;

    private GameObject _boardParentGO;

    public void Init(HexTile p_hexTilePrefab){
        _hexTilePrefab = p_hexTilePrefab;

        _boardParentGO = new GameObject("GameBoard");
        _boardParentGO.transform.position = Vector3.zero;
    }

    public HexTile[][] GenerateBoard(int p_width, int p_height){
        HexTile[][] result = new HexTile[p_width][];

        HexTile newTile = null;

        for (int i = 0; i < p_width; i++)
        {
            result[i] = new HexTile[p_height];

            for (int j = 0; j < p_height; j++)
            {

                newTile = Instantiate(_hexTilePrefab);
                newTile.name = "HexTile_" + i + "_" + j;
                newTile.transform.SetParent(_boardParentGO.transform);

                newTile.transform.position =
                    Vector3.left * p_width / 2f
                    + Vector3.down * p_height / 2f
                    + Vector3.right * HEX_TILE_WIDTH / 2f
                    + Vector3.up * HEX_TILE_HEIGTH / 2f
                    + (Vector3.right * i * 0.79f) * HEX_TILE_WIDTH
                    + (Vector3.up * j) * HEX_TILE_HEIGTH
                    + (i % 2 == 0 ? Vector3.zero : Vector3.up * HEX_TILE_HEIGTH * 0.49f);

                result[i][j] = newTile;
            }
        }

        result = SetTileConnections(result);

        return result;
    }

    private HexTile[][] SetTileConnections(HexTile[][] p_hexTiles){
        int width = p_hexTiles.Length;
        int height = p_hexTiles[0].Length;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(j != height - 1)
                    p_hexTiles[i][j].SetConnection(HexTileDirection.North, p_hexTiles[i][j + 1]);
                if(i != width - 1)
                    p_hexTiles[i][j].SetConnection(HexTileDirection.NorthEast, p_hexTiles[i + 1][j]);
                if (i != width - 1 && j > 0)
                    p_hexTiles[i][j].SetConnection(HexTileDirection.SouthEast, p_hexTiles[i + 1][j - 1]);
                if (j > 0)
                    p_hexTiles[i][j].SetConnection(HexTileDirection.South, p_hexTiles[i][j - 1]);
                if (i > 0 && j > 0)
                    p_hexTiles[i][j].SetConnection(HexTileDirection.SouthWest, p_hexTiles[i - 1][j - 1]);
                if (i > 0)
                p_hexTiles[i][j].SetConnection(HexTileDirection.NorthWest, p_hexTiles[i - 1][j]);
            }
        }

        return p_hexTiles;
    }
}
