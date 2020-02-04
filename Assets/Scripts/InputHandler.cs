using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    private Action<Vector3, HexTile[]> _selectionAction;
    private Action<bool> _swipeAction;
    private Func<Vector2> _transferSelectionPositionFunc;

    private int _hexTileLayerMask;
    
    private Vector2 _firstTouchPosition;

    public void Init(int p_hexTileLayerMask, Action<Vector3, HexTile[]> p_selectionAction
    , Func<Vector2> p_transferSelectionPositionFunc, Action<bool> p_swipeAction){

        _hexTileLayerMask = p_hexTileLayerMask;

        _selectionAction = p_selectionAction;
        _swipeAction = p_swipeAction;
        _transferSelectionPositionFunc = p_transferSelectionPositionFunc;
    }

    public void Tick(){
        if(Input.GetMouseButtonDown(0)){
            _firstTouchPosition = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0)){
            if(Vector2.Distance(_firstTouchPosition, Input.mousePosition) < 10f){
                HexTile[] selectedHexTiles = null;
                Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bool hexHit = SelectHexTile(inputPosition, out selectedHexTiles);

                if (hexHit)
                {
                    if (_selectionAction != null)
                    {
                        _selectionAction.Invoke(inputPosition, selectedHexTiles);
                    }
                }
            }else{
                if(_transferSelectionPositionFunc != null){
                    Vector2 selectionPosition = _transferSelectionPositionFunc.Invoke();
                    Vector2 swipeVectorStart = Camera.main.ScreenToWorldPoint(_firstTouchPosition);
                    Vector2 swipeVectorEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    Vector2 selectionToStart = swipeVectorStart - selectionPosition;
                    Vector2 selectionToEnd = swipeVectorEnd - selectionPosition;

                    float signedAngle = Vector2.SignedAngle(selectionToStart, selectionToEnd);

                    if (_swipeAction != null)
                    {
                        if (signedAngle < 0f)
                        {
                            _swipeAction.Invoke(true);
                        }
                        else
                        {
                            _swipeAction.Invoke(false);
                        }
                    }
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
