using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>,INetworkSerializable
{
   public ulong ClientId;
   public int colorId;
   public FixedString64Bytes playerName;
   public FixedString64Bytes playerId;

    public bool Equals(PlayerData other)
    {
        return other.ClientId== ClientId
        && colorId == other.colorId 
        && playerName==other.playerName
        && playerId==other.playerId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref colorId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref playerId);
    }
}
