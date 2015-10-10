using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Matrix {

	private int _rows;
	private int _columns;
	private int _size;
	private double[] _data;
	
	public Matrix(int rows, int columns, Func<int, int, double> lambda) {
		
		_rows = rows;
		_columns = columns;
		_size = rows * columns;
		_data = new double[_size];
		
		for (int row = 0; row < rows; ++row) {

			for (int column = 0; column < columns; ++column) {
				
				_data[row * columns + column] = lambda(row, column);
			}
		}
	}

	public Matrix(int rows, int columns, double [] values) {

		if (rows * columns != values.Length)
			throw new Exception("Matrix int width, int height, double [] values");

		_rows = rows;
		_columns = columns;
		_size = rows * columns;
		_data = new double[_size];

		for (int i = 0; i <_size; ++i) {

			_data[i] = values[i];
		}
	}
	
	public Matrix map(Func<int, int, double, double> lambda) {
		
		return new Matrix(
			_rows,
			_columns,
			(row, column) => lambda(row, column, getValue(row, column))
		);
	}
	
	public Matrix map(Func<double, double> lambda) {
		
		return new Matrix (
			_rows,
			_columns,
			(row, column) => lambda(getValue(row, column))
		);
	}

	public Matrix dotProduct(Matrix other) {

		if (this._columns != other._rows)
			throw new Exception("Matrix dotProduct. has " + this._rows + ", " + this._columns + " " + other._rows + ", " + other._columns);

		return new Matrix(
			this._rows,
			other._columns,
			(int row, int column) => {

				double sum = 0;

				for (int i = 0; i < this._columns; ++i) {

					sum += this.getValue(row, i) * other.getValue(i, column);
				}

				return sum;
			}
		);
	}

	public int getRows() { return _rows; }
	public int getColumns() { return _columns; }
	public double getValue(int row, int column) { return _data[row * _columns + column]; }

	public bool sameDimensions(Matrix other) { return _rows == other._rows && _columns == other._columns; }

	public override string ToString () {

		string s = "[";

		for (int row = 0; row < _rows; ++row) {

			s += "[";

			for (int column = 0; column < _columns; ++column) {

				s += getValue(row, column);

				if (column < _columns - 1) {

					s += ",";
				}
			}

			s += "]";

			if (row < _rows - 1) {

				s += ",";
			}
		}

		return s + "]";
	}
	
	public static Matrix operator *(Matrix m1, Matrix m2) {

		if (!m1.sameDimensions(m2))
			throw new Exception("Matrix *");

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

		if (!m1.sameDimensions(m2))
			throw new Exception("Matrix /");

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

		if (!m1.sameDimensions(m2))
			throw new Exception("Matrix -");

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
		
		if (!m1.sameDimensions(m2))
			throw new Exception("Matrix +");

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
