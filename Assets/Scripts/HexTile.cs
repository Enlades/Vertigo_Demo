using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    private HexTile[] _connectedTiles;

    public void SetColor(){

    }

    public void SetConnection(HexTile p_tile, HexTileDirection p_direction){
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