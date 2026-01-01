namespace CheeseMods.CheesesBalloonBase.Components;

public class BalloonSpawn : AIUnitSpawn
{
    public BalloonPhysics phyiscs;
    
    [UnitSpawn("Default Waypoint")]
    public Waypoint defaultWaypoint;

    [UnitSpawnAttributeRange("Default Altitude", 100f, 15000f, UnitSpawnAttributeRange.RangeTypes.Float)]
    public float defaultAltitude;

    public override void OnSpawnUnit()
    {
        base.OnSpawnUnit();

        if (defaultWaypoint != null)
        {
            phyiscs.SetTargetPos(defaultWaypoint.globalPoint);
        }
        phyiscs.SetTargetAlt(defaultAltitude);
    }

    [VTEvent("Set Waypoint", "Command the baloon to magically move towards a waypoint.", new string[]
    {
        "Waypoint"
    })]
    public void SetOrbitNow(Waypoint wpt)
    {
        phyiscs.SetTargetPos(wpt.globalPoint);
    }

    [VTEvent("Set Altitude", "Set the target altitude.", new string[]
    {
        "Altitude"
    })]
    public void SetAltitude([VTRangeParam(100f, 15000f)] float alt)
    {
        phyiscs.SetTargetAlt(alt);
    }

    [VTEvent("Drift", "Drift freely with the wind")]
    public void Drift()
    {
        phyiscs.Drift();
    }
}
