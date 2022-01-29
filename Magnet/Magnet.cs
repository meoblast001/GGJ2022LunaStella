using Godot;
using Magnetism;

namespace Magnet {
  public class Magnet : RigidBody, IMagnetic {
    public void ApplyRepellingForce(Vector3 direction) {
      this.AddCentralForce(direction * 500);
    }

    public void ApplyAttractionForce(Vector3 direction) {
      this.AddCentralForce(direction * 10);
    }
  }
}
