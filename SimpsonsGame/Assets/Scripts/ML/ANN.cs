using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ANN {

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

	public ANN(string biases, string weights) {

		const string beginToken = "[[";
		const string middleToken = ",";
		const string endToken = "]]";
		const string splitter = endToken + middleToken + beginToken;

		this._biases = biases.Substring(
			beginToken.Length + 1,
			biases.Length - beginToken.Length - endToken.Length - 1
		).Split(
			new[] { splitter },
			System.StringSplitOptions.None
		).ToList().ConvertAll<Matrix>(s => new Matrix(beginToken + s + endToken));

		this._weights = weights.Substring(
			beginToken.Length + 1,
			weights.Length - beginToken.Length - endToken.Length - 1
		).Split(
			new[] { splitter },
			System.StringSplitOptions.None
		).ToList().ConvertAll<Matrix>(s => new Matrix(beginToken + s + endToken));
	}

	public List<int> getLayers() {

		List<int> layers = this._biases.ConvertAll<int>(b => b.getRows());
		layers.Insert(
			0,
			this._weights.First().getColumns()
		);

		return layers;
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
