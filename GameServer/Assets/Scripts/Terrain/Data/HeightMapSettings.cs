using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class HeightMapSettings : UpdatableData {

	public NoiseSettings noiseSettings;
	public int numberOfTreePrefabs;
	public int numberOfTreesPerChunk;

	public bool useFalloff;
	public int falloffMapSize = 300;

	public float heightMultiplier;
	public AnimationCurve heightCurve;

	public float minHeight {
		get {
			return heightMultiplier * heightCurve.Evaluate (0);
		}
	}

	public float maxHeight {
		get {
			return heightMultiplier * heightCurve.Evaluate (1);
		}
	}

	#if UNITY_EDITOR

	protected override void OnValidate() {
		noiseSettings.ValidateValues ();
		base.OnValidate ();
	}
	#endif

}
