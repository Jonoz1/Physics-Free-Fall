using Godot;
using System;

public partial class rigid_body_2d : RigidBody2D
{
	public float scaleInPixels;
	public float highestY;
	public float atT = 0f;
	public float totalT = 0f;
	public float totalD = 0f;
	Viewport rootViewport;
	int rootVPHeight;
	float stringHighestY;

	public override void _Ready()
	{
		highestY = GlobalPosition.Y;
		rootViewport = GetViewport();
		rootVPHeight = (int)rootViewport.GetVisibleRect().Size.Y;
		stringHighestY = highestY + 100;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GlobalPosition.Y < highestY)
		{
			highestY = GlobalPosition.Y;
			stringHighestY = highestY + 100;
			atT += (float)delta;
		}
		
		if (LinearVelocity != Vector2.Zero)
		{
			totalT += (float)delta;
			totalD += Math.Abs(LinearVelocity.Y) * (float)delta / scaleInPixels;
		}
		
		GetNode<Polygon2D>("Polygon2D").GlobalPosition = new Vector2(GlobalPosition.X, highestY);
		GetNode<Label>("Polygon2D/Control/VBoxContainer/HighestLabel").Text = $"Highest Point: {((rootVPHeight - stringHighestY) / scaleInPixels).ToString("F3")}m";
		GetNode<Label>("Polygon2D/Control/VBoxContainer/TimeLabel").Text = $"At t = {atT:F2}s";
		GetNode<Label>("VBoxContainer/TotalTLabel").Text = $"t = {totalT:F2}s";
		GetNode<Label>("VBoxContainer/TotalDLabel").Text = $"D = {totalD:F3}m";
	}
	
	public void ResetHighestY()
	{
		highestY = (int)GlobalPosition.Y;
		atT = 0f;
		totalT = 0f;
		totalD = 0f;
		GetNode<Polygon2D>("Polygon2D").GlobalPosition = new Vector2(GlobalPosition.X, highestY);
	}
}
