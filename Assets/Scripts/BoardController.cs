using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public const float HEX_TILE_WIDTH = 1.397f;
    public const float HEX_TILE_HEIGTH = 1.276f;

    public HexTile[][] GameTiles{get; private set;}

    private HexTile _hexTilePrefab;

    private GameObject _boardParentGO;

    public void Init(HexTile p_hexTilePrefab){
        _hexTilePrefab = p_hexTilePrefab;

        _boardParentGO = new GameObject("GameBoard");
        _boardParentGO.transform.position = Vector3.zero;
    }

    public HexTile[][] GenerateBoard(int p_width, int p_height){
        GameTiles = new HexTile[p_width][];

        HexTile newTile = null;

        for (int i = 0; i < p_width; i++)
        {
            GameTiles[i] = new HexTile[p_height];

            for (int j = 0; j < p_height; j++)
            {

                newTile = Instantiate(_hexTilePrefab);
                newTile.SetBoardPosition(new Vector2Int(i, j));
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

                GameTiles[i][j] = newTile;
            }
        }

        SetTileConnections(GameTiles);

        return GameTiles;
    }

    public void RotateTiles(HexTile[] p_selectedTiles, bool p_isClockwise)
    {
        if (p_selectedTiles == null)
            return;

        HexTile.HexTilesSwap(p_selectedTiles, p_isClockwise);

        for (int i = 0; i < p_selectedTiles.Length; i++)
        {
            GameTiles[p_selectedTiles[i].BoardPosition.x][p_selectedTiles[i].BoardPosition.y]
            = p_selectedTiles[i];
        }
    }

    public void UpdateTileConnections(){
        SetTileConnections(GameTiles);
    }

    public bool CheckForExplosions(out HexTile[] p_explodingHexes){
        List<HexTile> sameColorHexes = new List<HexTile>();
        p_explodingHexes = null;

        for (int i = 0; i < GameTiles.Length - 1; i++)
        {
            for (int j = 0; j < GameTiles[i].Length - 1; j++)
            {

                if (GameTiles[i][j] != null && GameTiles[i][j].HexTileColor
                == GameTiles[i][j].GetConnectedTile(HexTileDirection.North).HexTileColor
                && GameTiles[i][j].HexTileColor
                == GameTiles[i][j].GetConnectedTile(HexTileDirection.NorthEast).HexTileColor)
                {
                    p_explodingHexes = SearchOtherHexes(GameTiles[i][j]);
                    return true;
                }

                if (GameTiles[i][j] != null && GameTiles[i][j].HexTileColor
                == GameTiles[i][j].GetConnectedTile(HexTileDirection.North).HexTileColor
                && i > 0
                && GameTiles[i][j].HexTileColor
                == GameTiles[i][j].GetConnectedTile(HexTileDirection.NorthWest).HexTileColor)
                {
                    p_explodingHexes = SearchOtherHexes(GameTiles[i][j]);
                    return true;
                }
            }
        }
        return false;
    }

    public void ExplodeTiles(HexTile[] p_explodingHexes){

        for (int k = 0; k < p_explodingHexes.Length; k++)
        {
            p_explodingHexes[k].Explode();

            GameTiles[p_explodingHexes[k].BoardPosition.x][p_explodingHexes[k].BoardPosition.y] = null;
        }

        /*List<HexTile> sameColorHexes = new List<HexTile>();
        for (int i = 0; i < GameTiles.Length - 1; i++)
        {
            for (int j = 0; j < GameTiles[i].Length - 1; j++)
            {

                if (GameTiles[i][j] != null && GameTiles[i][j].HexTileColor
                == GameTiles[i][j].GetConnectedTile(HexTileDirection.North).HexTileColor
                && GameTiles[i][j].HexTileColor
                == GameTiles[i][j].GetConnectedTile(HexTileDirection.NorthEast).HexTileColor)
                {
                    HexTile[] explodingHexes = SearchOtherHexes(GameTiles[i][j]);

                    for (int k = 0; k < explodingHexes.Length; k++)
                    {
                        explodingHexes[k].Explode();

                        GameTiles[explodingHexes[k].BoardPosition.x][explodingHexes[k].BoardPosition.y] = null;
                    }
                }

                if (GameTiles[i][j] != null && GameTiles[i][j].HexTileColor
                == GameTiles[i][j].GetConnectedTile(HexTileDirection.North).HexTileColor
                && i > 0
                && GameTiles[i][j].HexTileColor
                == GameTiles[i][j].GetConnectedTile(HexTileDirection.NorthWest).HexTileColor)
                {
                    HexTile[] explodingHexes = SearchOtherHexes(GameTiles[i][j]);

                    for (int k = 0; k < explodingHexes.Length; k++)
                    {
                        explodingHexes[k].Explode();

                        GameTiles[explodingHexes[k].BoardPosition.x][explodingHexes[k].BoardPosition.y] = null;
                    }
                }
            }
        }*/
    }

    public void RefillBoard(Color[] p_colors){
        HexTile newTile = null;
        for(int i = 0; i < GameTiles.Length; i++){
            for(int j = 0; j < GameTiles[i].Length; j++){
                if(GameTiles[i][j] == null){
                    newTile = Instantiate(_hexTilePrefab);
                    newTile.SetBoardPosition(new Vector2Int(i, j));
                    newTile.transform.SetParent(_boardParentGO.transform);

                    newTile.SetColor(p_colors[Random.Range(0, p_colors.Length)]);

                    newTile.transform.position =
                        Vector3.left * GameTiles.Length / 2f
                        + Vector3.down * GameTiles[i].Length / 2f
                        + Vector3.right * HEX_TILE_WIDTH / 2f
                        + Vector3.up * HEX_TILE_HEIGTH / 2f
                        + (Vector3.right * i * 0.79f) * HEX_TILE_WIDTH
                        + (Vector3.up * j) * HEX_TILE_HEIGTH
                        + (Vector3.down * (HEX_TILE_HEIGTH - 1f))
                        + (Vector3.left * (HEX_TILE_WIDTH - 1f))
                        + (i % 2 == 0 ? Vector3.zero : Vector3.up * HEX_TILE_HEIGTH * 0.49f);

                    GameTiles[i][j] = newTile;
                }
            }
        }

        SetTileConnections(GameTiles);
    }

    private HexTile[] SearchOtherHexes(HexTile p_hexTile){
        List<HexTile> hexesToLook = new List<HexTile>();
        HexTile currentTile = null;
        int index = 0;

        hexesToLook.Add(p_hexTile);

        int infiniteBreaker = 0;

        while(index < hexesToLook.Count && infiniteBreaker < 100){
            currentTile = hexesToLook[index];

            for(int i = 0; i < currentTile.ConnectedTiles.Length; i++){
                if(currentTile.ConnectedTiles[i] != null 
                && currentTile.ConnectedTiles[i].HexTileColor == currentTile.HexTileColor
                && !hexesToLook.Contains(currentTile.ConnectedTiles[i])){
                    hexesToLook.Add(currentTile.ConnectedTiles[i]);
                }
            }

            index++;

            infiniteBreaker++;
        }

        if(infiniteBreaker >= 100){
            Debug.Log("Search infinite");
        }

        return hexesToLook.ToArray();
    }

    private void SetTileConnections(HexTile[][] p_hexTiles){
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
    }
}
