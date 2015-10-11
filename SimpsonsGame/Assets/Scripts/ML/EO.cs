using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

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
		double mutateChance,
		double mutateMaxFactor,
		int generations,
		Action<ANNTrainer, Action<double>> train
	){

		string timestamp = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff");

		List<ANN> population = new List<ANN>();

		for (int i = 0; i < populationCount; ++i) {

			population.Add(new ANN(layers));
		}
		
		System.IO.File.WriteAllText(
			@"C:\Users\Public\EO\" + timestamp + "generation0.txt",
			String.Join(
				",\n",
				population.Select(
					nn => nn.ToString()
				).ToArray()
			)
		);

		// This lambda soup is to allow the training to be asynchronous,
		// I tried using C#'s await, yeild and Task, but was unable to get
		// the desired behvaiour;
		Action<int> trainGeneration;

		trainGeneration = (int gen) => {

			List<ANNTrainer> trained = new List<ANNTrainer>();

			Action populationFinished = () => {

				// Biggest to smalest
				trained.Sort((a, b) => b.score.CompareTo(a.score));
				
				ANNTrainer best = trained[0];
				ANNTrainer second = trained[1];

				// Add two children
				trained[trained.Count() - 3] = this.haveChild(best, second, layers, mutateChance, mutateMaxFactor);
				trained[trained.Count() - 2] = this.haveChild(best, second, layers, mutateChance, mutateMaxFactor);
				// Add imigrant
				trained[trained.Count() - 1] = new ANNTrainer(new ANN(layers));

				population = trained.ConvertAll<ANN>(trainer => trainer.nn);

				System.IO.File.WriteAllText(
					@"C:\Users\Public\EO\" + timestamp + "generation" + gen + ".txt",
					String.Join(
						",\n",
						population.Select(
							nn => nn.ToString()
						).ToArray()
					)
				);

				if (gen + 1 < generations) {

					trainGeneration(gen + 1);
				}
			};

			Action<int> trainPopulation;

			trainPopulation = (int pop) => {

				ANNTrainer trainer = new ANNTrainer(population[pop]);

				train(
					trainer,
					(double score) => {

						trainer.score = score;
						trained.Add(trainer);

						if (pop + 1 < populationCount) {

							trainPopulation(pop + 1);
						} else {

							populationFinished();
						}
					}
				);
			};

			trainPopulation(0);
		};

		trainGeneration(0);
	}

	private ANNTrainer haveChild(
		ANNTrainer parent1,
		ANNTrainer parent2, 
		List<int> layers,
		double mutateChance,
		double mutateMaxFactor
	) {

		ANNTrainer child = new ANNTrainer(new ANN(layers));

		for (int i = 0; i < parent1.nn._biases.Count(); ++i) {

			child.nn._biases[i] = Rand.Chance(.5) ? parent1.nn._biases[i] : parent2.nn._biases[i];
		}

		for (int i = 0; i < parent1.nn._weights.Count(); ++i) {

			child.nn._weights[i] = Rand.Chance(.5) ? parent1.nn._weights[i] : parent2.nn._weights[i];
		}

		return mutate(child, mutateChance, mutateMaxFactor);
	}
	
	private ANNTrainer mutate(ANNTrainer trainer, double mutateChance, double mutateMaxFactor) {

		for (int i = 0; i < trainer.nn._biases.Count(); ++i) {

			trainer.nn._biases[i] = trainer.nn._biases[i].map(
				(double b) => {
			
					return Rand.Chance(mutateChance) ? b + Rand.Range(mutateMaxFactor) : b;
				}
			);
		}

		for (int i = 0; i < trainer.nn._weights.Count(); ++i) {

			trainer.nn._weights[i] = trainer.nn._weights[i].map(
				(double w) => {
			
					return Rand.Chance(mutateChance) ? w + Rand.Range(mutateMaxFactor) : w;
				}
			);
		}

		return trainer;
	}

	private class Rand {
		
		private static System.Random r = new System.Random();
		
		public static bool Chance(double c) {

			return Rand.r.NextDouble() <= c;
		}
		
		public static double Range(double minmax) {
			
			return (Rand.r.NextDouble() - .5) * 2 * minmax;
		}
	}
}
