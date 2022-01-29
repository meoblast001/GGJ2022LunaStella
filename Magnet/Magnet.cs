using Godot;
using Magnetism;

namespace Magnet {
  public class Magnet : RigidBody, IMagnetic {
	public void ApplyRepellingForce(Vector3 direction) {
	  GD.Print($"Magnet repelling in direction ${direction}");

	  this.AddCentralForce(direction * 2000);
	}
	public void ApplyAttractionForce(Vector3 direction) {
	  GD.Print($"Magnet attracting in direction ${direction}");

	  this.AddCentralForce(direction * 10);
	}
  }
}
