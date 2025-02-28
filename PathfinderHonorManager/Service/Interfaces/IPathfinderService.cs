﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Outgoing = PathfinderHonorManager.Dto.Outgoing;
using Incoming = PathfinderHonorManager.Dto.Incoming;

namespace PathfinderHonorManager.Service.Interfaces
{

    public interface IPathfinderService
    {
        Task<Outgoing.PathfinderDependantDto> GetByIdAsync(
            Guid id,
            CancellationToken token);

        Task<ICollection<Outgoing.PathfinderDto>> GetAllAsync(CancellationToken token);

        Task<Outgoing.PathfinderDto> AddAsync(
            Incoming.PathfinderDto newPathfinder,
            CancellationToken token);
    }

}
