using UnityEngine;

/// <summary>
/// Cho khung biet player dang o dau ma khong can biet class player cua game.
/// Class player cua game implement roi dang ky: PlayerBootstrap.Register&lt;IPlayerRoot&gt;(this).
/// </summary>
public interface IPlayer
{
    Transform Root { get; }
}
