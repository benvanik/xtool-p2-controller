namespace P2Controller.p2Command;

internal class p2_laserHead_MoveRequest
{
	public string action { get; }

	public int waitTime { get; set; }

	public int? f { get; set; }

	public double x { get; set; }

	public double y { get; set; }

	public p2_laserHead_MoveRequest()
	{
		action = "go_to";
	}
}
