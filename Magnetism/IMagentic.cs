using Godot;

namespace Magnetism {
  public interface IMagnetic {
	  void ApplyRepellingForce(Vector3 direction);
	  void ApplyAttractionForce(Vector3 direction);
  }
}
