using System;
using Godot;

public interface Interactable
{
    public void interact(Camera3D camera);
    public String getInteractType();
}