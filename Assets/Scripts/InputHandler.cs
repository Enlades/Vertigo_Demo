using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    private Action<Vector3, HexTile[]> _selectionAction;

    private int _hexTileLayerMask;

    public void Init(int p_hexTileLayerMask, Action<Vector3, HexTile[]> p_selectionAction){
        _hexTileLayerMask = p_hexTileLayerMask;

        _selectionAction = p_selectionAction;
    }

    public void Tick(){
        if(Input.GetMouseButtonUp(0)){
            HexTile[] selectedHexTiles = null;
            Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool hexHit = SelectHexTile(inputPosition, out selectedHexTiles);

            if (hexHit)
            {
                if(_selectionAction != null){
                    _selectionAction.Invoke(inputPosition, selectedHexTiles);   
                }
            }
        }
    }

    private bool SelectHexTile(Vector3 p_inputPosition, out HexTile[] p_hexTiles){
        p_hexTiles = null;

        RaycastHit2D[] hit = Physics2D.RaycastAll(p_inputPosition, Vector2.zero, 10f, _hexTileLayerMask);

        if (hit.Length > 0)
        {
            p_hexTiles = new HexTile[hit.Length];

            for(int i = 0; i < p_hexTiles.Length; i++){
                p_hexTiles[i] = hit[i].transform.GetComponent<HexTile>();
            }
            return true;
        }else{
            return false;
        }
    }
}
