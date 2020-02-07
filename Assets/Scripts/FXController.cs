using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FXController : MonoBehaviour
{
    public void MoveHexTiles(Transform[] p_hexTransforms, bool p_isClockwise, Action p_callback){
        StartCoroutine(SmoothHexRotation(p_hexTransforms, p_isClockwise, p_callback));
    }

    private IEnumerator SmoothHexRotation(Transform[] p_hexTransforms, bool p_isClockwise, Action p_callback){
        float timer = 0.2f;
        float maxTimer = timer;

        Transform[] p_newHexTransforms = new Transform[p_hexTransforms.Length];
        Vector3[] p_hexPositions = new Vector3[p_newHexTransforms.Length];
        Vector3[] p_startPositions = new Vector3[p_newHexTransforms.Length];
        

        if (!p_isClockwise){
            for(int i = 0; i < p_hexTransforms.Length; i++){
                p_newHexTransforms[i] = p_hexTransforms[p_hexTransforms.Length - i - 1];
            }
        }else{
            for (int i = 0; i < p_hexTransforms.Length; i++)
            {
                p_newHexTransforms[i] = p_hexTransforms[i];
            }
        }

        for (int i = 0; i < p_hexPositions.Length; i++)
        {
            p_hexPositions[i] = p_newHexTransforms[i].position;
        }


        while (timer >= 0f)
        {

            for (int i = 0; i < p_hexTransforms.Length; i++)
            {
                if (i == p_newHexTransforms.Length - 1)
                {
                    p_newHexTransforms[i].position = Vector3.Lerp(p_hexPositions[i], p_hexPositions[0], 1 - timer / maxTimer);
                }
                else
                {
                    p_newHexTransforms[i].position = Vector3.Lerp(p_hexPositions[i], p_hexPositions[i + 1], 1 - timer / maxTimer);
                }
            }

            timer -= Time.deltaTime;

            yield return null;
        }

        for (int i = 0; i < p_hexTransforms.Length; i++)
        {
            p_newHexTransforms[i].position = p_hexPositions[i ];
        }

        if(p_callback != null){
            p_callback.Invoke();
        }
    }
}
