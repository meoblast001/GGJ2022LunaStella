using Godot;
using Magnetism;

namespace Magnet {
  public class Magnet : RigidBody, IMagnetic {
    public void ApplyMagneticForce(Vector3 direction) {
      GD.Print($"Magnet had force applied in direction ${direction}");
      this.AddCentralForce(direction * 1000);
    }
  }
}
