using Godot;
using Magnetism;

public class MagneticWall : StaticBody, IPassiveMagnetic {
  [Export] private float attractStrength;
  [Export] private float repelStrength;

  public float AttractStrength => attractStrength;
  public float RepelStrength => repelStrength;
}
