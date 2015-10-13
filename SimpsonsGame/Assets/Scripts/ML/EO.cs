using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.IO;

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
		string populationFilePath,
		double mutateChance,
		double mutateMaxFactor,
		double migrationPercentage,
		double childrenPercentage,
		int generations,
		Action<ANNTrainer, Action<double>> train
	) {

		System.IO.StreamReader reader = new System.IO.StreamReader(populationFilePath);

		List<ANN> population = new List<ANN>();

		string line;
		while ((line = reader.ReadLine()) != null) {

			const string biasesToken = "ANN, biases:[";
			const string weightsToken = "], weights:[";

			int biasesBegin = 0;
			int biasesEnd = biasesBegin + biasesToken.Length;
			int weightsBegin = line.IndexOf(weightsToken);
			int weightsEnd = weightsBegin + weightsToken.Length;
			int lastIndex = line.Length - (line.Last() == ',' ?  2 : 1);

			population.Add(
				new ANN(
					line.Substring(biasesEnd, weightsBegin - biasesEnd),
					line.Substring(weightsEnd, lastIndex - weightsEnd)
				)
			);
		}

		reader.Close();

		_initialize(mutateChance, mutateMaxFactor, migrationPercentage, childrenPercentage, generations, train, population);
	}

	public EO(
		int populationCount,
		List<int> layers,
		double mutateChance,
		double mutateMaxFactor,
		double migrationPercentage,
		double childrenPercentage,
		int generations,
		Action<ANNTrainer, Action<double>> train
	){

		List<ANN> population = new List<ANN>();

		for (int i = 0; i < populationCount; ++i) {

			population.Add(new ANN(layers));
		}

		_initialize(mutateChance, mutateMaxFactor, migrationPercentage, childrenPercentage, generations, train, population);
	}

	private void _initialize(
		double mutateChance,
		double mutateMaxFactor,
		double migrationPercentage,
		double childrenPercentage,
		int generations,
		Action<ANNTrainer, Action<double>> train,
		List<ANN> population
	) {
		
		string uniqueId = "mc" + ((int) (mutateChance * 100)) +
			"mmf" + ((int) (mutateMaxFactor * 100)) +
				"mp"  + ((int) (migrationPercentage * 100)) +
				"cp"  + ((int) (childrenPercentage * 100)) +
				"g"  + generations + "p" + population.Count();
		
		int numMigrate = Mathf.RoundToInt((float) migrationPercentage * population.Count());
		int numChildren = Mathf.RoundToInt((float) childrenPercentage * population.Count());
		
		System.IO.File.WriteAllText(
			@"C:\Users\Public\EO\" + uniqueId + "generation0.txt",
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
				
				// Add migrants
				for (int i = 1; i <= numMigrate; ++i) {
					Debug.Log("Adding migrant at " + (trained.Count() - i));
					trained[trained.Count() - i] = new ANNTrainer(new ANN(best.nn.getLayers()));
				}
				for (int i = 1; i <= numChildren; ++i) {
					Debug.Log("Adding child at " + (trained.Count() - i - numMigrate));
					trained[trained.Count() - i - numMigrate] = this.haveChild(
						best,
						second,
						mutateChance,
						mutateMaxFactor
					);
				}
				
				population = trained.ConvertAll<ANN>(trainer => trainer.nn);
				
				System.IO.File.WriteAllText(
					@"C:\Users\Public\EO\" + uniqueId + "generation" + gen + ".txt",
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
				Debug.Log("Training Generation " + gen + " Pop " + pop);
				ANNTrainer trainer = new ANNTrainer(population[pop]);
				
				train(
					trainer,
					(double score) => {
					Debug.Log ("Pop " + pop + " scored " + score);
					trainer.score = score;
					trained.Add(trainer);
					
					if (pop + 1 < population.Count()) {
						
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
		double mutateChance,
		double mutateMaxFactor
	) {

		ANNTrainer child = new ANNTrainer(
			new ANN(
				parent1.nn.getLayers()
			)
		);

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
