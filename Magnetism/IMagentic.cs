using Godot;

namespace Magnetism {
  public interface IMagnetic {
    void ApplyMagneticForce(Vector3 direction);
  }
}