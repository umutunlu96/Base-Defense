using System;

[Serializable]
public class PlayerData
{
    public PlayerMovementData playerMovementData;
}

[Serializable]
public class PlayerMovementData
{
    public float MoveSpeed = 10f;
    // public float TurnSpeed = .5f;
}