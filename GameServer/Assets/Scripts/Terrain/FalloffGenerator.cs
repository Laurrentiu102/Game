﻿using UnityEngine;
using System.Collections;

public static class FalloffGenerator {

	public static float[,] GenerateFalloffMap(int size, Vector2 sampleCentre = default(Vector2)) {
		float[,] map = new float[size,size];
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				float x = (i + sampleCentre.x + size/3) / (float)size * 2 - 1;
				float y = (j - sampleCentre.y + size/3) / (float)size * 2 - 1;

				float value = Mathf.Max (Mathf.Abs (x), Mathf.Abs (y));
				map[i, j] = Evaluate(value);
			}
		}

		return map;
	}

	static float Evaluate(float value) {
		float a = 3;
		float b = 2.2f;

		return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
}
