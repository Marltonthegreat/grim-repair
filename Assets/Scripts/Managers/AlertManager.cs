using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour
{

    public Dictionary<Transform,GameObject> breachAlerts = new Dictionary<Transform, GameObject>();
    public List<Transform> enemyTransforms = new List<Transform>();

    public GameObject defaultBreach;

    public GameObject breachAlertPrefab;
    public GameObject enemyAlertPrefab;

    // Start is called before the first frame update
    void Start()
    {
        breachAlerts.Add(defaultBreach.transform, null);
    }

    // Update is called once per frame
    void Update()
    {
        displayBreachAlert();
    }

    void displayBreachAlert()
    {
        print("Display Breach Start");
        print(breachAlerts.Count);
        
        foreach (KeyValuePair<Transform,GameObject> t in breachAlerts)
        {
            RaycastHit hit;
            Vector3 pos = Camera.main.WorldToScreenPoint(t.Key.position);
            if (!t.Value)
            {
                if (!Screen.safeArea.Contains(pos))
                {
                    pos = new Vector3(Mathf.Clamp(pos.x, Screen.safeArea.xMin, Screen.safeArea.xMax), Mathf.Clamp(pos.y, Screen.safeArea.yMin, Screen.safeArea.yMax));
                }
                pos = Camera.main.ScreenToWorldPoint(pos);
                GameObject alert = Instantiate(breachAlertPrefab, pos + new Vector3(0, 2.5f, 0), t.Key.rotation);
                breachAlerts[t.Key] = alert;
            }
            else
            {
                if (!Screen.safeArea.Contains(pos))
                {
                    pos = new Vector3(Mathf.Clamp(pos.x, Screen.safeArea.xMin, Screen.safeArea.xMax), Mathf.Clamp(pos.y, Screen.safeArea.yMin, Screen.safeArea.yMax));
                    pos = Camera.main.ScreenToWorldPoint(pos);
                    t.Value.transform.position = pos;
                }
            }

            
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(t.Key.position));
            
            Debug.DrawRay(ray.origin, ray.direction* 10);
            if (Physics.Raycast(ray, out hit))
            {
                print("Display Breach Raycast");
                if (hit.collider != null)
                {
                    print("Display Breach Collider Hit");
                }
            }

        }
        print("Display Breach End");
    }

    void displayEnemyAlert()
    {

    }
}
