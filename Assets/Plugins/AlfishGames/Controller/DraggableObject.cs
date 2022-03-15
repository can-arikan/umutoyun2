using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AlfishGames/Controller/Draggable Object")]

public class DraggableObject: MonoBehaviour
{
    private Vector3 dist, startPos;
    private float posX, posZ, posY;

    private void OnMouseDown()
    {
        startPos = transform.position;
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
        posZ = Input.mousePosition.z - dist.z;
    }

    private void OnMouseDrag()
    {
        float disX = Input.mousePosition.x - posX;
        float disY = Input.mousePosition.y - posY;
        float disZ = Input.mousePosition.z - posZ;
        Vector3 lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
        transform.position = new Vector3(lastPos.x, startPos.y, lastPos.z);
    }

    private void OnMouseUp()
    {
        
    }
}
