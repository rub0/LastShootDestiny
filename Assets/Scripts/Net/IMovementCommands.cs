using UnityEngine;
using System.Collections;

/**
 * Right now its composed of just one method. Instead of inheritance
 * we could have used callbacks for this simple behaviour. For the
 * moment, lets just keep it like this.
 */
public interface IMovementCommands
{
    
    /** 
     * Needs to be overriden by derived classes. The aim here is to
     * separate commands from implementation. That way is fairly easy
     * to swap between different controllers and IAs.
     * 
     * We could have used a mask of enumerated values. I went ahead and
     * used a simple vector3 to avoid conversions when processing input
     * in the movement component.
     */
    Vector3 updateMovement();

    bool getButtonJumpUp();

    bool startButton();

    int getNumJoysticks();

    bool getOKButton();

    bool getCancelButton();
}
