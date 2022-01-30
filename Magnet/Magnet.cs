using Godot;
using Magnetism;

namespace Magnet {
  public class Magnet : RigidBody, IMagnetic {
    [Export] private float attractStrength;
    [Export] private float repelStrength;

    public void ApplyRepellingForce(Vector3 direction, float delta) {
      this.AddCentralForce(direction * attractStrength * delta);
    }

    public void ApplyAttractionForce(Vector3 direction, float delta) {
      this.AddCentralForce(direction * repelStrength * delta);
    }
  }
}
