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
	public List<Matrix> _biases;
	public List<Matrix> _weights;

	public ANN(List<int> layers){

		// First layer is input, does not need biases
		this._biases = Enumerable.Range(
			1,
			layers.Count() - 1
		).Select(
			(int layerIndex) => {
			
				int size = layers[layerIndex];
			
				return Matrix.RandomDist(size, 1);
			}
		);

		this._weights = Enumerable.Range(
			1,
			layers.Count() - 1
		).Select(
			(int nextLayerIndex) => {

				int thisSize = layers[nextLayerIndex - 1];
				int nextSize = layers[nextLayerIndex];

				return Matrix.RandomDist(nextSize, thisSize) / Mathf.Sqrt(thisSize);
			}
		);
	}

	public List<Matrix> feedForward(Matrix input) {

		List<Matrix> activations;

		activations.Add(input);

		for (int layerIndex = 0; layerIndex < this._biases.Count(); ++layerIndex) {

			Matrix biase = this._biases[layerIndex];
			Matrix weight = this._weights[layerIndex];

			Matrix activation = (
				weight.dotProduct(activations.Last()) + biase
			).map(
				// sigmoid
				(d) => 1 / (1 + Mathf.Exp(-d))
			);

			activations.Add(activation);
		}

		return activations;
	}

	public override string ToString () {

		return "ANN, biases:[" +
			_biases.Select((m) => m.ToString()).Join(",") +
			"], weights:[" +
			_weights.Select((m) => m.ToString()).Join(",") +
			"]";
	}
}
