using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Color HexTileColor{get; private set;}
    public HexTile[] ConnectedTiles{get; private set;}

    private SpriteRenderer _spriteRenderer;
    
    private void Awake(){
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ConnectedTiles = new HexTile[6];
    }
    
    public void Init(Color p_color){
        HexTileColor = p_color;
        _spriteRenderer.color = p_color;
    }
    
    public void SetConnection(HexTileDirection p_direction, HexTile p_tile){
        if(ConnectedTiles[(int)p_direction] != null){
            Debug.LogWarning("Overwriting an already existing Hex connection");
            return;
        }

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
}