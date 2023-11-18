using Godot;
using System;

public partial class main : Node2D
{
	[ExportCategory("Meter To Pixel Scale")]
	[Export] public float scaleInPixels = 100; // 1 meter = this number (Default is 1:100).
	Label speedLabel;
	RigidBody2D body;
	rigid_body_2d bodyScript;
	LineEdit initialField;
	float intialVelocity;

	public override void _Ready()
	{
		speedLabel = GetNode<Label>("CanvasLayer/Control/MarginContainer/VBoxContainer/Speed");
		body = GetNode<RigidBody2D>("RigidBody2D");
		bodyScript = body as rigid_body_2d;
		initialField = GetNode<LineEdit>("CanvasLayer/Control/MarginContainer/VBoxContainer/InitialField");
		initialField.Editable = false;
		intialVelocity = initialField.Text.ToFloat();
		
		bodyScript.scaleInPixels = scaleInPixels;
	}

	public override void _PhysicsProcess(double delta)
	{
		speedLabel.Text = $"Speed: {(body.LinearVelocity.Y == -0 ? 0 : -body.LinearVelocity.Y / scaleInPixels):F1}m/s";
		
		intialVelocity = initialField.Text.ToFloat() * 100;
		
		ResetHighestY();
	}
	
	public void ResetHighestY()
	{
		if (Input.IsKeyPressed(Key.Space) && body.LinearVelocity.Y == 0)
		{
			body.LinearVelocity = new Vector2(0, -intialVelocity);
			bodyScript.ResetHighestY();
		}
	}
	
	#region IntialField	
	public void _on_initial_field_mouse_entered()
	{
		initialField.Editable = true;
		initialField.FocusMode = Control.FocusModeEnum.Click;
	}
	public void _on_initial_field_mouse_exited()
	{
		initialField.Editable = false;
		initialField.FocusMode = Control.FocusModeEnum.None;
	}
	public void _on_initial_field_text_changed(string newText)
	{
		string numericText = "";
		foreach (char c in newText)
		{
			if (Char.IsDigit(c) || c.ToString() == ".")
			{
				numericText += c;
			}
		}
		
		if (newText != numericText)
		{
			initialField.Text = numericText;
			initialField.CaretColumn = initialField.Text.Length;
		}
	}
	#endregion
}
