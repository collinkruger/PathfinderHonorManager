﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Incoming = PathfinderHonorManager.Dto.Incoming;
using Outgoing = PathfinderHonorManager.Dto.Outgoing;
using PathfinderHonorManager.Model;
using PathfinderHonorManager.DataAccess;
using PathfinderHonorManager.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PathfinderHonorManager.Service
{
    public class PathfinderHonorService : IPathfinderHonorService
    {
        private readonly PathfinderContext _dbContext;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;

        private readonly IValidator<Incoming.PathfinderHonorDto> _validator;


        public PathfinderHonorService(
            PathfinderContext context,
            IMapper mapper,
            IValidator<Incoming.PathfinderHonorDto> validator,
            ILogger<PathfinderHonorService> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }


        public async Task<Outgoing.PathfinderHonorChildDto> GetByIdAsync(Guid pathfinderId, Guid pathfinderHonorId, CancellationToken token)
        {
            PathfinderHonor entity;

            entity = await GetFilteredPathfinderHonors(pathfinderId, pathfinderHonorId, token)
                .Include(phs => phs.PathfinderHonorStatus)
                .Include(h => h.Honor)
                .SingleOrDefaultAsync();

            return entity == default
                ? default
                : _mapper.Map<Outgoing.PathfinderHonorChildDto>(entity);
        }

        public async Task<Outgoing.PathfinderHonorChildDto> AddAsync(Incoming.PathfinderHonorDto newPathfinderHonor, CancellationToken token)
        {
            await _validator.ValidateAsync(
                newPathfinderHonor,
                opts => opts.ThrowOnFailures(),
                token);

            var newEntity = _mapper.Map<PathfinderHonor>(newPathfinderHonor);

            await _dbContext.AddAsync(newEntity, token);
            await _dbContext.SaveChangesAsync(token);
            _logger.LogInformation($"Pathfinder honor(Id: {newEntity.PathfinderHonorID} added to database.");

            return _mapper.Map<Outgoing.PathfinderHonorChildDto>(newEntity);
        }

        public IQueryable<PathfinderHonor> GetFilteredPathfinderHonors(Guid pathfinderId, Guid pathfinderHonorId,CancellationToken token)
        { 
            return pathfinderId == null
            ? _dbContext.PathfinderHonors
            : _dbContext.PathfinderHonors
                .Where(p => p.PathfinderID == pathfinderId)
                .Where(ph => ph.PathfinderHonorID == pathfinderHonorId);
        }
    }
}
