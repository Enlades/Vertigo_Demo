using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    private HexTile[] _selectedHexTiles;
    private HexTile[] _selectionHexTiles;

    public void Init(HexTile p_hexTilePrefab, Color p_selectionTileColor){
        _selectionHexTiles = new HexTile[3];

        for (int i = 0; i < _selectionHexTiles.Length; i++)
        {
            _selectionHexTiles[i] = Instantiate(p_hexTilePrefab);
            _selectionHexTiles[i].GetComponent<Collider2D>().enabled = false;
            _selectionHexTiles[i].SetColor(p_selectionTileColor, -1);
            _selectionHexTiles[i].transform.localScale *= 1.32f;
            _selectionHexTiles[i].gameObject.SetActive(false);
        }
    }

    public Vector2 GetSelectionPosition(){
        if(_selectedHexTiles != null){
            return _selectedHexTiles[0].transform.position;
        }else{
            return Vector2.zero;
        }
    }

    public HexTile[] GetSelectedTiles(){
        return _selectedHexTiles;
    }

    public void HandleSelection(Vector3 p_inputPosition, HexTile[] p_candidateTiles){
        HexTile selectedTile = null;
        float distance = float.MaxValue;

        for(int i = 0; i < p_candidateTiles.Length; i++){
            float currentDistance = Vector2.Distance(p_inputPosition, p_candidateTiles[i].transform.position);
            if(currentDistance < distance){
                selectedTile = p_candidateTiles[i];
                distance = currentDistance;
            }
        }

        HexTileDirection selectionDirection = FindSelectionDirection(p_inputPosition, selectedTile);

        _selectedHexTiles = GetTilesFromConnectionDirection(selectionDirection, selectedTile);

        ConstructSelectionTiles(_selectedHexTiles);
    }

    public void DeSelect(){
        for (int i = 0; i < _selectionHexTiles.Length; i++)
        {
            _selectionHexTiles[i].gameObject.SetActive(false);
        }

        _selectedHexTiles = null;
    }

    private HexTileDirection FindSelectionDirection(Vector3 p_inputPosition, HexTile p_selectedTile){
        float distance = float.MaxValue;
        int selectedIndex = -1;
        for(int i = 0; i < p_selectedTile.ConnectedTiles.Length; i++){
            if(p_selectedTile.ConnectedTiles[i] == null){
                continue;
            }
            
            float currentDistance = Vector3.Distance(p_inputPosition, p_selectedTile.ConnectedTiles[i].transform.position);

            if(currentDistance < distance){
                distance = currentDistance;
                selectedIndex = i;
            }
        }

        return (HexTileDirection)selectedIndex;
    }

    private HexTile[] GetTilesFromConnectionDirection(HexTileDirection p_selectionDirection, HexTile p_selectedTile){
        HexTile[] result = new HexTile[3];

        result[0] = p_selectedTile;

        while (p_selectedTile.ConnectedTiles[(int)p_selectionDirection] == null)
        {
            p_selectionDirection = GetNextClockwise(p_selectionDirection);
        }

        result[1] = p_selectedTile.ConnectedTiles[(int)p_selectionDirection];

        while (p_selectedTile.ConnectedTiles[(int)p_selectionDirection] == null
            || !p_selectedTile.ConnectedTiles[(int)p_selectionDirection].HasConnection(result[1]))
        {
            p_selectionDirection = GetNextClockwise(p_selectionDirection);
        }

        result[2] = p_selectedTile.ConnectedTiles[(int)p_selectionDirection];

        return result;
    }

    private HexTileDirection GetNextClockwise(HexTileDirection p_direction){
        if(p_direction == HexTileDirection.NorthWest){
            return HexTileDirection.North;
        }else{
            return (HexTileDirection)(((int)p_direction) + 1);
        }
    }

    private void ConstructSelectionTiles(HexTile[] p_selectedTiles)
    {
        for(int i = 0; i < p_selectedTiles.Length; i++){
            _selectionHexTiles[i].transform.position = p_selectedTiles[i].transform.position
             + Vector3.forward * 1f;
            _selectionHexTiles[i].gameObject.SetActive(true);
        }
    }
}
