using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ANN {

	private int _numInput;
	private int _numHidden;
	private int _numOutput;

	// I wouldn't normally make this public except that EO needs
	// access to it and I don't see much use in making EO extend ANN
	// or creating getters and setters
	public List<Matrix> _biases = new List<Matrix>();
	public List<Matrix> _weights = new List<Matrix>();

	public ANN(List<int> layers){

		// The first layer does need a biase as that is supplied by the input
		for (int layerIndex = 1; layerIndex < layers.Count(); ++layerIndex) {

			int previousSize = layers[layerIndex - 1];
			int thisSize = layers[layerIndex];

			this._biases.Add(Matrix.RandomDist(thisSize, 1));
			this._weights.Add(Matrix.RandomDist(thisSize, previousSize) / Mathf.Sqrt(previousSize));
		}
	}

	public List<Matrix> feedForward(Matrix input) {

		List<Matrix> activations = new List<Matrix>(){input};

		for (int layerIndex = 0; layerIndex < this._biases.Count(); ++layerIndex) {

			Matrix biase = this._biases[layerIndex];
			Matrix weight = this._weights[layerIndex];

			Matrix activation = (
				weight.dotProduct(activations.Last()) + biase
			).map(
				// a sigmoid from -1 to 1
				(d) => {

					double result = 2 / (1 + Mathf.Exp((float)-d)) - 1;

					return double.IsNaN(result) ? 0 : result;
				}
			);

			activations.Add(activation);
		}

		return activations;
	}

	public Matrix getResult(Matrix input) {

		return feedForward(input).Last();
	}

	public override string ToString () {

		return "ANN, biases:[" +
			string.Join(",", _biases.Select((m) => m.ToString()).ToArray()) +
			"], weights:[" +
			string.Join(",", _weights.Select((m) => m.ToString()).ToArray()) +
			"]";
	}
}
