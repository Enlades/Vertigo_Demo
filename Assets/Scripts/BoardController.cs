using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public const float HEX_TILE_WIDTH = 1.397f;
    public const float HEX_TILE_HEIGTH = 1.276f;
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
                newTile.SetBoardPosition(new Vector2Int(i, j));
                newTile.name = "HexTile_" + i + "_" + j;
                newTile.transform.SetParent(_boardParentGO.transform);

                newTile.transform.position =
                    Vector3.left * p_width / 2f
                    + Vector3.down * p_height / 2f
                    + Vector3.right * HEX_TILE_WIDTH / 2f
                    + Vector3.up * HEX_TILE_HEIGTH / 2f
                    + (Vector3.right * i * 0.79f) * HEX_TILE_WIDTH
                    + (Vector3.up * j) * HEX_TILE_HEIGTH
                    + (Vector3.down * (HEX_TILE_HEIGTH - 1f))
                    + (Vector3.left * (HEX_TILE_WIDTH - 1f))
                    + (i % 2 == 0 ? Vector3.zero : Vector3.up * HEX_TILE_HEIGTH * 0.49f);

                result[i][j] = newTile;
            }
        }

        result = SetTileConnections(result);

        return result;
    }

    public void RotateTiles(HexTile[] p_selectedTiles, bool p_isClockwise){
        if(p_isClockwise){
            Vector3 firstTilePosition = p_selectedTiles[0].transform.position;
            for (int i = 0; i < p_selectedTiles.Length; i++)
            {
                if (i < p_selectedTiles.Length - 1)
                {
                    p_selectedTiles[i].transform.position = p_selectedTiles[i + 1].transform.position;
                }
                else
                {
                    p_selectedTiles[i].transform.position = firstTilePosition;
                }
            }
        }else{
            Vector3 lastTilePosition = p_selectedTiles[p_selectedTiles.Length - 1].transform.position;
            for (int i = p_selectedTiles.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    p_selectedTiles[i].transform.position = lastTilePosition;
                }
                else
                {
                    p_selectedTiles[i].transform.position = p_selectedTiles[i - 1].transform.position;
                }
            }
        }
    }

    private HexTile[][] SetTileConnections(HexTile[][] p_hexTiles){
        int width = p_hexTiles.Length;
        int height = p_hexTiles[0].Length;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(j != height - 1){
                    p_hexTiles[i][j].SetConnection(HexTileDirection.North, p_hexTiles[i][j + 1]);
                }

                if(i % 2 == 0){
                    if (i != width - 1)
                    {
                        p_hexTiles[i][j].SetConnection(HexTileDirection.NorthEast, p_hexTiles[i + 1][j]);
                    }
                }else{
                    if (i != width - 1 && j < height - 1)
                    {
                        p_hexTiles[i][j].SetConnection(HexTileDirection.NorthEast, p_hexTiles[i + 1][j + 1]);
                    }
                }

                if(i % 2 == 0){
                    if (i != width - 1 && j > 0)
                    {
                        p_hexTiles[i][j].SetConnection(HexTileDirection.SouthEast, p_hexTiles[i + 1][j - 1]);
                    }
                }else{
                    if (i != width - 1)
                    {
                        p_hexTiles[i][j].SetConnection(HexTileDirection.SouthEast, p_hexTiles[i + 1][j]);
                    }
                }

                if (j > 0){
                    p_hexTiles[i][j].SetConnection(HexTileDirection.South, p_hexTiles[i][j - 1]);
                }

                if (i % 2 == 0)
                {
                    if (i > 0 && j > 0)
                    {
                        p_hexTiles[i][j].SetConnection(HexTileDirection.SouthWest, p_hexTiles[i - 1][j - 1]);
                    }
                }else{
                    if (i > 0)
                    {
                        p_hexTiles[i][j].SetConnection(HexTileDirection.SouthWest, p_hexTiles[i - 1][j]);
                    }
                }

                if(i % 2 == 0){
                    if (i > 0)
                    {
                        p_hexTiles[i][j].SetConnection(HexTileDirection.NorthWest, p_hexTiles[i - 1][j]);
                    }
                }else{
                    if (i > 0 && j < height - 1)
                    {
                        p_hexTiles[i][j].SetConnection(HexTileDirection.NorthWest, p_hexTiles[i - 1][j + 1]);
                    }
                }
            }
        }

        return p_hexTiles;
    }
}
