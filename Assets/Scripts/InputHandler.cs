using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private int _hexTileLayerMask;

    public void Init(int p_hexTileLayerMask){
        _hexTileLayerMask = p_hexTileLayerMask;
    }

    private void Update(){
        if(Input.GetMouseButtonDown(0)){

        }
    }

    public bool SelectHexTile(Vector3 p_inputPosition, out HexTile p_hexTile){
        p_hexTile = null;

        RaycastHit2D hit = Physics2D.Raycast(p_inputPosition, Vector2.zero, 10f, _hexTileLayerMask);

        if (hit.collider != null)
        {
            p_hexTile = hit.collider.GetComponent<HexTile>();
            return true;
        }else{
            return false;
        }
    }
}
