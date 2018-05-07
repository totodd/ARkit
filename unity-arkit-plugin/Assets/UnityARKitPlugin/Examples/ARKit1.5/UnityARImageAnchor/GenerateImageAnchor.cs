using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;

public class GenerateImageAnchor : MonoBehaviour {


	[SerializeField]
	private ARReferenceImage referenceImage;

	[SerializeField]
	private GameObject prefabToGenerate;

	private GameObject imageAnchorGO, TempanchorGO;

	private UnityARAnchorManager unityARAnchorManager;

	// On GUI
	public Text PathCoordinates, camCoordinates;
	public Text PathRotation, camRotation;
	public Camera maincamera;
	public float referenceD;

	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
		referenceD = 1.2f;

	}
	// Q1: Scan angle will lead to different generation
	void AddImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor added");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			Vector3 newposition = new Vector3 (position.x, position.y-referenceD, position.z);
			Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			Quaternion newrotation = Quaternion.Euler (rotation.x, rotation.y-90, rotation.z);

			imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, newposition, newrotation);
			// new edited
			Debug.Log (newposition);
			PathCoordinates.text = imageAnchorGO.transform.position.ToString ();
			PathRotation.text = rotation.ToString ();

		}
	}

	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor updated");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			Vector3 newposition = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			Quaternion newrotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			imageAnchorGO.transform.position = new Vector3 (newposition.x, newposition.y - referenceD, newposition.z);
			imageAnchorGO.transform.rotation = Quaternion.Euler (newrotation.x, newrotation.y-90, newrotation.z);
			PathCoordinates.text = imageAnchorGO.transform.position.ToString ();
			PathRotation.text = imageAnchorGO.transform.rotation.ToString ();
		}

	}

	void RemoveImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor removed");
		if (imageAnchorGO) {
			GameObject.Destroy (imageAnchorGO);
		}

	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;
		unityARAnchorManager.Destroy ();

	}


	void update()
	{
		PathCoordinates.text = imageAnchorGO.transform.position.ToString ();
		PathRotation.text = imageAnchorGO.transform.rotation.ToString ();
		camCoordinates.text = maincamera.transform.position.ToString ();
		camRotation.text = maincamera.transform.rotation.ToString ();
	}

}
