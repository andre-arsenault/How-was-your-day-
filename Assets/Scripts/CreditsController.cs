using UnityEngine;
using System.Collections;

public class CreditsController : MonoBehaviour
{
	public bool isCrawling = true;
	public float speed = 0.1f;
	public float endY = 2f;

    // Use this for initialization
    void Start()
    {
		GUIText gui = GetComponent<GUIText>();
		TextAsset credits = Resources.Load<TextAsset>("Credits");
		gui.text = credits.text;
    }

    // Update is called once per frame
    void Update()
    {
		if (!isCrawling)
			return;

		transform.Translate(Vector3.up * Time.deltaTime * speed);
		if (gameObject.transform.position.y > endY)
		{
			isCrawling = false;
		}
	}
}