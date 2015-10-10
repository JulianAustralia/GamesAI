using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

private static Random r = new Random();

private bool chance(double c) {

	return r.NextDouble() <= c;
}

private double range(double maxmin) {

	return (r.NextDouble() - .5) * 2 * maxmin;
}

public class ANNTrainer {

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

		ANN bestBiases = best.nn._biases.map(
			(int row, int column, double b) => {

				return chance(crossOverChance) ? second.nn._biases.getValue(row, column) : b;
			}
		);

		ANN bestWeights = best.nn._weights.map (
			(int row, int column, double w) => {
			
				return chance(crossOverChance) ? second.nn._weights.getValue(row, column) : w;
			}
		);

		ANN secondBiases = second.nn._biases.map(
			(int row, int column, double b) => {

				return chance(crossOverChance) ? best.nn._biases.getValue(row, column) : b;
			}
		);

		ANN secondWeights = second.nn._weights.map (
			(int row, int column, double w) => {

				return chance(crossOverChance) ? best.nn._weights.getValue(row, column) : w;
			}
		);

		best.nn._biases = bestBiases;
		best.nn._weights = bestWeights;
		second.nn._biases = secondBiases;
		second.nn._weights = secondWeights;
	}

	private void mutate(ref ANNTrainer trainer, double mutateChance, double mutateMaxFactor) {
		
		trainer.nn._biases = trainer.nn._biases.map (
			(double b) => {

				return chance(mutateChance) ? b + range(mutateMaxFactor) : b;
			}
		);

		trainer.nn._wights = trainer.nn._wights.map(
			(double b) => {

				return chance(mutateChance) ? b + range(mutateMaxFactor) : b;
			}
		);
	}
}
