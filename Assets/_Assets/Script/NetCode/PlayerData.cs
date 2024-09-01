using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>,INetworkSerializable
{
   public ulong ClientId;
   public int colorId;

    public bool Equals(PlayerData other)
    {
        return other.ClientId== ClientId && colorId == other.colorId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref colorId);
    }
}
