using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Side {
	Up, Down, Left, Right, Center, None
}

public static class SideMethods {

	public static int DeltaX(this Side s) {
		switch (s) {
			case Side.Left: return -1;
			case Side.Right: return 1;
			default: return 0;
		}
	}

	public static int DeltaY(this Side s) {
		switch (s) {
			case Side.Up: return -1;
			case Side.Down: return 1;
			default: return 0;
		}
	}

	public static Side Opposite(this Side s) {
		switch (s) {
			case Side.Up: return Side.Down;
			case Side.Down: return Side.Up;
			case Side.Left: return Side.Right;
			case Side.Right: return Side.Left;
		}
		throw new Exception("Opposite for center?");
	}

	public static float ToRotation(this Side s) {
		switch (s) {
			case Side.Left: return 90;
			case Side.Up: return 0;
			case Side.Right: return -90;
			case Side.Down: return 180;
		}
		throw new Exception("Rotation what? " + s);
	}
}