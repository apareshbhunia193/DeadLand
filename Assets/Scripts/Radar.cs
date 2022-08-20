using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarObject { 
    public Image icon { get; set; }
    public GameObject owner { get; set; }
}

public class Radar : MonoBehaviour
{

    public Transform playerPos;
    public float mapScale = 2.0f;
    public static List<RadarObject> radarObjects = new List<RadarObject>();

    public static void RegisterRadarObject(GameObject o, Image i) {
        Image image = Instantiate(i);
        radarObjects.Add(new RadarObject() { owner = o, icon = image});
    }

    public static void RemoveRadarObject(GameObject o) {
        List<RadarObject> newList = new List<RadarObject>();
        for (int i = 0; i < radarObjects.Count; i++) {
            if (radarObjects[i].owner == o)
            {
                Destroy(radarObjects[i].icon);
            }
            else {
                newList.Add(radarObjects[i]);
            }
        }

        radarObjects.RemoveRange(0, radarObjects.Count);
        radarObjects.AddRange(newList);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos == null) return;
        foreach (RadarObject ro in radarObjects) {
            Vector3 radPos = ro.owner.transform.position - playerPos.position;
            float distanceToObject = Vector3.Distance(playerPos.position, ro.owner.transform.position) * mapScale;

            float deltay = Mathf.Atan2(radPos.x, radPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
            radPos.x = distanceToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radPos.z = distanceToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            ro.icon.transform.SetParent(this.transform);
            RectTransform rt = GetComponent<RectTransform>();
            ro.icon.transform.position = new Vector3(radPos.x + rt.pivot.x, radPos.z + rt.pivot.y, 0) + transform.position;
        }
    }
}
