using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Color HexTileColor{get; private set;}
    private HexTile[] _connectedTiles;

    private SpriteRenderer _spriteRenderer;
    
    private void Awake(){
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _connectedTiles = new HexTile[6];
    }
    
    public void Init(Color p_color){
        HexTileColor = p_color;
        _spriteRenderer.color = p_color;
    }
    public void SetConnection(HexTileDirection p_direction, HexTile p_tile){
        if(_connectedTiles[(int)p_direction] != null){
            Debug.LogWarning("Overwriting an already existing Hex connection");
            return;
        }

        _connectedTiles[(int)p_direction] = p_tile;
    }

    public HexTile GetConnectedTile(HexTileDirection p_direction){
        return _connectedTiles[(int)p_direction];
    }
}