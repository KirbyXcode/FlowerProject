using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class PetalBehaviour : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Sprite[] petalSprites;
    private List<GameObject> petalList;
    private List<Vector3> lastPositionList;
    [SerializeField]
    private GameObject petal;
    public int count = 100;
    public float fleeSpeed = 10;
    public float smooth = 1.2f;
    public float fleeDistance = 100;
    public float backDistance = 300;
    private Vector3 pointPosition;

	void Start () 
	{
        LoadSprites();
        CreatePetals();
	}

    private void LoadSprites()
    {
        petalSprites = new Sprite[5];
        petalSprites = Resources.LoadAll<Sprite>("Petals");
    }

    private void CreatePetals()
    {
        petalList = new List<GameObject>();
        lastPositionList = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(petal);
            int index = Random.Range(0, 5);
            go.GetComponent<Image>().sprite = petalSprites[index];
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(Random.Range(-896, 896), Random.Range(-476, 476), 0);
            petalList.Add(go);
            lastPositionList.Add(go.transform.localPosition);
        }
    }

    private void GetPointPosition(Vector2 point)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, point, null, out localPoint);

        pointPosition = new Vector3(localPoint.x, localPoint.y, 0);
    }

    private void PetalFlee(Vector3 pointPosition)
    {
        if (pointPosition == Vector3.zero) return;
        GameObject[] petalArray = petalList.Where(e => Vector3.Distance(pointPosition, e.transform.localPosition) < fleeDistance).ToArray();

        foreach (GameObject petal in petalArray)
        {
            Vector3 offset = petal.transform.localPosition - pointPosition;
            petal.transform.localPosition += offset * Time.deltaTime * fleeSpeed;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetPointPosition(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetPointPosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointPosition = Vector3.zero;
    }

    void Update()
    {
        PetalFlee(pointPosition);

        for (int i = 0; i < petalList.Count; i++)
        {
            
            if (Vector3.Distance(petalList[i].transform.localPosition, pointPosition) > backDistance && petalList[i].transform.localPosition != lastPositionList[i])
            {
                petalList[i].transform.localPosition = Vector3.Lerp(petalList[i].transform.localPosition, lastPositionList[i], Time.deltaTime * smooth);
            }
        }
    }
}
