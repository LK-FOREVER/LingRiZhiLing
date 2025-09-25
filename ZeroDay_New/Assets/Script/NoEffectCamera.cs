using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoEffectCamera : MonoBehaviour
{
    /// <summary>
    /// 空后期处理（没有处理的后期处理）
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination);
    }
}
