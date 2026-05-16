using System.Numerics;

namespace RenData.PhysDef;

public class Phys3
{
    public const float DEFAULT_STEP_HEIGHT = 0.25f;                     // the distance an object will "step up" over an obstacle
    public static readonly float DEFAULT_SLIDE_ANGLE = WWMath.DEG_TO_RADF(45.0f);    // steepest angle the character can walk up
    public const float DEFAULT_NORMALIZED_SPEED = 10.0f;

    public const float GROUND_DISTANCE = 0.1f;                          // On ground if within this distance
    public const float GROUND_EPSILON = (GROUND_DISTANCE) / 5.0f;   // Stop at this distance from ground
    public const float WALL_EPSILON = 0.5f;//(GROUND_DISTANCE - 0.001f);	// Stop at this distance from walls/slides

    public const float MIN_STEP_MOVE = 0.25f;                               // only try to step if we could move this distance
    public const float MAX_STEP_MOVE_ANGLE_TAN = 1.0f;                  // only step if moving at an angle close to the x-y plane

    // Debug Vector colors
    public static readonly Vector3 VELOCITY_COLOR = new(1, 0, 0);               // color for the velocity debug vector
    public static readonly Vector3 CONTACT_COLOR = new(0.25f, 0.7f, 0.2f);  // color for contact vectors
    public static readonly Vector3 GROUND_COLOR = new(0.0f, 1.0f, 1.0f);
}
