﻿// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Tera.Game.Messages;

namespace Tera.Game
{
    // A player character, including your own
    public class UserEntity : Entity
    {
        public UserEntity(EntityId id)
            : base(id)
        {
        }

        internal UserEntity(SpawnUserServerMessage message)
            : base(message.Id, message.Position, message.Heading)
        {
            Name = message.Name;
            GuildName = message.GuildName;
            RaceGenderClass = message.RaceGenderClass;
            PlayerId = message.PlayerId;
            ServerId = message.ServerId;
        }

        internal UserEntity(LoginServerMessage message)
            : this(message.Id)
        {
            Name = message.Name;
            GuildName = message.GuildName;
            RaceGenderClass = message.RaceGenderClass;
            PlayerId = message.PlayerId;
            ServerId = message.ServerId;
        }

        public string Name { get; set; }
        public string GuildName { get; set; }
        public RaceGenderClass RaceGenderClass { get; set; }
        public uint ServerId { get; set; }
        public uint PlayerId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UserEntity) obj);
        }

        public override string ToString()
        {
            return $"{Name} [{GuildName}]";
        }

        public static Dictionary<string, Entity> ForEntity(Entity entity)
        {
            var entities = new Dictionary<string, Entity>();
            var ownedEntity = entity as IHasOwner;
            while (ownedEntity?.Owner != null)
            {
                if (entity.GetType() == typeof(NpcEntity))
                {
                    entities.Add("source", entity);
                }
                entity = ownedEntity.Owner;
                ownedEntity = entity as IHasOwner;
            }
            entities.Add("root_source", entity);
            if (!entities.ContainsKey("source"))
            {
                entities.Add("source", null);
            }
            return entities;
        }

        public bool Equals(UserEntity other)
        {
            return Id.Equals(other.Id);
        }

        public static bool operator ==(UserEntity a, UserEntity b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object) a == null) || ((object) b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(UserEntity a, UserEntity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}