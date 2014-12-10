using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public GameObject player;
	
	public float spinningRadius = 2f;
	public float spinningHeight = 1f;

	// summon all selected swords and spin around player
	public void SummonSwords() {
		List<GameObject> selectedSwords = new List<GameObject>();
		GameObject[] swords = GameObject.FindGameObjectsWithTag("Sword");

		// get selected swords count
		foreach(GameObject sword in swords) {
			SwordController sc = (SwordController) sword.GetComponent<SwordController>();
			// we don't want to trigger summon when there's swords spinning or inHand
			if(sc.GetState () == SwordState.Spinning 
			   || sc.GetState() == SwordState.InHand) {
				return;
			} else if(sc.GetState() == SwordState.Selected) {
				selectedSwords.Add(sword);
			}
		}
		
		// circle distribution of swords
		List<Vector3> positions = GetEvenDistributionOnCircle(player.transform
		                                                      , selectedSwords.Count
		                                                      , spinningHeight
		                                                      , spinningRadius);
		
		// distribute swords
		for(int i = 0; i < selectedSwords.Count; ++i) {
			GameObject sword = selectedSwords[i];
			Vector3 oldPos = sword.transform.position;
			sword.transform.position = positions[i];
			SwordController sc = sword.GetComponent<SwordController>();

			// self-rotating
			sc.StartRotate();
			
			// orbit
			sc.StartOrbit();
			
			Debug.Log (positions[i]);
		}
	}

	public void ChooseSword(string swordName) {
		GameObject[] swords = GameObject.FindGameObjectsWithTag("Sword");
		foreach(GameObject sword in swords) {
			if(!sword.name.Equals(swordName)) {
				// destroy abandoned swords
				Destroy(sword.gameObject);
			}
		}
	}

	// distribute swords evenly on the circle
	List<Vector3> GetEvenDistributionOnCircle(Transform playerTransform, int num, float spinningHeight, float spinningRadius) {
		List<Vector3> list = new List<Vector3>();
		
		if(num == 0)
			return list;
		
		for(float i = 0f; i < num; i = i + 1.0f) {
			float progress = i / num;
			float angle = progress * Mathf.PI * 2f;
			var x = Mathf.Sin (angle) * spinningRadius;
			var z = Mathf.Cos (angle) * spinningRadius;
			
			Vector3 pos = new Vector3(x, spinningHeight, z);
			pos += playerTransform.position;
			list.Add (pos);
		}
		return list;
	}
}
