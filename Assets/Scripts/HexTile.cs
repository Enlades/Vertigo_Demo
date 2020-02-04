using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Color HexTileColor{get; private set;}
    public HexTile[] ConnectedTiles{get; private set;}
    public Vector2Int BoardPosition{get; private set;}

    private SpriteRenderer _spriteRenderer;
    
    private void Awake(){
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ConnectedTiles = new HexTile[6];
    }

    public void SetBoardPosition(Vector2Int p_position){
        name = "HexTile_" + p_position.x + "_" + p_position.y;
        BoardPosition = p_position;
    }
    
    public void SetColor(Color p_color){
        HexTileColor = p_color;
        _spriteRenderer.color = p_color;
    }
    
    public void SetConnection(HexTileDirection p_direction, HexTile p_tile){
        ConnectedTiles[(int)p_direction] = p_tile;
    }

    public HexTile GetConnectedTile(HexTileDirection p_direction){
        return ConnectedTiles[(int)p_direction];
    }

    public bool HasConnection(HexTile p_hexTile){
        for(int i = 0; i < ConnectedTiles.Length; i++){
            if(ConnectedTiles[i] == p_hexTile){
                return true;
            }
        }

        return false;
    }

    public static void HexTilesSwap(HexTile[] p_hexTiles, bool p_isClockwise)
    {
        if (p_isClockwise)
        {
            Vector3 firstTilePosition = p_hexTiles[0].transform.position;
            Vector2Int firstTileBoardPosition = p_hexTiles[0].BoardPosition;
            for (int i = 0; i < p_hexTiles.Length - 1; i++)
            {
                p_hexTiles[i].transform.position = p_hexTiles[i + 1].transform.position;
                p_hexTiles[i].BoardPosition = p_hexTiles[i + 1].BoardPosition;
            }

            p_hexTiles[p_hexTiles.Length - 1].transform.position = firstTilePosition;
            p_hexTiles[p_hexTiles.Length - 1].BoardPosition = firstTileBoardPosition;
        }
        else
        {
            Vector3 lastTilePosition = p_hexTiles[p_hexTiles.Length - 1].transform.position;
            Vector2Int lastTileBoardPosition = p_hexTiles[p_hexTiles.Length - 1].BoardPosition;
            for (int i = p_hexTiles.Length - 1; i > 0; i--)
            {
                p_hexTiles[i].transform.position = p_hexTiles[i - 1].transform.position;
                p_hexTiles[i].BoardPosition = p_hexTiles[i - 1].BoardPosition;
            }

            p_hexTiles[0].transform.position = lastTilePosition;
            p_hexTiles[0].BoardPosition = lastTileBoardPosition;
        }
    }
}