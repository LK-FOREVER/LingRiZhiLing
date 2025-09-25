using System;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
   private void OnEnable()
   {
      DataNode.anyPanelOpen = true;
   }

   private void OnDisable()
   {
      DataNode.anyPanelOpen = false;
   }
}
