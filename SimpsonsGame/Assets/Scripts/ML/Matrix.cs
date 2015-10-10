using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Matrix {

	private int _width;
	private int _height;
	private int _size;
	private double[] _data;
	
	public Matrix(int width, int height, Func<int, int, double> lambda) {
		
		_width = width;
		_height = height;
		_size = width * height;
		_data = new double[_size];
		
		for (int row = 0; row < height; ++row) {
			for (int column = 0; column < width; ++column) {
				
				_data[row * width + column] = lambda(row, column);
			}
		}
	}
	
	public Matrix map(Func<int, int, double, double> lambda) {
		
		return new Matrix(
			_width,
			_height,
			(row, column) => lambda(row, column, getValue(row, column))
		);
	}
	
	public Matrix map(Func<double, double> lambda) {
		
		return new Matrix (
			_width,
			_height,
			(row, column) => lambda(getValue(row, column))
		);
	}

	public Matrix dotProduct(Matrix other) {

		return new Matrix(
			this._height,
			other._width,
			(int row, int column) => {

				double sum = 0;

				for (int i = 0; i < this._width; ++i) {

					sum += this.getValue(row, i) * other.getValue(i, column);
				}

				return sum;
			}
		);
	}

	public double getWidth() { return _width; }
	public double getHeight() { return _height; }
	public double getValue(int row, int column) { return _data[row * _width + column]; }

	public bool sameDimensions(Matrix other) { return _width == other._width && _height == other._height; }

	public override string ToString () {

		string s = "[";

		for (int row = 0; row < _height; ++row) {

			s += "[";

			for (int column = 0; column < _width; ++column) {

				s += getValue(row, column);

				if (column < _width - 1) {

					s += ",";
				}
			}

			s += "]";

			if (row < _height - 1) {

				s += ",";
			}
		}

		return s + "]";
	}
	
	public static Matrix operator *(Matrix m1, Matrix m2) {
		
		return m1.map(
			(int row, int column, double value) => value * m2.getValue(row, column)
			);
	}
	
	public static Matrix operator *(Matrix m, double d) {
		
		return m.map(
			(double value) => value * d
			);
	}
	
	public static Matrix operator /(Matrix m1, Matrix m2) {
		
		return m1.map(
			(int row, int column, double value) => value / m2.getValue(row, column)
		);
	}
	
	public static Matrix operator /(Matrix m, double d) {
		
		return m.map(
			(double value) => value / d
		);
	}
	
	public static Matrix operator -(Matrix m1, Matrix m2) {
		
		return m1.map(
			(int row, int column, double value) => value - m2.getValue(row, column)
		);
	}
	
	public static Matrix operator -(Matrix m, double d) {
		
		return m.map(
			(double value) => value - d
		);
	}
	
	public static Matrix operator +(Matrix m1, Matrix m2) {
		
		return m1.map(
			(int row, int column, double value) => value + m2.getValue(row, column)
		);
	}
	
	public static Matrix operator +(Matrix m, double d) {
		
		return m.map(
			(double value) => value + d
		);
	}

	public static Matrix RandomDist(int width, int height) {

		System.Random r = new System.Random();

		return new Matrix(
			width,
			height,
			(x, y) => {

				// A Box-Muller transfrom for a quick distribution

				double uniform1 = r.NextDouble();
				double uniform2 = r.NextDouble();

				double sqrd = -2 * Math.Log(uniform1) * Math.Sin(2 * Math.PI * uniform2);

				double sqrt = Mathf.Sqrt((float) sqrd);

				return Double.IsNaN(sqrt) ? 0 : sqrt;
			}
		);
	}

	public static Matrix Zerod(int width, int height) {

		return new Matrix(width, height, (x, y) => 0);
	}
}
