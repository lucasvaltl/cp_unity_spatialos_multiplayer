using Assets.Gamelogic.Utils;
using Improbable;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

[WorkerType(WorkerPlatform.UnityWorker)]
public class TransformSender : MonoBehaviour
{

    [Require] private Position.Writer PositionWriter;
    [Require] private Rotation.Writer RotationWriter;

    void Update ()
    {
        var pos = transform.position;
        // if(pos.y < -10.0f){
        //     Debug.Log("TransformSender: "+ pos.y);
        //     pos =  new Vector3(0.0f,2.0f,0.0f);
        // }
        var positionUpdate = new Position.Update()
            .SetCoords(new Coordinates(pos.x, pos.y, pos.z));
        PositionWriter.Send(positionUpdate);

        var rotationUpdate = new Rotation.Update()
            .SetRotation(MathUtils.ToNativeQuaternion(transform.rotation));
        RotationWriter.Send(rotationUpdate);
    }
}
