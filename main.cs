using Godot;
using System;

public partial class main : Node2D
{
	[ExportCategory("Meter To Pixel Scale")]
	[Export] public float scaleInPixels = 100; // 1 meter = this number (Default is 1:100).
	Label speedLabel;
	RigidBody2D body;
	rigid_body_2d bodyScript;
	LineEdit iVelocityField;
	float intialVelocity;
	LineEdit iHeightField;
	float initialHeight;
	bool isMoving = false;

	public override void _Ready()
	{
		speedLabel = GetNode<Label>("CanvasLayer/Control/MarginContainer/VBoxContainer/Speed");
		body = GetNode<RigidBody2D>("RigidBody2D");
		bodyScript = body as rigid_body_2d;
		
		iVelocityField = GetNode<LineEdit>("CanvasLayer/Control/MarginContainer/VBoxContainer/IVelocityField");
		iVelocityField.Editable = false;
		
		iHeightField = GetNode<LineEdit>("CanvasLayer/Control/MarginContainer/VBoxContainer/IHeightField"); 
		iHeightField.Editable = false;
		
		bodyScript.scaleInPixels = scaleInPixels;
	}

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("reset"))
		{
			body.LinearVelocity = new Vector2(0, 0);
			body.GlobalPosition = initialHeight >= 0 ? new Vector2(640, 620.3f) : new Vector2(640, initialHeight);
			bodyScript.ResetHighestY();
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		speedLabel.Text = $"Speed: {(body.LinearVelocity.Y == -0 ? 0 : -body.LinearVelocity.Y / scaleInPixels):F1}m/s";
		
		intialVelocity = iVelocityField.Text == "" ? 0 : iVelocityField.Text.ToFloat() * 100;
		
		initialHeight = iHeightField.Text == "" ? 0 : -(iHeightField.Text.ToFloat() * scaleInPixels) + 620.3f;
		
		// GD.Print(body.GlobalPosition);
		
		HandleIsMoving();
		
		if (isMoving)
		{
			ResetHighestY();
		}
	}
	
	public void ResetHighestY()
	{
		if (Input.IsActionJustPressed("jump") && body.GlobalPosition == new Vector2(640, 620.3f))
		{
			body.LinearVelocity = new Vector2(0, -intialVelocity);
			body.GlobalPosition = initialHeight >= 0 ? new Vector2(640, 620.3f) : new Vector2(640, initialHeight);
			bodyScript.ResetHighestY();
		}
	}
	
	public void HandleIsMoving()
	{
		if (Input.IsActionJustPressed("jump"))
		{
			if (body.LinearVelocity == Vector2.Zero && ((body.GlobalPosition == new Vector2(640, 620.3f)) || body.GlobalPosition == new Vector2(640, initialHeight)))
			{
				isMoving = true;
				GD.Print("1");
			} else if (iVelocityField.Text == "0" || iVelocityField.Text == "")
			{
				isMoving = true;
				GD.Print("2");
			} else
			{
				isMoving = true;
				GD.Print("3");
			}
		}
			
	
		if (!isMoving)
		{
			body.LinearVelocity = Vector2.Zero;
			body.GlobalPosition = initialHeight >= 0 ? new Vector2(640, 620.3f) : new Vector2(640, initialHeight);
		}
	}
	
	#region iVelocityField	
	public void _on_initial_field_mouse_entered()
	{
		iVelocityField.Editable = true;
		iVelocityField.FocusMode = Control.FocusModeEnum.Click;
	}
	public void _on_initial_field_mouse_exited()
	{
		iVelocityField.Editable = false;
		iVelocityField.FocusMode = Control.FocusModeEnum.None;
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
			iVelocityField.Text = numericText;
			iVelocityField.CaretColumn = iVelocityField.Text.Length;
		}
	}
	#endregion
	#region iHeightField
		public void _on_i_height_field_mouse_entered()
	{
		iHeightField.Editable = true;
		iHeightField.FocusMode = Control.FocusModeEnum.Click;
	}
	public void _on_i_height_field_mouse_exited()
	{
		iHeightField.Editable = false;
		iHeightField.FocusMode = Control.FocusModeEnum.None;
	}
	public void _on_i_height_field_text_changed(string newText)
	{
		string numericText = "";
		foreach (char c in newText)
		{
			if (char.IsDigit(c) || c.ToString() == ".")
			{
				numericText += c;
			}
		}
		
		if (newText != numericText)
		{
			iHeightField.Text = numericText;
			iHeightField.CaretColumn = iHeightField.Text.Length;
		}
	}
		
	#endregion

	public void _on_reset_at_start_timeout()
	{
		body.LinearVelocity = new Vector2(0, 0);
		body.GlobalPosition = initialHeight >= 0 ? new Vector2(640, 620.3f) : new Vector2(640, initialHeight);
		bodyScript.ResetHighestY();
	}

}
