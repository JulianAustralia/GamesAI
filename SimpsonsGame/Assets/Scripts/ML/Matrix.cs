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

	private void _testCases() {

		if (true) {
			double[] xd = {0, 4, -2, -4, -3, 0};
			Matrix x = new Matrix(2, 3, xd);

			double[] yd = {0, 1, 1, -1, 2, 3};
			Matrix y = new Matrix(3, 2, yd);

			double[] dd = {0, -10, -3, -1};
			Matrix dp = new Matrix(2, 2, dd);

			Matrix xy = x.dotProduct(y);

			if (dp.sameDimensions(xy) && dp.ToString() == xy.ToString()) {

				Debug.Log("DOT PRODUCT IS CORRECT");
			} else {

				Debug.Log("DOT PRODUCT DIDN'T WORK");
			}
		}

		if (true) {
			double[] xd = {0, 4, -2, -4, -3, 0};
			Matrix x = new Matrix(2, 3, xd);

			double[] yd = {0, 1, 1, -1, 2, 3};
			Matrix y = new Matrix(2, 3, yd);

			double[] sd = {0, 5, -1, -5, -1, 3};
			Matrix s = new Matrix(2, 3, sd);

			Matrix xy = x + y;

			if (s.sameDimensions(xy) && s.ToString() == xy.ToString()) {

				Debug.Log("SUM IS CORRECT");
			} else {

				Debug.Log("SUM DIDN'T WORK");
			}
		}

		if (true) {
			double[] xd = {0, 4, -2, -4, -3, 0};
			Matrix x = new Matrix(2, 3, xd);

			double[] yd = {0, 1, 1, -1, 2, 3};
			Matrix y = new Matrix(2, 3, yd);

			double[] pd = {0, 4, -2, 4, -6, 0};
			Matrix p = new Matrix(2, 3, pd);

			Matrix xy = x * y;

			if (p.sameDimensions(xy) && p.ToString() == xy.ToString()) {

				Debug.Log("PRODUCT IS CORRECT");
			} else {

				Debug.Log("PRODUCT DIDN'T WORK");
			}

			for (int y1 = 0; y1 < 2; ++y1) {

				for (int x1 = 0; x1 < 3; ++x1) {

					if (xy.getValue(x1, y1) != p.getValue(x1, y1)) {
						Debug.Log("GetValue " + x1 + " " + y1);
						Debug.Log("expected " + p.getValue(x1, y1) + " got " + xy.getValue(x1, y1));
					}
				}
			}
		}
	}
	
	public Matrix(int rows, int columns, Func<int, int, double> lambda) {

		_rows = rows;
		_columns = columns;
		_size = rows * columns;
		_data = new double[_size];
		
		for (int y = 0; y < rows; ++y) {

			for (int x = 0; x < columns; ++x) {
				
				_data[y * columns + x] = lambda(x, y);
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
			(x, y) => lambda(x, y, getValue(x, y))
		);
	}
	
	public Matrix map(Func<double, double> lambda) {
		
		return new Matrix (
			_rows,
			_columns,
			(x, y) => lambda(getValue(x, y))
		);
	}

	public Matrix dotProduct(Matrix other) {

		if (this._columns != other._rows)
			throw new Exception("Matrix dotProduct. has " + this._rows + ", " + this._columns + " " + other._rows + ", " + other._columns);

		return new Matrix(
			this._rows,
			other._columns,
			(int x, int y) => {

				double sum = 0;

				for (int i = 0; i < this._columns; ++i) {

					sum += this.getValue(i, y) * other.getValue(x, i);
				}

				return sum;
			}
		);
	}

	public int getRows() { return _rows; }
	public int getColumns() { return _columns; }
	public double getValue(int x, int y) { return _data[y * _columns + x]; }

	public bool sameDimensions(Matrix other) { return _rows == other._rows && _columns == other._columns; }

	public override string ToString () {

		string s = "[";

		for (int y = 0; y < _rows; ++y) {

			s += "[";

			for (int x = 0; x < _columns; ++x) {

				s += getValue(x, y);

				if (x + 1 < _columns) {

					s += ",";
				}
			}

			s += "]";

			if (y + 1 < _rows) {

				s += ",";
			}
		}

		return s + "]";
	}
	
	public static Matrix operator *(Matrix m1, Matrix m2) {

		if (!m1.sameDimensions(m2))
			throw new Exception("Matrix *");

		return m1.map(
			(int x, int y, double value) => value * m2.getValue(x, y)
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
			(int x, int y, double value) => value / m2.getValue(x, y)
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
			(int x, int y, double value) => value - m2.getValue(x, y)
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
			(int x, int y, double value) => value + m2.getValue(x, y)
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

				return Double.IsNaN(sqrt) ? 0 : r.NextDouble() < .5 ? -sqrt : sqrt;
			}
		);
	}

	public static Matrix Zerod(int width, int height) {

		return new Matrix(width, height, (x, y) => 0);
	}
}
