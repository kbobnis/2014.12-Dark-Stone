using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Side {
	Up, Down, Left, Right, None, UpLeft, UpRight, DownLeft, DownRight
}

public static class SideMethods {

	public static List<Side> AllSides() {
		List<Side> l = new List<Side>() { Side.UpLeft, Side.UpRight, Side.DownLeft, Side.DownRight };
		l.AddRange(AdjacentSides());
		return l;
	}
	public static List<Side> AdjacentSides() {
		return new List<Side>() { Side.Up, Side.Down, Side.Left, Side.Right };
	}

	public static int DeltaX(this Side s) {
		switch (s) {
			case Side.Left: return -1;
			case Side.Right: return 1;
			default:
				throw new Exception("Implement me");
		}
	}

	public static int DeltaY(this Side s) {
		switch (s) {
			case Side.Up: return -1;
			case Side.Down: return 1;
			default:
				throw new Exception("Implement me");
		}
	}

	public static Side Opposite(this Side s) {
		switch (s) {
			case Side.Up: return Side.Down;
			case Side.Down: return Side.Up;
			case Side.Left: return Side.Right;
			case Side.Right: return Side.Left;
			case Side.UpLeft: return Side.DownRight;
			case Side.UpRight: return Side.DownLeft;
			case Side.DownLeft: return Side.UpRight;
			case Side.DownRight: return Side.UpLeft;
		}
		throw new Exception("Opposite for " + s + "?");
	}

	public static float ToRotation(this Side s) {
		switch (s) {
			case Side.Left: return 90;
			case Side.Up: return 0;
			case Side.Right: return -90;
			case Side.Down: return 180;
			case Side.UpLeft: return 45;
			case Side.UpRight: return -45;
			case Side.DownLeft: return 135;
			case Side.DownRight: return -135;
		}
		throw new Exception("Rotation what? " + s);
	}
}