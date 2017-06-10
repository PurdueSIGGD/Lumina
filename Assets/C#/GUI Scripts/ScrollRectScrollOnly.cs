using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*********************************************************************
 * 
 * ScrollView have click & drag functionality,
 * 
 * this script will disable it. Because I  only want to use mouse
 *      
 **********************************************************************/
public class ScrollRectScrollOnly : ScrollRect {

    public override void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public override void OnDrag(PointerEventData eventData)
    {
        
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
