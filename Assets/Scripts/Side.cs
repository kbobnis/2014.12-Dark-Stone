using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Side {
	Up, Down, Left, Right, None, UpLeft, UpRight, DownLeft, DownRight
}

public static class SideMethods {

	public static List<Side> AllSides() {
		return new List<Side>() {Side.Up, Side.Down, Side.Left, Side.Right , Side.UpLeft, Side.UpRight, Side.DownLeft, Side.DownRight };
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

	public static Side GetSide(int x, int y){
		switch (x) {
			case 0:
				switch (y) {
					case 0: return Side.None;
					case -1: return Side.Up;
					case 1: return Side.Down;
				}
				break;
			case 1:
				switch (y) {
					case 0: return Side.Right;
					case -1: return Side.UpRight;
					case 1: return Side.DownRight;
				}
				break;
			case -1:
				switch (y) {
					case 0: return Side.Left;
					case -1: return Side.UpLeft;
					case 1: return Side.DownLeft;
				}
				break;
		}
		throw new Exception("There is no side for x: " + x + ", y: " + y);
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