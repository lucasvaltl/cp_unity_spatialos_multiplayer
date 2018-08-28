using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable;
using Improbable.Core;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

[WorkerType(WorkerPlatform.UnityWorker)]
public class PlayerMover : MonoBehaviour {

    [Require] private Position.Writer PositionWriter;
    [Require] private Rotation.Writer RotationWriter;
    [Require] private PlayerInput.Reader PlayerInputReader;
    public Vector3 SpawnPoint = new Vector3(0.0f,5.0f,0.0f);

    private Rigidbody rigidbody;
    private Transform transform;

	void OnEnable ()
  {
        rigidbody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
	}

	void FixedUpdate ()
  {

        var joystick = PlayerInputReader.Data.joystick;
        var direction = new Vector3(joystick.xAxis, 0, joystick.yAxis);
				if (direction.sqrMagnitude > 1)
	 			{
			 	direction.Normalize();
	 			}
        rigidbody.AddForce(direction * SimulationSettings.PlayerAcceleration);


        var pos = rigidbody.position;
        if(pos.y < -10.0f){
            Debug.Log("playermover: "+ pos.y);
            rigidbody.isKinematic = true;
            transform.position = SpawnPoint;
            rigidbody.isKinematic = false;
        }
        var positionUpdate = new Position.Update()
            .SetCoords(new Coordinates(pos.x, pos.y, pos.z));
        PositionWriter.Send(positionUpdate);

        var rotationUpdate = new Rotation.Update()
            .SetRotation(rigidbody.rotation.ToNativeQuaternion());
        RotationWriter.Send(rotationUpdate);
	}
}
