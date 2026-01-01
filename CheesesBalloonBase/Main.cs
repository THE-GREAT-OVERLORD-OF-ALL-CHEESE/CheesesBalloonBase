using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using UnityEngine;

namespace CheeseMods.CheesesBalloonBase;

[ItemId("cheese.balloonbase")]
public class Main : VtolMod
{
    public string ModFolder;

    private void Awake()
    {
        Debug.Log("Cheese's Balloon Base: Ready");
    }

    public override void UnLoad()
    {
        Debug.Log("Cheese's Balloon Base: Nothing to unload");
    }
}