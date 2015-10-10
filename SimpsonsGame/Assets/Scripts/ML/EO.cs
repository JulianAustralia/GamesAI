using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class ANNTrainer {

	public ANNTrainer(ANN nn) {

		this.nn = nn;
		this.score = 0;
	}

	public ANN nn;
	public double score;
}

public class EO {

	public EO(
		int populationCount,
		List<int> layers,
		double crossOverChance,
		double mutateChance,
		double mutateMaxFactor,
		int generations,
		Func<ANNTrainer, ANNTrainer> train
	){

		List<ANN> population = Enumerable.Range(
			0,
			populationCount
		).Select(
			(x) => new ANN(layers)
		);

		for (int i = 0; i < generations; ++i) {

			List<ANNTrainer> trained = population.Select(
				(ANN) => train(new ANNTrainer(ANN))
			);

			trained.Sort(
				(ANNTrainer a, ANNTrainer b) => a.score.CompareTo(b.score)
			);

			ANNTrainer best = trained[0];
			ANNTrainer second = trained[1];

			this.crossOver(ref best, ref second, crossOverChance);

			this.mutate(ref best, mutateChance, mutateMaxFactor);
			this.mutate(ref second, mutateChance, mutateMaxFactor);

			trained[trained.Count() - 1] = new ANNTrainer(new ANN(layers));

			population = trained.Select(
				(ANNTrainer t) => t.nn
			);
		}
	}

	private void crossOver(ref ANNTrainer best, ref ANNTrainer second, double crossOverChance) {

		Random r = new Random();

		ANN bestBiases = best.nn._biases.map(
			(int row, int column, double b) => {

				bool crossOver = r.NextDouble() <= crossOverChance;
			 
				return crossOver ? second.nn._biases.getValue(row, column) : b;
			}
		);

		ANN bestWeights = best.nn._weights.map (
			(int row, int column, double w) => {
			
				bool crossOver = r.NextDouble() <= crossOverChance;
			
				return crossOver ? second.nn._weights.getValue(row, column) : w;
			}
		);

		ANN secondBiases = second.nn._biases.map(
			(int row, int column, double b) => {

				bool crossOver = r.NextDouble() <= crossOverChance;
			 
				return crossOver ? best.nn._biases.getValue(row, column) : b;
			}
		);

		ANN secondWeights = second.nn._weights.map (
			(int row, int column, double w) => {
			
				bool crossOver = r.NextDouble() <= crossOverChance;
				
				return crossOver ? best.nn._weights.getValue(row, column) : w;
			}
		);

		best.nn._biases = bestBiases;
		best.nn._weights = bestWeights;
		second.nn._biases = secondBiases;
		second.nn._weights = secondWeights;
	}
}
