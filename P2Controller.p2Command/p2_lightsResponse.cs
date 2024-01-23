using System.Collections.Generic;

namespace P2Controller.p2Command;

internal class p2_lightsResponse
{
	public int code { get; set; }

	public List<p2_light> data { get; set; }

	public string msg { get; set; }
}
