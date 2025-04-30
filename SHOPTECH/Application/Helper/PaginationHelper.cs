using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Helper
{
    /// <summary>
    /// Helper class for handling pagination functionality
    /// </summary>
    public static class PaginationHelper
{
    /// <summary>
    /// Creates a PageResultDto from an IQueryable source
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <typeparam name="TDto">The DTO type</typeparam>
    /// <param name="source">The IQueryable source</param>
    /// <param name="pageIndex">Current page number (1-based)</param>
    /// <param name="pageSize">Items per page</param>
    /// <param name="mapper">Function to map from entity to DTO</param>
    /// <returns>Paged result with the requested items</returns>
    public static async Task<PageResultDto<TDto>> CreatePagedResultAsync<T, TDto>(
        IQueryable<T> source,
        int pageIndex,
        int pageSize,
        Func<IEnumerable<T>, IEnumerable<TDto>> mapper)
        where T : class
        where TDto : class
    {
        // Ensure valid pagination parameters
        pageIndex = pageIndex <= 0 ? 1 : pageIndex;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        // Get total count
        var totalItems = await source.CountAsync();

        // Apply pagination
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Map to DTOs
        var dtoItems = mapper(items);

        // Create result
        return new PageResultDto<TDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItems = totalItems,
            Items = dtoItems
        };
    }

    /// <summary>
    /// Creates a PageResultDto from an in-memory collection
    /// </summary>
    /// <typeparam name="T">The source type</typeparam>
    /// <typeparam name="TDto">The DTO type</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="pageIndex">Current page number (1-based)</param>
    /// <param name="pageSize">Items per page</param>
    /// <param name="mapper">Function to map from source to DTO</param>
    /// <returns>Paged result with the requested items</returns>
    public static PageResultDto<TDto> CreatePagedResult<T, TDto>(
        IEnumerable<T> source,
        int pageIndex,
        int pageSize,
        Func<IEnumerable<T>, IEnumerable<TDto>> mapper)
        where T : class
        where TDto : class
    {
        // Ensure valid pagination parameters
        pageIndex = pageIndex <= 0 ? 1 : pageIndex;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var enumerable = source as T[] ?? source.ToArray();

        // Get total count
        var totalItems = enumerable.Length;

        // Apply pagination
        var items = enumerable
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        // Map to DTOs
        var dtoItems = mapper(items);

        // Create result
        return new PageResultDto<TDto>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItems = totalItems,
            Items = dtoItems
        };
    }

    /// <summary>
    /// Extension method to paginate IQueryable directly
    /// </summary>
    public static async Task<PageResultDto<TDto>> ToPagedResultAsync<T, TDto>(
        this IQueryable<T> query,
        int pageIndex,
        int pageSize,
        Func<IEnumerable<T>, IEnumerable<TDto>> mapper)
        where T : class
        where TDto : class
    {
        return await CreatePagedResultAsync(query, pageIndex, pageSize, mapper);
    }

    /// <summary>
    /// Extension method to paginate IEnumerable directly
    /// </summary>
    public static PageResultDto<TDto> ToPagedResult<T, TDto>(
        this IEnumerable<T> collection,
        int pageIndex,
        int pageSize,
        Func<IEnumerable<T>, IEnumerable<TDto>> mapper)
        where T : class
        where TDto : class
    {
        return CreatePagedResult(collection, pageIndex, pageSize, mapper);
    }
}
}
