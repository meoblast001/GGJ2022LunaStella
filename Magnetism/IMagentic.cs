using Godot;

namespace Magnetism {
  public interface IMagnetic {
    void ApplyRepellingForce(Vector3 direction, float delta);
    void ApplyAttractionForce(Vector3 direction, float delta);
  }
}
