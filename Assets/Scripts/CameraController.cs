using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float CheckRadius = 0.2f;

    public GameObject Lense;
    public GameObject Pointer;

    public GameObject Ghost;
    public Camera camera;

    public Color HighlightColor = Color.red;
    public Color DefaultColor = Color.black;

    public Text Health;
    public Button trigger;
    public Button end;

    public int GhostHealth = 10;

    private void Start()
    {
        trigger.onClick.AddListener(TriggerClick);
    }

    private void TriggerClick()
    {
        GhostHealth--;
        if (GhostHealth == 0)
        {
            end.gameObject.SetActive(true);
            end.onClick.AddListener(EndClick);
        }
    }

    private void EndClick()
    {
        GhostHealth = 10;
        end.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Health.text = GhostHealth.ToString();
        // https://www.youtube.com/watch?v=8-KFyiSFuxw
        /*
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.SphereCast(ray, CheckRadius, out hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.root.gameObject == Ghost.gameObject)
            {
                Lense.GetComponent<SpriteRenderer>().color = HighlightColor;
                Pointer.GetComponent<SpriteRenderer>().color = HighlightColor;
            }
        }
        else
        {
            Lense.GetComponent<SpriteRenderer>().color = DefaultColor;
            Pointer.GetComponent<SpriteRenderer>().color = DefaultColor;
        }*/
    }
}
